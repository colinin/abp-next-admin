using System.IO;
using System.Threading.Tasks;

namespace LINGYUN.Platform.Versions
{
    public interface IVersionFileManager
    {
        Task<string> AppendFileAsync(string version, string fileName, string fileVersion, byte[] data);
        Task<Stream> GetFileAsync(string version, string fileName, string fileVersion);
    }
}
