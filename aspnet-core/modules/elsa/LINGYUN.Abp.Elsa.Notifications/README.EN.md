# LINGYUN.Abp.Elsa.Notifications

Workflow notification integration. When a workflow is triggered, it publishes corresponding event notifications.

## Available States

* Faulted: Workflow execution encountered an error
* Cancelled: Workflow was cancelled
* Completed: Workflow execution completed
* Suspended: Workflow is suspended

## Configuration and Usage

```csharp
[DependsOn(
    typeof(AbpElsaNotificationsModule)
    )]
public class YouProjectModule : AbpModule
{
}
```

```csharp
// Define notifications
public class DemoNotificationDefinitionProvider : NotificationDefinitionProvider
{
    public override void Define(INotificationDefinitionContext context)
    {
        var demoGroup = context.AddGroup("Group");

        // Due to the diversity of notifications, template messages are used to transmit data
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
// Define workflow
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
                // Can pass custom parameters, which will be written as transient variables into the published notification data
                context.WithNotificationData("demo", "demo");
                // Custom tenant ID for publishing notifications
                context.WithNotificationTenantId(Guid.NewGuid());
            })
            .WriteLine("Start a workflow.")
            .WriteLine("Workflow finished.");
    }
}
```

[中文文档](./README.md)
