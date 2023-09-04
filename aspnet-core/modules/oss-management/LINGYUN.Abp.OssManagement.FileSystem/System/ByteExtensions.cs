using System;
using System.Collections.Generic;
using System.Linq;

namespace System;
public static class ByteExtensions
{
    private readonly static string[] ImageTypes = new string[]
    {
        "6677",// bmp
        "7173",// gif
        "13780",// png
        "255216"// jpg
    };

    public static bool IsImage(this byte[] fileBytes)
    {
        if (fileBytes.IsNullOrEmpty())
        {
            return false;
        }

        string fileclass = "";
        for (int i = 0; i < 2; i++)
        {
            fileclass += fileBytes[i].ToString();
        }

        return ImageTypes.Any(type => type.Equals(fileclass));
    }
}
