using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.FileManagement
{
    public class UploadOssObjectInput 
    {
        public string Bucket { get; set; }
        public string Path { get; set; }

        #region 配合Uplaoder 分块传输
        /// <summary>
        /// 文件名
        /// </summary>
        [Required]
        public string FileName { get; set; }
        /// <summary>
        /// 常规块大小
        /// </summary>
        [Required]
        public int ChunkSize { get; set; }
        /// <summary>
        /// 当前块大小
        /// </summary>
        [Required]
        public int CurrentChunkSize { get; set; }
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
        /// <summary>
        /// 总文件大小
        /// </summary>
        [Required]
        public int TotalSize { get; set; }

        #endregion

        public IFormFile File { get; set; }
    }
}
