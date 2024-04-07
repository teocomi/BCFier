// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

namespace IPA.Bcfier.Revit.OpenProject
{
    /// <summary>
    /// Immutable implementation of an axis aligned bounding box with decimal precision.
    /// </summary>
    public sealed class AxisAlignedBoundingBox : IEquatable<AxisAlignedBoundingBox>
    {
        /// <summary>
        /// A bounding box with max values, representing an infinite bounding box
        /// </summary>
        public static AxisAlignedBoundingBox Infinite =>
          new AxisAlignedBoundingBox(Vector3.InfiniteMin, Vector3.InfiniteMax);

        /// <summary>
        /// This min corner of the axis aligned bounding box.
        /// </summary>
        public Vector3 Min { get; }

        /// <summary>
        /// This min corner of the axis aligned bounding box.
        /// </summary>
        public Vector3 Max { get; }

        public AxisAlignedBoundingBox(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Merges this bounding box with the given one and returns the resulting bounding box, that
        /// is included in both bounding boxes. If there is no intersection between both bounding
        /// boxes, an infinite bounding box is returned.
        /// </summary>
        /// <param name="box">The other bounding box</param>
        /// <returns>The merged bounding box.</returns>
        public AxisAlignedBoundingBox MergeReduce(AxisAlignedBoundingBox box)
        {
            var min = new Vector3(
              Math.Max(Min.X, box.Min.X),
              Math.Max(Min.Y, box.Min.Y),
              Math.Max(Min.Z, box.Min.Z));
            var max = new Vector3(
              Math.Min(Max.X, box.Max.X),
              Math.Min(Max.Y, box.Max.Y),
              Math.Min(Max.Z, box.Max.Z));

            // validity check
            if (min.X < max.X && min.Y < max.Y && min.Z < max.Z)
                return new AxisAlignedBoundingBox(min, max);

            return Infinite;
        }

        /// <inheritdoc/>
        public bool Equals(AxisAlignedBoundingBox other)
        {
            if (other == null)
                return false;

            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }
    }
}
