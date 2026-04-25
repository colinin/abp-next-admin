using System.ComponentModel.DataAnnotations;

namespace LINGYUN.Abp.BlobManagement.Dtos;

public abstract class BlobFileChunkCreateBaseDto : BlobFileCreateBaseDto
{
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
}
