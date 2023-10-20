namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Browsers;

public static class CoreUIBrowse
{
    public static CoreUIRequest<CoreUIBrowseReadComponent> Read(
       string repository,
       string node = "/")
    {
        var readComponent = new CoreUIBrowseReadComponent(repository, node);

        return new CoreUIRequest<CoreUIBrowseReadComponent>(
            "coreui_Browse",
            "read",
            readComponent);
    }
}
