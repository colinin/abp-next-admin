namespace LINGYUN.Abp.IdGenerator.Snowflake;

public class SnowflakeIdOptions
{
    /// <summary>
    /// 机器Id
    /// </summary>
    public int WorkerId { get; set; }
    /// <summary>
    /// 机器Id长度
    /// </summary>
    public int WorkerIdBits { get; set; }
    /// <summary>
    /// 数据中心Id
    /// </summary>
    public int DatacenterId { get; set; }
    /// <summary>
    /// 数据中心Id长度
    /// </summary>
    public int DatacenterIdBits { get; set; }
    /// <summary>
    /// 12bit 的序号
    /// </summary>
    public long Sequence { get; set; }

    public int SequenceBits { get; set; }

    public SnowflakeIdOptions()
    {
        WorkerIdBits = 5;
        DatacenterIdBits = 5;
        Sequence = 0L;
        SequenceBits = 12;
    }
}
