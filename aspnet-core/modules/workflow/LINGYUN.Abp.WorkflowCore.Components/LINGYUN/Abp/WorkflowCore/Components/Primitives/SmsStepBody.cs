using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Sms;
using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace LINGYUN.Abp.WorkflowCore.Components.Primitives
{
    public class SmsStepBody : StepBodyAsyncBase
    {
        public ILogger<SmsStepBody> Logger { protected get; set; }


        private readonly ISmsSender _smsSender;

        public SmsStepBody(ISmsSender smsSender)
        {
            _smsSender = smsSender;

            Logger = NullLogger<SmsStepBody>.Instance;
        }

        [NotNull]
        public string Message { get; set; }

        [NotNull]
        public string PhoneNumber { get; set; }

        [CanBeNull]
        public string Template { get; set; }

        public override async Task<ExecutionResult> RunAsync(IStepExecutionContext context)
        {
            Logger.LogInformation("Working on sending sms message step.");

            var smsMessage = new SmsMessage(PhoneNumber, Message);
            if (!Template.IsNullOrWhiteSpace())
            {
                smsMessage.Properties.Add("TemplateCode", Template);
            }
            await _smsSender.SendAsync(smsMessage);

            Logger.LogInformation("Sms message sent, forward to next step.");

            return ExecutionResult.Next();
        }
    }
}
