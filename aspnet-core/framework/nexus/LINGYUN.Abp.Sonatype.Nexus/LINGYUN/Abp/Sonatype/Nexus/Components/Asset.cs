namespace LINGYUN.Abp.Sonatype.Nexus.Components;
public class Asset
{
    public string FileName { get; }
    public byte[] FileBytes { get; }
    public Asset(string fileName, byte[] fileBytes)
    {
        FileName = fileName;
        FileBytes = fileBytes;
    }
}
