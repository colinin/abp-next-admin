using System.Text;

namespace System.IO;

internal static class FileSystemExtensions
{
    public static string MD5(this FileStream stream)
    {
        if (stream.CanSeek)
        {
            stream.Seek(0, SeekOrigin.Begin);
        }
        using var md5 = Security.Cryptography.MD5.Create();
        var retVal = md5.ComputeHash(stream);
        var sb = new StringBuilder();
        for (var i = 0; i < retVal.Length; i++)
        {
            sb.Append(retVal[i].ToString("x2"));
        }
        stream.Seek(0, SeekOrigin.Begin);
        return sb.ToString();
    }
}
