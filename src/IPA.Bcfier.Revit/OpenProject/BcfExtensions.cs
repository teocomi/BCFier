// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

using IPA.Bcfier.Models.Bcf;

namespace IPA.Bcfier.Revit.OpenProject
{
    public static class BcfExtensions
    {
        public static Vector3 ToVector3(this BcfViewpointVector direction) =>
          new Vector3(
            direction.X.ToDecimal(),
            direction.Y.ToDecimal(),
            direction.Z.ToDecimal());

        public static Vector3 ToVector3(this BcfViewpointPoint point) =>
          new Vector3(
            point.X.ToDecimal(),
            point.Y.ToDecimal(),
            point.Z.ToDecimal());

        /// <summary>
        /// Converts a axis aligned bounding box into a list of bcf api clipping planes.
        /// </summary>
        /// <param name="clippingBox">
        /// The bounding box that defines the clipping. Can contain infinite values, which are
        /// interpreted as if the view is not clipped in that direction.
        /// </param>
        /// <param name="clippingCenter">
        /// An optional clipping center. Important for positioning the clipping planes not too far
        /// away from the model. If no clipping center is given, the center of the clipping box is
        /// used, which can result in very odd clipping plane locations, if the clipping box
        /// contains infinite values.
        /// </param>
        /// <returns>A list of clipping planes.</returns>
        public static List<BcfViewpointClippingPlane> ToClippingPlanes(
          this AxisAlignedBoundingBox clippingBox,
          Vector3 clippingCenter = null)
        {
            Vector3 center = clippingCenter ?? (clippingBox.Min + clippingBox.Max) * 0.5m;

            var planes = new List<BcfViewpointClippingPlane>();

            if (clippingBox.Min.X.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(clippingBox.Min.X),
                        Y = Convert.ToSingle(center.Y),
                        Z = Convert.ToSingle(center.Z)
                    },
                    Direction = new BcfViewpointVector { X = -1, Y = 0, Z = 0 }
                });
            }

            if (clippingBox.Min.Y.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(center.X),
                        Y = Convert.ToSingle(clippingBox.Min.Y),
                        Z = Convert.ToSingle(center.Z)
                    },
                    Direction = new BcfViewpointVector { X = 0, Y = -1, Z = 0 }
                });
            }

            if (clippingBox.Min.Z.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(center.X),
                        Y = Convert.ToSingle(center.Y),
                        Z = Convert.ToSingle(clippingBox.Min.Z)
                    },
                    Direction = new BcfViewpointVector { X = 0, Y = 0, Z = -1 }
                });
            }

            if (clippingBox.Max.X.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(clippingBox.Max.X),
                        Y = Convert.ToSingle(center.Y),
                        Z = Convert.ToSingle(center.Z)
                    },
                    Direction = new BcfViewpointVector { X = 1, Y = 0, Z = 0 }
                });
            }

            if (clippingBox.Max.Y.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(center.X),
                        Y = Convert.ToSingle(clippingBox.Max.Y),
                        Z = Convert.ToSingle(center.Z)
                    },
                    Direction = new BcfViewpointVector { X = 0, Y = 1, Z = 0 }
                });
            }

            if (clippingBox.Max.Z.IsFinite())
            {
                planes.Add(new BcfViewpointClippingPlane
                {
                    Location = new BcfViewpointPoint
                    {
                        X = Convert.ToSingle(center.X),
                        Y = Convert.ToSingle(center.Y),
                        Z = Convert.ToSingle(clippingBox.Max.Z)
                    },
                    Direction = new BcfViewpointVector { X = 0, Y = 0, Z = 1 }
                });
            }

            return planes;
        }
    }
}
