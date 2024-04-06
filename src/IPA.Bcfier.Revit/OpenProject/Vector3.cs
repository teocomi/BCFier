// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

namespace IPA.Bcfier.Revit.OpenProject
{
    /// <summary>
    /// A light-weight vector class for 3d vectors, containing values in decimal precision.
    /// </summary>
    public sealed class Vector3 : IEquatable<Vector3>
    {
        /// <summary>
        /// A vector of max values, representing an positive infinite point.
        /// </summary>
        public static Vector3 InfiniteMax =>
          new Vector3(decimal.MaxValue, decimal.MaxValue, decimal.MaxValue);

        /// <summary>
        /// A vector of min values, representing an negative infinite point.
        /// </summary>
        public static Vector3 InfiniteMin =>
          new Vector3(decimal.MinValue, decimal.MinValue, decimal.MinValue);

        /// <summary>
        /// A vector of zero values, representing the zero vector.
        /// </summary>
        public static Vector3 Zero => new Vector3(0, 0, 0);

        public decimal X { get; }
        public decimal Y { get; }
        public decimal Z { get; }

        public Vector3(decimal x, decimal y, decimal z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Calculates the angle between this vector and the given one.
        /// </summary>
        /// <param name="vec">The other vector.</param>
        /// <returns>The angle in radians.</returns>
        public decimal AngleBetween(Vector3 vec)
        {
            if (Equals(Zero) || vec.Equals(Zero))
                throw new ArgumentException("no angle calculation possible for zero vectors");

            return DecimalMath.DecimalEx.ACos(this * vec / (Euclidean() * vec.Euclidean()));
        }

        /// <summary>
        /// Calculates the sum of this vector and the given one.
        /// </summary>
        /// <param name="v1">The left vector.</param>
        /// <param name="v2">The right vector.</param>
        /// <returns>The dot product value.</returns>
        public static Vector3 operator +(Vector3 v1, Vector3 v2) => new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

        /// <summary>
        /// Calculates the dot product of this vector and the given one.
        /// </summary>
        /// <param name="v1">The left vector.</param>
        /// <param name="v2">The right vector.</param>
        /// <returns>The dot product value.</returns>
        public static decimal operator *(Vector3 v1, Vector3 v2) =>
          v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;

        /// <summary>
        /// Calculates the scalar product of this vector and a scalar value.
        /// </summary>
        /// <param name="v1">The vector.</param>
        /// <param name="scalar">The scalar value.</param>
        /// <returns>The dot product value.</returns>
        public static Vector3 operator *(Vector3 v1, decimal scalar) =>
          new Vector3(v1.X * scalar, v1.Y * scalar, v1.Z * scalar);

        /// <summary>
        /// Calculates the euclidean distance of this vector.
        /// </summary>
        /// <returns>The euclidean distance.</returns>
        public decimal Euclidean() => DecimalMath.DecimalEx.Sqrt(X * X + Y * Y + Z * Z);

        /// <inheritdoc/>
        public bool Equals(Vector3 other)
        {
            if (other == null) return false;

            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }
}
