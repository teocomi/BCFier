using Autodesk.Revit.DB;
using Bcfier.Bcf.Bcf2;
using System;
using Point = Bcfier.Bcf.Bcf2.Point;

namespace Bcfier.Revit.Data
{
  public static class RevitUtils
  {
    /// <summary>
    //MOVES THE CAMERA ACCORDING TO THE PROJECT BASE LOCATION 
    //function that changes the coordinates accordingly to the project base location to an absolute location (for BCF export)
    //if the value negative is set to true, does the opposite (for opening BCF views)
    /// </summary>
    /// <param name="c">center</param>
    /// <param name="view">view direction</param>
    /// <param name="up">up direction</param>
    /// <param name="negative">convert to/from</param>
    /// <returns></returns>
    public static ViewOrientation3D ConvertBasePoint(Document doc, XYZ c, XYZ view, XYZ up, bool negative)
    {
      double angle = 0;
      double x = 0;
      double y = 0;
      double z = 0;

      //VERY IMPORTANT
      //BuiltInParameter.BASEPOINT_EASTWEST_PARAM is the value of the BASE POINT LOCATION
      //position is the location of the BPL related to Revit's absolute origin
      //if BPL is set to 0,0,0 not always it corresponds to Revit's origin

      XYZ origin = new XYZ(0, 0, 0);
#if Version2019 || Version2020 || Version2021 || Version2022
      ProjectPosition position = doc.ActiveProjectLocation.GetProjectPosition(origin);
#else
      ProjectPosition position = doc.ActiveProjectLocation.get_ProjectPosition(origin);
#endif

      int i = (negative) ? -1 : 1;

      x = i * position.EastWest;
      y = i * position.NorthSouth;
      z = i * position.Elevation;
      angle = i * position.Angle;

      if (negative) // I do the addition BEFORE
        c = new XYZ(c.X + x, c.Y + y, c.Z + z);

      //rotation
      double centX = (c.X * Math.Cos(angle)) - (c.Y * Math.Sin(angle));
      double centY = (c.X * Math.Sin(angle)) + (c.Y * Math.Cos(angle));

      XYZ newC = new XYZ();
      if (negative)
        newC = new XYZ(centX, centY, c.Z);
      else // I do the addition AFTERWARDS
        newC = new XYZ(centX + x, centY + y, c.Z + z);


      double viewX = (view.X * Math.Cos(angle)) - (view.Y * Math.Sin(angle));
      double viewY = (view.X * Math.Sin(angle)) + (view.Y * Math.Cos(angle));
      XYZ newView = new XYZ(viewX, viewY, view.Z);

      double upX = (up.X * Math.Cos(angle)) - (up.Y * Math.Sin(angle));
      double upY = (up.X * Math.Sin(angle)) + (up.Y * Math.Cos(angle));

      XYZ newUp = new XYZ(upX, upY, up.Z);
      return new ViewOrientation3D(newC, newUp, newView);
    }

    public static XYZ GetRevitXYZ(double X, double Y, double Z)
    {
      return new XYZ(X.ToFeet(), Y.ToFeet(), Z.ToFeet());
    }

    public static XYZ GetRevitXYZ(Direction d)
    {
      return new XYZ(d.X.ToFeet(), d.Y.ToFeet(), d.Z.ToFeet());
    }

    public static XYZ GetRevitXYZ(Point d)
    {
      return new XYZ(d.X.ToFeet(), d.Y.ToFeet(), d.Z.ToFeet());
    }

    /// <summary>
    /// Converts feet units to meters
    /// </summary>
    /// <param name="feet">Value in feet to be converted to meters</param>
    /// <returns></returns>
    public static double ToMeters(this double feet)
    {
#if Version2021 || Version2022
      return UnitUtils.ConvertFromInternalUnits(feet, UnitTypeId.Meters);
#else
      return UnitUtils.ConvertFromInternalUnits(feet, DisplayUnitType.DUT_METERS);
#endif
    }
    /// <summary>
    /// Converts meters units to feet
    /// </summary>
    /// <param name="meters">Value in feet to be converted to feet</param>
    /// <returns></returns>
    public static double ToFeet(this double meters)
    {
#if Version2021 || Version2022
      return UnitUtils.ConvertToInternalUnits(meters, UnitTypeId.Meters);
#else
      return UnitUtils.ConvertToInternalUnits(meters, DisplayUnitType.DUT_METERS);
#endif

    }
  }

}
