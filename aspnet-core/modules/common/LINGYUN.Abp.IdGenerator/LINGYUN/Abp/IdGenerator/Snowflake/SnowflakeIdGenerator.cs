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

        private SnowflakeIdGenerator(SnowflakeIdOptions options)
        {
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

            if (idGenerator.WorkerId == 0 || (int)Math.Log10(options.WorkerId) + 1 > idGenerator.MaxWorkerId)
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

            if (idGenerator.DatacenterId == 0 || (int)Math.Log10(options.DatacenterId) + 1 > idGenerator.MaxDatacenterId)
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

                // TODO: 时间回退解决方案, 保存一个时间节点, 当服务器时间发生改变, 从保存的节点开始递增
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
