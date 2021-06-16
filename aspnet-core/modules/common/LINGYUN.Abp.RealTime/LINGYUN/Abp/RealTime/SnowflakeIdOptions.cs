namespace LINGYUN.Abp.RealTime
{
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
        /// 每秒生成Id的数量
        /// </summary>
        public int SequenceBits { get; set; }
        public SnowflakeIdOptions()
        {
            WorkerIdBits = 5;
            DatacenterIdBits = 5;
            SequenceBits = 12;
        }
    }
}
