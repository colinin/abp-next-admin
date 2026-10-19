using System;
using System.IO;
using Volo.Abp;

namespace LINGYUN.Abp.BlobManagement;

/// <summary>
/// BlobManagement 名称校验
/// </summary>
public static class BlobManagementNameValidator
{
    /// <summary>
    /// 不允许出现的字符(各平台非法字符的并集, 含路径分隔符)
    /// </summary>
    public static readonly char[] InvalidNameCharacters =
    [
        '\0', '/', '\\', ':', '*', '?', '"', '<', '>', '|',
    ];

    /// <summary>
    /// 校验单一名称(容器名、文件/目录名), 不合法时抛出异常
    /// </summary>
    /// <remarks>
    /// 不允许路径分隔符, 否则会产生容器别名或把层级写进名字;
    /// 依次检查: 空值 → 穿越段(./..) → 非法字符 → 结尾点/空格 → 保留设备名。
    /// </remarks>
    public static void CheckName(string? name, string errorCode)
    {
        if (!IsValidSegment(name))
        {
            throw CreateInvalidNameException(name, errorCode);
        }
    }

    /// <summary>
    /// 校验名称路径(如 <see cref="Blob.FullName"/>), 不合法时抛出异常
    /// </summary>
    /// <remarks>
    /// 允许 '/' 与 '\' 作为分隔符, 但逐段按单一名称规则校验;
    /// 先去掉前导 '/' (模块约定全名以 '/' 开头), 再拒绝绝对路径与 UNC。
    /// </remarks>
    public static void CheckPath(string? path, string errorCode)
    {
        if (!IsValidPath(path))
        {
            throw CreateInvalidNameException(path, errorCode);
        }
    }

    /// <summary>
    /// 是否为合法路径(逐段校验, 允许分隔符)
    /// </summary>
    public static bool IsValidPath(string? path)
    {
        if (path.IsNullOrWhiteSpace())
        {
            return false;
        }

        var name = path.RemovePreFix("/");

        if (Path.IsPathRooted(name) ||
            name.StartsWith(@"\\", StringComparison.Ordinal))
        {
            return false;
        }

        var segments = name.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            return false;
        }

        foreach (var segment in segments)
        {
            if (!IsValidSegment(segment))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 是否为合法单一名称(不含分隔符)
    /// </summary>
    public static bool IsValidName(string? name)
    {
        return IsValidSegment(name);
    }

    /// <summary>
    /// 是否为合法路径段
    /// </summary>
    public static bool IsValidSegment(string? segment)
    {
        if (segment.IsNullOrWhiteSpace())
        {
            return false;
        }
        if (segment == "." || segment == "..")
        {
            return false;
        }
        if (segment.IndexOfAny(InvalidNameCharacters) >= 0)
        {
            return false;
        }
        if (segment.EndsWith(".") || segment.EndsWith(" "))
        {
            return false;
        }

        return !IsReservedDeviceName(segment);
    }

    /// <summary>
    /// 是否为 Windows 保留设备名(CON/PRN/AUX/NUL/COM1-9/LPT1-9)
    /// </summary>
    public static bool IsReservedDeviceName(string name)
    {
        if (name.IsNullOrWhiteSpace())
        {
            return false;
        }

        var extensionIndex = name.IndexOf('.');
        if (extensionIndex > 0)
        {
            name = name.Substring(0, extensionIndex);
        }

        if (name.Equals("CON", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("PRN", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("AUX", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("NUL", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (name.Length == 4)
        {
            var prefix = name.Substring(0, 3);
            var suffix = name[3];

            return suffix >= '1' && suffix <= '9' &&
                   (prefix.Equals("COM", StringComparison.OrdinalIgnoreCase) ||
                    prefix.Equals("LPT", StringComparison.OrdinalIgnoreCase));
        }

        return false;
    }

    /// <summary>
    /// 创建名称非法异常
    /// </summary>
    public static BusinessException CreateInvalidNameException(string? name, string errorCode)
    {
        return new BusinessException(errorCode, $"Not allowed name: {name}!")
            .WithData("Name", name ?? string.Empty);
    }
}
