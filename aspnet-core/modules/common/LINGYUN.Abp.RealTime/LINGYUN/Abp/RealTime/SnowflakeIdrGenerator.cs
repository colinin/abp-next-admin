using Microsoft.Extensions.Options;
using System;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace LINGYUN.Abp.RealTime
{
    // reference: https://github.com/dotnetcore/CAP
    // reference: https://blog.csdn.net/lq18050010830/article/details/89845790
    public class SnowflakeIdrGenerator : ISnowflakeIdrGenerator, ISingletonDependency
    {
        public const long Twepoch = 1288834974657L;

        private static readonly object _lock = new object();
        private long _lastTimestamp = -1L;

        protected long MaxWorkerId { get; set; }
        protected long MaxDatacenterId { get; set; }

        protected int WorkerIdShift { get; }
        protected int DatacenterIdShift { get; }
        protected int TimestampLeftShift { get; }
        protected long SequenceMask { get; }

        protected SnowflakeIdOptions Options { get; }

        public SnowflakeIdrGenerator(IOptions<SnowflakeIdOptions> options)
        {
            Options = options.Value;

            WorkerIdShift = Options.SequenceBits;
            DatacenterIdShift = Options.SequenceBits + Options.WorkerIdBits;
            TimestampLeftShift = Options.SequenceBits + Options.WorkerIdBits + Options.DatacenterIdBits;
            SequenceMask = -1L ^ (-1L << Options.SequenceBits);
        }

        internal void Initialize(long sequence = 0L)
        {
            Sequence = sequence;
            MaxWorkerId = -1L ^ (-1L << Options.WorkerIdBits);
            MaxDatacenterId = -1L ^ (-1L << Options.DatacenterIdBits);

            if (!int.TryParse(Environment.GetEnvironmentVariable("WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
            {
                workerId = RandomHelper.GetRandom((int)MaxWorkerId);
            }

            if (!int.TryParse(Environment.GetEnvironmentVariable("DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
            {
                datacenterId = RandomHelper.GetRandom((int)MaxDatacenterId);
            }

            if (workerId > MaxWorkerId || workerId < 0)
                throw new ArgumentException($"worker Id can't be greater than {MaxWorkerId} or less than 0");

            if (datacenterId > MaxDatacenterId || datacenterId < 0)
                throw new ArgumentException($"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");

            WorkerId = workerId;
            DatacenterId = datacenterId;
            
        }

        public long WorkerId { get; protected set; }
        public long DatacenterId { get; protected set; }
        public long Sequence { get; protected set; }

        public virtual long Create()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                    throw new Exception(
                        $"InvalidSystemClock: Clock moved backwards, Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");

                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0) timestamp = TilNextMillis(_lastTimestamp);
                }
                else
                {
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | Sequence;

                return id;
            }
        }

        protected virtual long TilNextMillis(long lastTimestamp)
        {
            var timestamp = TimeGen();
            while (timestamp <= lastTimestamp) timestamp = TimeGen();
            return timestamp;
        }

        protected virtual long TimeGen()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
