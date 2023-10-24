using System;
using Volo.Abp;

namespace LINGYUN.Abp.IdGenerator.Snowflake
{
    // reference: https://github.com/dotnetcore/CAP
    // reference: https://blog.csdn.net/lq18050010830/article/details/89845790
    public class SnowflakeIdGenerator : IDistributedIdGenerator
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

        private SnowflakeIdGenerator(SnowflakeIdOptions options)
        {
            Options = options;
            WorkerIdShift = options.SequenceBits;
            DatacenterIdShift = options.SequenceBits + options.WorkerIdBits;
            TimestampLeftShift = options.SequenceBits + options.WorkerIdBits + options.DatacenterIdBits;
            SequenceMask = -1L ^ (-1L << options.SequenceBits);
        }

        public static SnowflakeIdGenerator Create(SnowflakeIdOptions options)
        {
            var idGenerator = new SnowflakeIdGenerator(options)
            {
                WorkerId = options.WorkerId,
                DatacenterId = options.DatacenterId,
                Sequence = options.Sequence,
                MaxWorkerId = -1L ^ (-1L << options.WorkerIdBits),
                MaxDatacenterId = -1L ^ (-1L << options.DatacenterIdBits)
            };

            if (idGenerator.WorkerId <= 0 || options.WorkerId > idGenerator.MaxWorkerId)
            {
                if (!int.TryParse(Environment.GetEnvironmentVariable("WORKERID", EnvironmentVariableTarget.Machine), out var workerId))
                {
                    workerId = RandomHelper.GetRandom((int)idGenerator.MaxWorkerId);
                }

                if (workerId > idGenerator.MaxWorkerId || workerId < 0)
                {
                    throw new ArgumentException($"worker Id can't be greater than {idGenerator.MaxWorkerId} or less than 0");
                }

                idGenerator.WorkerId = workerId;
            }

            if (idGenerator.DatacenterId <= 0 || options.DatacenterId > idGenerator.MaxDatacenterId)
            {
                if (!int.TryParse(Environment.GetEnvironmentVariable("DATACENTERID", EnvironmentVariableTarget.Machine), out var datacenterId))
                {
                    datacenterId = RandomHelper.GetRandom((int)idGenerator.MaxDatacenterId);
                }

                if (datacenterId > idGenerator.MaxDatacenterId || datacenterId < 0)
                {
                    throw new ArgumentException($"datacenter Id can't be greater than {idGenerator.MaxDatacenterId} or less than 0");
                }

                idGenerator.DatacenterId = datacenterId;
            }

            return idGenerator;
        }

        public long WorkerId { get; internal set; }
        public long DatacenterId { get; internal set; }
        public long Sequence { get; internal set; }

        public virtual long Create()
        {
            lock (_lock)
            {
                var timestamp = TimeGen();

                if (timestamp < _lastTimestamp)
                {
                    // 如果启用此选项, 发生时间回退时使用上一个时间戳
                    if (!Options.UsePreviousInTimeRollback)
                    {
                        throw new Exception(
                            $"InvalidSystemClock: Clock moved backwards, Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
                    }
                    timestamp = _lastTimestamp;
                }

                if (_lastTimestamp == timestamp)
                {
                    Sequence = (Sequence + 1) & SequenceMask;
                    if (Sequence == 0L)
                    {
                        timestamp = TilNextMillis(_lastTimestamp);
                    }
                }
                else
                {
                    Sequence = 0;
                }

                _lastTimestamp = timestamp;
                var id = ((timestamp - Twepoch) << TimestampLeftShift) |
                         (DatacenterId << DatacenterIdShift) |
                         (WorkerId << WorkerIdShift) | 
                         Sequence;

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
