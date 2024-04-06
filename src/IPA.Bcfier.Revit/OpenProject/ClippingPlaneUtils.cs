// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

using IPA.Bcfier.Models.Bcf;

namespace IPA.Bcfier.Revit.OpenProject
{
    public static class ClippingPlaneUtils
    {
        private static Vector3 XPositive() => new Vector3(1, 0, 0);

        private static Vector3 XNegative() => new Vector3(-1, 0, 0);

        private static Vector3 YPositive() => new Vector3(0, 1, 0);

        private static Vector3 YNegative() => new Vector3(0, -1, 0);

        private static Vector3 ZPositive() => new Vector3(0, 0, 1);

        private static Vector3 ZNegative() => new Vector3(0, 0, -1);

        /// <summary>
        /// Converts a bcf clipping plane into an axis aligned clipping bounding box. The clipping
        /// plane direction vector (plane normal) is compared to the coordinate axes. If the angle
        /// is below a given delta, the plane is converted to one side of an axis aligned clipping
        /// bounding box. The rest of the box's sides are set to max value.
        /// </summary>
        /// <param name="plane">The bcf clipping plane.</param>
        /// <param name="delta">
        /// The maximum angle the plane direction can differ from a XYZ coordinate axis.
        /// </param>
        /// <returns>The axis aligned bounding box.</returns>
        public static AxisAlignedBoundingBox ToAxisAlignedBoundingBox(this BcfViewpointClippingPlane plane, decimal delta = 0)
        {
            var infiniteMax = new Vector3(decimal.MaxValue, decimal.MaxValue, decimal.MaxValue);
            var infiniteMin = new Vector3(decimal.MinValue, decimal.MinValue, decimal.MinValue);

            var normal = new Vector3(
              (decimal)plane.Direction.X,
              (decimal)plane.Direction.Y,
              (decimal)plane.Direction.Z);

            var boundingBox = new AxisAlignedBoundingBox(infiniteMin, infiniteMax);

            if (normal.AngleBetween(ZPositive()) < delta)
            {
                var max = new Vector3(decimal.MaxValue, decimal.MaxValue, (decimal)plane.Location.Z);
                boundingBox = new AxisAlignedBoundingBox(infiniteMin, max);
            }
            else if (normal.AngleBetween(ZNegative()) < delta)
            {
                var min = new Vector3(decimal.MinValue, decimal.MinValue, (decimal)plane.Location.Z);
                boundingBox = new AxisAlignedBoundingBox(min, infiniteMax);
            }
            else if (normal.AngleBetween(YPositive()) < delta)
            {
                var max = new Vector3(decimal.MaxValue, (decimal)plane.Location.Y, decimal.MaxValue);
                boundingBox = new AxisAlignedBoundingBox(infiniteMin, max);
            }
            else if (normal.AngleBetween(YNegative()) < delta)
            {
                var min = new Vector3(decimal.MinValue, (decimal)plane.Location.Y, decimal.MinValue);
                boundingBox = new AxisAlignedBoundingBox(min, infiniteMax);
            }
            else if (normal.AngleBetween(XPositive()) < delta)
            {
                var max = new Vector3((decimal)plane.Location.X, decimal.MaxValue, decimal.MaxValue);
                boundingBox = new AxisAlignedBoundingBox(infiniteMin, max);
            }
            else if (normal.AngleBetween(XNegative()) < delta)
            {
                var min = new Vector3((decimal)plane.Location.X, decimal.MinValue, decimal.MinValue);
                boundingBox = new AxisAlignedBoundingBox(min, infiniteMax);
            }

            return boundingBox;
        }
    }
}
