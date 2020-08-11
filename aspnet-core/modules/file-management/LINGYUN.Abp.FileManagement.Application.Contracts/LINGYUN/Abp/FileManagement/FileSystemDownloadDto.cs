using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LINGYUN.Abp.FileManagement
{
    public class FileSystemDownloadDto : FileSystemGetDto
    {
        /// <summary>
        /// 当前字节数
        /// </summary>
        public int CurrentByte { get; set; }
    }
}
