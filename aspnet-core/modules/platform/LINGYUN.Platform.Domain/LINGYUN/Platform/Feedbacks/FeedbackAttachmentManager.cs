using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Services;
using Volo.Abp.IO;

namespace LINGYUN.Platform.Feedbacks;
public class FeedbackAttachmentManager : DomainService
{
    private const string DefaultSaveToApplication = "abp-application";
    private const string DefaultSaveToTempPath = "feedback-upload-tmp";

    private readonly IBlobContainer<FeedbackContainer> _blobContainer;
    private readonly IApplicationInfoAccessor _applicationInfoAccessor;

    public FeedbackAttachmentManager(
        IBlobContainer<FeedbackContainer> blobContainer,
        IApplicationInfoAccessor applicationInfoAccessor)
    {
        _blobContainer = blobContainer;
        _applicationInfoAccessor = applicationInfoAccessor;
    }

    /// <summary>
    /// 将文件流写入临时存储
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <returns>返回临时存储的文件标识</returns>
    public async Task<FeedbackAttachmentTempFile> SaveToTempAsync(Stream stream)
    {
        var blobSize = stream.Length;
        var blobId = GuidGenerator.Create().ToString("N");
        var timeStampPath = Clock.Now.ToString("yyyy-MM-dd");

        var tempFilePath = Path.Combine(
            Path.GetTempPath(),
            _applicationInfoAccessor.ApplicationName ?? DefaultSaveToApplication,
            DefaultSaveToTempPath,
            timeStampPath);
        var tempSavedFile = Path.Combine(tempFilePath, $"{blobId}.png");

        DirectoryHelper.CreateIfNotExists(tempFilePath);

        using (var fs = new FileStream(tempSavedFile, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fs);
        }

        return new FeedbackAttachmentTempFile
        {
            Path = timeStampPath,
            Size = blobSize,
            Id = blobId,
        };
    }
    /// <summary>
    /// 从临时存储附件拷贝到blob存储
    /// </summary>
    /// <param name="feedback">用户反馈实体</param>
    /// <param name="file">附件临时存储文件</param>
    /// <returns>已保存的附件</returns>
    public async Task<FeedbackAttachmentFile> CopyFromTempAsync(Feedback feedback, string filePtah, string fileId)
    {
        var fileName = $"{filePtah}/{fileId}.png";
        var tempFilePath = Path.Combine(
            Path.GetTempPath(),
            _applicationInfoAccessor.ApplicationName ?? DefaultSaveToApplication,
            DefaultSaveToTempPath);
        var tempFromFile = Path.Combine(tempFilePath, fileName);
        var blobFile = $"{fileId}.png";
        var saveToBlobFile = $"{feedback.Id}/{blobFile}";

        if (!File.Exists(tempFromFile))
        {
            throw new BusinessException(PlatformErrorCodes.FeedackAttachmentLoseEffectiveness)
                .WithData("Name", fileId);
        }
        FeedbackAttachmentFile attachmentFile;

        using (var fs = new FileStream(tempFromFile, FileMode.Open, FileAccess.Read))
        {
            await _blobContainer.SaveAsync(saveToBlobFile, fs, true);

            attachmentFile = new FeedbackAttachmentFile(blobFile, fs.Length);
        }

        FileHelper.DeleteIfExists(tempFromFile);

        return attachmentFile;
    }
    /// <summary>
    /// 下载附件
    /// </summary>
    /// <param name="attachment">附件实体</param>
    /// <returns>附件文件流</returns>
    public async Task<Stream> DownloadAsync(FeedbackAttachment attachment)
    {
        var blobFile = $"{attachment.FeedbackId}/{attachment.Name}";

        return await _blobContainer.GetAsync(blobFile);
    }
    /// <summary>
    /// 删除附件
    /// </summary>
    /// <param name="attachment">附件实体</param>
    /// <returns>附件文件流</returns>
    public async Task DeleteAsync(FeedbackAttachment attachment)
    {
        var blobFile = $"{attachment.FeedbackId}/{attachment.Name}";

        await _blobContainer.DeleteAsync(blobFile);
    }
}
