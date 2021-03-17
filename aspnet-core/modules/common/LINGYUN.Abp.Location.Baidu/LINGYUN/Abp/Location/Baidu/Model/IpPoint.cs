using System;

namespace LINGYUN.Abp.Location.Baidu.Model
{
    public class IpPoint
    {
        public string X { get; set; }
        public string Y { get; set; }

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
}
