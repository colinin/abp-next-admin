using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Platform.Versions
{
    public interface IVersionFileManager
    {
        Task<string> SaveFileAsync(string version, string filePath, string fileName, string fileVersion, byte[] data);
        Task<Stream> DownloadFileAsync(PlatformType platformType, string version, string filePath, string fileName, string fileVersion);
    }
}
