namespace LINGYUN.Abp.Sonatype.Nexus.Services.CoreUI.Assets;

public static class CoreUIAsset
{
    public static CoreUIRequest<CoreUIAssetRead> Read(
        string assetId,
        string repository)
    {
        var asset = new CoreUIAssetRead(assetId, repository);

        return new CoreUIRequest<CoreUIAssetRead>(
            "coreui_Component",
            "readAsset",
            asset);
    }
}
