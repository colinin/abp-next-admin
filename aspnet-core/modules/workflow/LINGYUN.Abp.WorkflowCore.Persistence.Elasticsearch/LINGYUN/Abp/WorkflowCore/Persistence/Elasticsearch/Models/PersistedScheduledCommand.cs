using System;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Persistence.Elasticsearch.Models
{
    public class PersistedScheduledCommand
    {
        public Guid Id { get; set; }
        public string CommandName { get; set; }
        public string Data { get; set; }
        public long ExecuteTime { get; set; }

        public PersistedScheduledCommand() { }

        public PersistedScheduledCommand(Guid id, ScheduledCommand command)
        {
            Id = id;
            CommandName = command.CommandName;
            Data = command.Data;
            ExecuteTime = command.ExecuteTime;
        }
        public ScheduledCommand ToScheduledCommand()
        {
            return new ScheduledCommand
            {
                CommandName = CommandName,
                Data = Data,
                ExecuteTime = ExecuteTime
            };
        }
    }
}
