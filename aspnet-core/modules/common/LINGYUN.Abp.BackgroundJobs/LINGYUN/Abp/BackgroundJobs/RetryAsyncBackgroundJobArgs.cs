namespace LINGYUN.Abp.BackgroundJobs
{
    public class RetryAsyncBackgroundJobArgs<TArgs>
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int RetryCount { get; set; } = 0;
        /// <summary>
        /// 重试间隔(毫秒)
        /// 默认 300000ms = 5min
        /// </summary>
        public double RetryIntervalMillisecond { get; set; } = 300000d;
        /// <summary>
        /// 最大重试次数
        /// 默认 20
        /// </summary>
        public int MaxRetryCount { get; set; } = 20;
        /// <summary>
        /// 作业参数
        /// </summary>
        public TArgs JobArgs { get; set; }

        public RetryAsyncBackgroundJobArgs()
        {

        }

        public RetryAsyncBackgroundJobArgs(TArgs jobArgs)
        {
            JobArgs = jobArgs;
        }

        public RetryAsyncBackgroundJobArgs(TArgs jobArgs, int retryCount = 0, double interval = 300000d, int maxRetryCount = 20)
        {
            JobArgs = jobArgs;

            RetryCount = retryCount;
            RetryIntervalMillisecond = interval;
            MaxRetryCount = maxRetryCount;
        }
    }
}
