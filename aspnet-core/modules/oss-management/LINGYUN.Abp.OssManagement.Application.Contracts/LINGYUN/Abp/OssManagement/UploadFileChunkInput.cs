using System.ComponentModel.DataAnnotations;
using Volo.Abp.Auditing;
using Volo.Abp.Content;
using Volo.Abp.Validation;

namespace LINGYUN.Abp.OssManagement
{
    public class UploadFileChunkInput : UploadFile
    {
        public string Bucket { get; set; }
        public string Path { get; set; }

        #region 配合Uplaoder 分块传输
        /// <summary>
        /// 常规块大小
        /// </summary>
        [Required]
        public long ChunkSize { get; set; }
        /// <summary>
        /// 当前块大小
        /// </summary>
        [Required]
        public long CurrentChunkSize { get; set; }
        /// <summary>
        /// 当前上传中块的索引
        /// </summary>
        [Required]
        public int ChunkNumber { get; set; }
        /// <summary>
        /// 块总数
        /// </summary>
        [Required]
        public int TotalChunks { get; set; }

        #endregion

        [DisableAuditing]
        [DisableValidation]
        public IRemoteStreamContent File { get; set; }
    }
}
