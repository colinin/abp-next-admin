using System;
using System.ComponentModel.DataAnnotations;

namespace PackageName.CompanyName.ProjectName.Users.Dtos
{
    [Serializable]
    public class CreateUpdateUserDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required(ErrorMessage = "用户名称不能为空")]
        [StringLength(50, ErrorMessage = "用户名称长度不能超过50个字符")]
        public string NickName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度必须在6-20个字符之间")]
        public string Password { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactInfo { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}