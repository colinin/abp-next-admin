namespace LINGYUN.Abp.BackgroundTasks.Activities;
public static class JobActionDefinitionExceptionExtensions
{
    private const string ExceptioinTypeKey = "__Abp_BackgroundTasks_Exception_Type__";
    public static JobActionDefinition WithExceptioinType(this JobActionDefinition definition, JobExceptionType exceptionType)
    {
        return definition.WithProperty(ExceptioinTypeKey, exceptionType);
    }

    public static JobExceptionType? GetExceptioinType(this JobActionDefinition definition)
    {
        if (definition.Properties.TryGetValue(ExceptioinTypeKey, out var value) && value is JobExceptionType exceptionType)
        {
            return exceptionType;
        }
        return null;
    }
}
