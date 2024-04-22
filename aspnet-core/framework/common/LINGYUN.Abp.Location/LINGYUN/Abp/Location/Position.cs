namespace LINGYUN.Abp.Location
{
    /// <summary>
    /// 位置量
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 左上纬度
        /// </summary>
        public double LeftTopLatitude { get; }
        /// <summary>
        /// 左上经度
        /// </summary>
        public double LeftTopLongitude { get; }
        /// <summary>
        /// 左下纬度
        /// </summary>
        public double LeftBottomLatitude { get; }
        /// <summary>
        /// 左下经度
        /// </summary>
        public double LeftBottomLongitude { get; }

        /// <summary>
        /// 右上纬度
        /// </summary>
        public double RightTopLatitude { get; }
        /// <summary>
        /// 右上经度
        /// </summary>
        public double RightTopLongitude { get; }
        /// <summary>
        /// 右下纬度
        /// </summary>
        public double RightBottomLatitude { get; }
        /// <summary>
        /// 右下经度
        /// </summary>
        public double RightBottomLongitude { get; }

        internal Position(double leftTopLat, double leftBottomLat, double leftTopLng, double leftBottomLng,
            double rightTopLat, double rightBottomLat, double rightTopLng, double rightBottomLng)
        {
            LeftTopLatitude = leftTopLat;
            LeftBottomLatitude = leftBottomLat;
            LeftTopLongitude = leftTopLng;
            LeftBottomLongitude = leftBottomLng;
            RightTopLatitude = rightTopLat;
            RightTopLongitude = rightTopLng;
            RightBottomLatitude = rightBottomLat;
            RightBottomLongitude = rightBottomLng;
        }
    }
}
