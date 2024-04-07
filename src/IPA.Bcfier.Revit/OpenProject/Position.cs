// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

namespace IPA.Bcfier.Revit.OpenProject
{
    /// <summary>
    /// This immutable class describes a directed location in 3d space. It contains a center point
    /// and a forward and up vector to describe a defined orientation in 3d space.
    /// </summary>
    public sealed class Position : IEquatable<Position>
    {
        /// <summary>
        /// The center position vector.
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// The forward direction vector.
        /// </summary>
        public Vector3 Forward { get; }

        /// <summary>
        /// The up direction vector.
        /// </summary>
        public Vector3 Up { get; }

        public Position()
        {
            Center = Vector3.Zero;
            Forward = Vector3.Zero;
            Up = Vector3.Zero;
        }

        public Position(Vector3 center, Vector3 forward, Vector3 up)
        {
            Center = center;
            Forward = forward;
            Up = up;
        }

        /// <inheritdoc/>
        public bool Equals(Position other)
        {
            if (other == null) return false;

            return Center.Equals(other.Center) &&
                   Forward.Equals(other.Forward) &&
                   Up.Equals(other.Up);
        }
    }
}
