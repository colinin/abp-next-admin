# LINGYUN.Abp.Elsa.Notifications

工作流通知集成, 当工作流触发后, 发布相应事件通知    

## 可用状态

* Faulted:      工作流执行出现错误  
* Cancelled:    工作流取消  
* Completed:    工作流执行完毕  
* Suspended:    工作流暂停  


## 配置使用

```csharp

    [DependsOn(
        typeof(AbpElsaNotificationsModule)
        )]
    public class YouProjectModule : AbpModule
    {
    }
```

```csharp
    // 定义通知
    public class DemoNotificationDefinitionProvider : NotificationDefinitionProvider
    {
        public override void Define(INotificationDefinitionContext context)
        {
            var demoGroup = context.AddGroup("Group");

            // 由于通知的多样性, 需要使用模板消息来传递数据
            demoGroup.AddNotification("Faulted")
                .WithTemplate(template => { });
            demoGroup.AddNotification("Cancelled")
                .WithTemplate(template => { });
            demoGroup.AddNotification("Suspended")
                .WithTemplate(template => { });
            demoGroup.AddNotification("Completed")
                .WithTemplate(template => { });
        }
    }
```

```csharp

    // 定义工作流
    public class DemoWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .WithFaultedNotification("Faulted")
                .WithCancelledNotification("Cancelled")
                .WithSuspendedNotification("Suspended")
                .WithCompletedNotification("Completed")
                .SetVariable("demo", context =>
                {
                    // 可传递自定义参数, 将作为瞬时变量写入到发布的通知数据中
                    context.WithNotificationData("demo", "demo");
                })
                .WriteLine("Start a workflow.")
                .WriteLine("Workflow finished.");
        }
    }
```