using Shouldly;
using System.Net;
using Volo.Abp;
using Xunit;

namespace LINGYUN.Abp.BackgroundTasks.Activities;
public class JobExceptionTypeFinder_Tests : AbpBackgroundTasksActivitiesTestBase
{
    private readonly IJobExceptionTypeFinder _jobExceptionTypeFinder;
    private readonly IJobActionDefinitionManager _jobActionDefinitionManager;
    public JobExceptionTypeFinder_Tests()
    {
        _jobExceptionTypeFinder = GetRequiredService<IJobExceptionTypeFinder>();
        _jobActionDefinitionManager = GetRequiredService<IJobActionDefinitionManager>();
    }

    [Fact]
    public virtual void Get_Exception_Type_Should_Be_Network()
    {
        var evntData = new JobEventData(
            "test",
            typeof(JobExceptionTypeFinder_Tests),
            "test",
            "test",
            null,
            new TestApplicationException((int)HttpStatusCode.RequestTimeout));
        var eventContext = new JobEventContext(ServiceProvider, evntData);
        var exceptionType = _jobExceptionTypeFinder.GetExceptionType(eventContext, evntData.Exception);
        exceptionType.ShouldBe(JobExceptionType.Network);
    }

    [Fact]
    public virtual void Job_Error_Should_Be_Triggerd()
    {
        _jobActionDefinitionManager
            .Get("test_app")
            .GetExceptioinType().Value
            .HasFlag(JobExceptionType.Network).ShouldBeFalse();

        _jobActionDefinitionManager
            .Get("test_net")
            .GetExceptioinType().Value
            .HasFlag(JobExceptionType.Network).ShouldBeTrue();

        _jobActionDefinitionManager
            .Get("test_all")
            .GetExceptioinType().Value
            .HasFlag(JobExceptionType.Network | JobExceptionType.Application).ShouldBeTrue();
    }

    [Fact]
    public virtual void Job_Error_Mapping_Should_Be_Triggerd()
    {
        var evntData = new JobEventData(
           "test_other",
           typeof(JobExceptionTypeFinder_Tests),
           "test",
           "test",
           null,
           new BusinessException("TEST:001"));
        var eventContext = new JobEventContext(ServiceProvider, evntData);
        var exceptionType = _jobExceptionTypeFinder.GetExceptionType(eventContext, evntData.Exception);
        exceptionType.ShouldBe(JobExceptionType.Business);
    }
}
