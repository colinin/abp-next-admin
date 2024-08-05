using System.Collections.Generic;

namespace LINGYUN.Abp.OssManagement;

public class OssObjectComparer : IComparer<OssObject>
{
    public virtual int Compare(OssObject x, OssObject y)
    {
        if (x.IsFolder && y.IsFolder)
        {
            return x.Name.CompareTo(y.Name);
        }

        if (x.IsFolder && !y.IsFolder)
        {
            return -1;
        }

        if (!x.IsFolder && y.IsFolder)
        {
            return 1;
        }

        return x.Name.CompareTo(y.Name);
    }
}
