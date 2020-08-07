using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.FileManagement
{
    public interface IFileSystemAppService : IApplicationService
    {
        Task<FileSystemDto> GetAsync(FileSystemGetDto input);

        Task<PagedResultDto<FileSystemDto>> GetListAsync(GetFileSystemListDto input);

        Task CreateFolderAsync(FolderCreateDto input);

        Task<FileSystemDto> UpdateAsync([Required, StringLength(255)] string name, FileSystemUpdateDto input);

        Task DeleteFolderAsync([Required, StringLength(255)] string path);

        Task MoveFolderAsync([Required, StringLength(255)] string path, FolderMoveDto input);

        Task CopyFolderAsync([Required, StringLength(255)] string path, FolderCopyDto input);

        Task CreateFileAsync(FileCreateDto input);

        Task DeleteFileAsync(FileDeleteDto input);

        Task MoveFileAsync(FileCopyOrMoveDto input);

        Task CopyFileAsync(FileCopyOrMoveDto input);

        Task<Stream> DownloadFileAsync(FileSystemGetDto input);
    }
}
