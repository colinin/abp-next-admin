using System;

namespace LINGYUN.Abp.Location
{
    public class Location
    {
        /// <summary>
        /// 地球半径（米）
        /// </summary>
        public const double EARTH_RADIUS = 6378137;
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 计算两个位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="location">参与计算的位置信息</param>
        /// <returns>返回两个位置的距离</returns>
        public double CalcDistance(Location location)
        {
            return CalcDistance(this, location);
        }
        /// <summary>
        /// 计算两个位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="location1">参与计算的位置信息</param>
        /// <param name="location2">参与计算的位置信息</param>
        /// <returns>返回两个位置的距离</returns>
        public static double CalcDistance(Location location1, Location location2)
        {
            double radLat1 = Rad(location1.Latitude);
            double radLng1 = Rad(location1.Longitude);
            double radLat2 = Rad(location2.Latitude);
            double radLng2 = Rad(location2.Longitude);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            result *= EARTH_RADIUS;
            return result;
        }
        /// <summary>
        /// 计算位置的偏移距离
        /// </summary>
        /// <param name="location">参与计算的位置</param>
        /// <param name="distance">位置偏移量,单位 米</param>
        /// <returns></returns>
        public static Position CalcOffsetDistance(Location location, double distance)
        {
            double dlng = 2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(Rad(location.Latitude)));
            dlng = Deg(dlng);
            double dlat = distance / EARTH_RADIUS;
            dlat = Deg(dlat);
            double leftTopLat = location.Latitude + dlat;
            double leftTopLng = location.Longitude - dlng;

            double leftBottomLat = location.Latitude - dlat;
            double leftBottomLng = location.Longitude - dlng;

            double rightTopLat = location.Latitude + dlat;
            double rightTopLng = location.Longitude + dlng;

            double rightBottomLat = location.Latitude - dlat;
            double rightBottomLng = location.Longitude + dlng;

            return new Position(leftTopLat, leftBottomLat, leftTopLng, leftBottomLng,
                rightTopLat, rightBottomLat, rightTopLng, rightBottomLng);
        }

        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }

        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Deg(double d)
        {
            return d * (180 / Math.PI);
        }
    }
}
