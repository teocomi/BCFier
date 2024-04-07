// This was mostly taken from:
// https://github.com/opf/openproject-revit-add-in

using Autodesk.Revit.DB;

namespace IPA.Bcfier.Revit.OpenProject
{
    /// <summary>
    /// A immutable wrapper class around the <see cref="Autodesk.Revit.DB.ProjectPosition"/>
    /// </summary>
    public sealed class ProjectPositionWrapper
    {
        /// <summary>
        /// The translation of the project into east-west, mostly translated into a translation into x-direction.
        /// </summary>
        public decimal EastWest { get; }

        /// <summary>
        /// The translation of the project into north-south, mostly translated into a translation
        /// into y-direction.
        /// </summary>
        public decimal NorthSouth { get; }

        /// <summary>
        /// The elevation of the project, mostly translated into a translation into z-direction.
        /// </summary>
        public decimal Elevation { get; }

        /// <summary>
        /// The angle of the project north to true north.
        /// </summary>
        public decimal Angle { get; }

        public ProjectPositionWrapper(decimal eastWest, decimal northSouth, decimal elevation, decimal angle)
        {
            EastWest = eastWest;
            NorthSouth = northSouth;
            Elevation = elevation;
            Angle = angle;
        }

        public ProjectPositionWrapper()
        {
            EastWest = 0;
            NorthSouth = 0;
            Elevation = 0;
            Angle = 0;
        }

        public ProjectPositionWrapper(ProjectPosition projectPosition)
        {
            EastWest = projectPosition.EastWest.ToDecimal();
            NorthSouth = projectPosition.NorthSouth.ToDecimal();
            Elevation = projectPosition.Elevation.ToDecimal();
            Angle = projectPosition.Angle.ToDecimal();
        }

        /// <summary>
        /// Returns the project position's translation as a <see cref="Vector3"/>. The east-west
        /// translation is taken as x, the north-south as y and the elevation as z. This represents
        /// the coordinates of the project as an xyz-coordinate system.
        /// </summary>
        /// <returns>The project position's location as a vector.</returns>
        public Vector3 GetTranslation() => new(EastWest, NorthSouth, Elevation);
    }
}
