using System;

namespace LINGYUN.Abp.Location.Baidu.Model;

public class IpPoint
{
    public string X { get; set; } = default!;
    public string Y { get; set; } = default!;

    public Point ToPoint()
    {
        if (!X.IsNullOrWhiteSpace() && 
            !Y.IsNullOrWhiteSpace())
        {
            if (float.TryParse(X, out float x) &&
                float.TryParse(Y, out float y))
            {
                return new Point
                {
                    X = x,
                    Y = y
                };
            }
        }

        return new Point();
    }
}
