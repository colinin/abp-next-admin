namespace LINGYUN.Abp.LocalizationManagement;

public abstract class DynamicLocalizationInitializerEto
{
    public string[] Keys { get; set; }
    protected DynamicLocalizationInitializerEto()
    {

    }

    protected DynamicLocalizationInitializerEto(string[] keys)
    {
        Keys = keys;
    }
}

public class DynamicLanguageInitializerEto : DynamicLocalizationInitializerEto
{
    public DynamicLanguageInitializerEto()
    {

    }

    public DynamicLanguageInitializerEto(string[] keys) : base(keys)
    {
    }
}


public class DynamicResourceInitializerEto : DynamicLocalizationInitializerEto
{
    public DynamicResourceInitializerEto()
    {

    }

    public DynamicResourceInitializerEto(string[] keys) : base(keys)
    {
    }
}


public class DynamicTextInitializerEto : DynamicLocalizationInitializerEto
{
    public DynamicTextInitializerEto()
    {

    }

    public DynamicTextInitializerEto(string[] keys) : base(keys)
    {
    }
}