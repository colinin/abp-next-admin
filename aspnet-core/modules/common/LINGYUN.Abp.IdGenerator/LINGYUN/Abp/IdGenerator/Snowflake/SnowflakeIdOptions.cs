namespace LINGYUN.Abp.IdGenerator.Snowflake;

public class SnowflakeIdOptions
{
    /// <summary>
    /// 机器Id长度
    /// </summary>
    public int WorkerIdBits { get; set; }
    /// <summary>
    /// 机房Id长度
    /// </summary>
    public int DatacenterIdBits { get; set; }
    /// <summary>
    /// 12bit 的序号
    /// </summary>
    public long Sequence { get; set; }
    /// <summary>
    /// 每秒生成Id的数量
    /// </summary>
    public int SequenceBits { get; set; }

    public SnowflakeIdOptions()
    {
        WorkerIdBits = 5;
        DatacenterIdBits = 5;
        Sequence = 0L;
        SequenceBits = 12;
    }
}
