namespace System;

internal static class StringExtensions
{
    public static string ReplaceTypeSimple(this string typeSimple)
    {
        typeSimple = typeSimple
            .Replace("?", "")
            .Replace("<System.String>", "<string>")
            .Replace("<System.Guid>", "<string>")
            .Replace("<System.Int32>", "<number>")
            .Replace("<System.Int64>", "<number>")
            .Replace("IRemoteStreamContent", "Blob")
            .Replace("{string:string}", "Dictionary<string, string>")
            .Replace("{number:string}", "Dictionary<number, string>")
            .Replace("{string:number}", "Dictionary<string, number>")
            .Replace("{string:object}", "Dictionary<string, any>");

        if (typeSimple.StartsWith("[") && typeSimple.EndsWith("]"))
        {
            typeSimple = typeSimple.ReplaceFirst("[", "").RemovePostFix("]", "");
            typeSimple = typeSimple.Replace(typeSimple, $"{typeSimple}[]");
        }

        return typeSimple;
    }

    public static string MiddleString(this string sourse, string startstr, string endstr)
    {
        var result = string.Empty;
        int startindex, endindex;
        startindex = sourse.IndexOf(startstr);
        if (startindex == -1)
        {
            return result;
        }
        var tmpstr = sourse.Substring(startindex + startstr.Length - 1);
        endindex = tmpstr.IndexOf(endstr);
        if (endindex == -1)
        {
            return result;
        }
        result = tmpstr.Remove(endindex + 1);

        return result;
    }
}
