using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LINGYUN.Abp.OperationRateLimiting.Algorithms.SlidingWindow;

/// <summary>
/// Sliding window algorithm: approximates the count over the last window with aligned segments.
/// </summary>
public class SlidingWindowRateLimitingRule : OperationRateLimitingRuleBase
{
    public SlidingWindowRateLimitingRule(IOperationRateLimitingStateAccessor stateAccessor)
        : base(stateAccessor)
    {
    }

    public override async Task<OperationRateLimitingStoreResult> ExecuteAsync(
        OperationRateLimitingRuleContext context)
    {
        var maxCount = context.Rule.MaxCount;
        var nowMs = context.Now.ToUnixTimeMilliseconds();
        var windowMs = Math.Max(1L, (long)context.Rule.Duration.TotalMilliseconds);
        var segmentCount = Math.Max(2, context.Rule.GetSegments());
        var segmentMs = Math.Max(1L, windowMs / segmentCount);
        var currentSegmentStartMs = nowMs - nowMs % segmentMs;
        var expiration = TimeSpan.FromMilliseconds(windowMs * 2 + segmentMs);

        if (!context.Consume)
        {
            var state = await StateAccessor.GetStateAsync<SlidingWindowRateLimitingState>(
                context.Key,
                context.CancellationToken);
            var evaluation = Evaluate(
                state, nowMs, windowMs, segmentMs, segmentCount, currentSegmentStartMs, maxCount, consume: false);

            return evaluation.Allowed
                ? OperationRateLimitingStoreResult.Allow(maxCount, evaluation.Count, evaluation.RetryAfter, context.Rule.Duration)
                : OperationRateLimitingStoreResult.Deny(maxCount, evaluation.Count, evaluation.RetryAfter, context.Rule.Duration);
        }

        var result = default(SlidingWindowEvaluation);
        await StateAccessor.UpdateStateAsync(
            context.Key,
            (SlidingWindowRateLimitingState? current) =>
            {
                result = Evaluate(
                    current, nowMs, windowMs, segmentMs, segmentCount, currentSegmentStartMs, maxCount, consume: true);

                return new SlidingWindowRateLimitingState
                {
                    SegmentStartUnixMs = result.Segments.Select(x => x.StartUnixMs).ToArray(),
                    SegmentCounts = result.Segments.Select(x => x.Count).ToArray()
                };
            },
            expiration,
            context.CancellationToken);

        return result.Allowed
            ? OperationRateLimitingStoreResult.Allow(maxCount, result.Count, result.RetryAfter, context.Rule.Duration)
            : OperationRateLimitingStoreResult.Deny(maxCount, result.Count, result.RetryAfter, context.Rule.Duration);
    }

    private static SlidingWindowEvaluation Evaluate(
        SlidingWindowRateLimitingState? state,
        long nowMs,
        long windowMs,
        long segmentMs,
        int segmentCount,
        long currentSegmentStartMs,
        int maxCount,
        bool consume)
    {
        var segments = new List<SlidingWindowSegment>();

        if (state != null)
        {
            var length = Math.Min(state.SegmentStartUnixMs.Length, state.SegmentCounts.Length);

            for (var i = 0; i < length; i++)
            {
                var start = state.SegmentStartUnixMs[i];
                var count = state.SegmentCounts[i];

                if (start > nowMs || start <= nowMs - windowMs)
                {
                    continue;
                }

                segments.Add(new SlidingWindowSegment(start, count));
            }
        }

        segments.Sort((left, right) => left.StartUnixMs.CompareTo(right.StartUnixMs));
        segments.RemoveAll(segment => segment.StartUnixMs > currentSegmentStartMs);

        if (segments.Count == 0 || segments[^1].StartUnixMs != currentSegmentStartMs)
        {
            segments.Add(new SlidingWindowSegment(currentSegmentStartMs, 0));
        }

        while (segments.Count > segmentCount)
        {
            segments.RemoveAt(0);
        }

        var total = segments.Sum(segment => segment.Count);
        var allowed = total < maxCount;

        if (allowed && consume)
        {
            segments[^1] = new SlidingWindowSegment(segments[^1].StartUnixMs, segments[^1].Count + 1);
            total++;
        }

        long retryMs = 0;

        if (!allowed)
        {
            var needToRemove = total - maxCount + 1;
            long removed = 0;

            foreach (var segment in segments)
            {
                removed += segment.Count;
                if (removed >= needToRemove)
                {
                    retryMs = segment.StartUnixMs + windowMs - nowMs;
                    break;
                }
            }

            if (retryMs <= 0)
            {
                retryMs = segmentMs;
            }
        }
        else if (segments.Count > 0)
        {
            retryMs = segments[0].StartUnixMs + windowMs - nowMs;
            if (retryMs <= 0)
            {
                retryMs = segmentMs;
            }
        }

        return new SlidingWindowEvaluation
        {
            Allowed = allowed,
            Count = (int)total,
            Segments = segments,
            RetryAfter = TimeSpan.FromMilliseconds(Math.Max(1, retryMs))
        };
    }

    private readonly struct SlidingWindowSegment
    {
        public long StartUnixMs { get; }

        public long Count { get; }

        public SlidingWindowSegment(long startUnixMs, long count)
        {
            StartUnixMs = startUnixMs;
            Count = count;
        }
    }

    private readonly struct SlidingWindowEvaluation
    {
        public bool Allowed { get; init; }

        public int Count { get; init; }

        public List<SlidingWindowSegment> Segments { get; init; }

        public TimeSpan RetryAfter { get; init; }
    }
}
