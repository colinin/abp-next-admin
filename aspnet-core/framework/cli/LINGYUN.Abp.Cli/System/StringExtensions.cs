namespace System;

internal static class StringExtensions
{
    public static string ReplaceTypeSimple(this string typeSimple)
    {
        typeSimple = typeSimple
            .Replace("<System.String>", "<string>")
            .Replace("<System.Guid>", "<string>")
            .Replace("<System.Int32>", "<number>")
            .Replace("<System.Int64>", "<number>")
            .Replace("<System.DateTime>", "<Date>")
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

    public static string ReplaceFlutterType(this string type)
    {
        type = type
            .Replace("{System.String:System.String}", "Map<String, String>")
            .Replace("{System.Int32:System.String}", "Map<num, String>")
            .Replace("{System.Int64:System.String}", "Map<num, String>")
            .Replace("{System.String:System.Int32}", "Map<String, num>")
            .Replace("{System.String:System.Int64}", "Map<String, num>")
            .Replace("{System.String:System.Object}", "Map<String, dynamic>")
            .Replace("System.String", "String")
            .Replace("System.Guid", "String")
            .Replace("System.Int32", "num")
            .Replace("System.Int64", "num")
            .Replace("System.DateTime", "DateTime")
            .Replace("System.Boolean", "bool")
            .Replace("System.Object", "dynamic")
            .Replace("IRemoteStreamContent", "Blob");

        if (type.StartsWith("[") && type.EndsWith("]"))
        {
            if (type.LastIndexOf('.') >= 0)
            {
                type = type[(type.LastIndexOf('.') + 1)..];
            }

            type = type.RemovePreFix("[", "<").RemovePostFix("]", ">");

            type = "List<" + type + ">";
        }
        else
        {
            if (type.LastIndexOf('.') >= 0)
            {
                type = type[(type.LastIndexOf('.') + 1)..];
            }
        }

        return type;
    }

    public static string ReplaceFlutterTypeSimple(this string typeSimple)
    {
        typeSimple = typeSimple
            .Replace("?", "")
            .Replace("<System.String>", "<String>")
            .Replace("<System.Guid>", "<String>")
            .Replace("<System.Int32>", "<num>")
            .Replace("<System.Int64>", "<num>")
            .Replace("<System.DateTime>", "<DateTime>")
            .Replace("IRemoteStreamContent", "Blob")
            .Replace("{string:string}", "Map<String, String>")
            .Replace("{number:string}", "Map<int, String>")
            .Replace("{string:number}", "Map<String, num>")
            .Replace("{string:object}", "Map<String, dynamic>");

        if (typeSimple.StartsWith("[") && typeSimple.EndsWith("]"))
        {
            typeSimple = typeSimple.RemovePreFix("[", "<").RemovePostFix("]", ">");
            typeSimple = "List<" + typeSimple + ">";
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
