using System;
using Volo.Abp;

namespace LINGYUN.Abp.AIManagement.Messages;
public class UserTextMessageRecord : UserMessageRecord
{
    public string Content { get; private set; }

    public UserTextMessageRecord()
    {
    }

    public UserTextMessageRecord(
        Guid id,
        string workspace,
        string content,
        Guid? tenantId = null) : base(id, workspace, tenantId)
    {
        WithContent(content);
    }

    public virtual UserTextMessageRecord WithContent(string content)
    {
        Content = Check.NotNullOrWhiteSpace(content, nameof(content), UserTextMessageRecordConsts.MaxContentLength);
        return this;
    }
}
