using System.Threading.Tasks;

namespace LINGYUN.Abp.FileManagement
{
    public interface IFileValidater
    {
        Task ValidationAsync(UploadOssObjectInput input);
    }
}
