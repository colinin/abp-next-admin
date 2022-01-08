using System;
using Volo.Abp.Domain.Entities;

namespace LINGYUN.Abp.TaskManagement;

public class BackgroundJobLog : Entity<long>
{
    public virtual Guid? JobId { get; set; }
    public virtual string JobName { get; protected set; }
    public virtual string JobGroup { get; protected set; }
    public virtual string JobType { get; protected set; }
    public virtual string Message { get; protected set; }
    public virtual DateTime RunTime { get; protected set; }
    public virtual string Exception { get; protected set; }
    protected BackgroundJobLog() { }
    public BackgroundJobLog(string type, string group, string name)
    {
        JobType = type;
        JobGroup = group;
        JobName = name;
        RunTime = DateTime.Now;
    }

    public BackgroundJobLog SetMessage(string message, Exception ex)
    {
        Message = message.Length > BackgroundJobLogConsts.MaxMessageLength 
            ? message.Substring(0, BackgroundJobLogConsts.MaxMessageLength - 1) 
            : message;

        if (ex != null)
        {
            var errMsg = ex.ToString();
            Exception = errMsg.Length > BackgroundJobLogConsts.MaxExceptionLength
                ? errMsg.Substring(0, BackgroundJobLogConsts.MaxExceptionLength - 1)
                : errMsg;
        }
        return this;
    }
}
