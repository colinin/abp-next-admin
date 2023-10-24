namespace LINGYUN.Abp.Sonatype.Nexus;
public class AbpSonatypeNexusOptions
{
    public string BaseUrl { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public AbpSonatypeNexusOptions()
    {
        BaseUrl = "http://127.0.0.1:8081";
        UserName = "sonatype";
        Password = "sonatype";
    }
}
