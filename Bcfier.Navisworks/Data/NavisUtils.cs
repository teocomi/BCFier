using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Navisworks.Api;
using Bcfier.Bcf.Bcf2;
using Point = Bcfier.Bcf.Bcf2.Point;
using Direction = Bcfier.Bcf.Bcf2.Direction;

namespace Bcfier.Navisworks.Data
{
  public static class NavisUtils
  {
    private static double _Units;

    public static Point3D GetNavisXYZ(Point d)
    {
      return new Point3D(d.X.ToInternal(), d.Y.ToInternal(), d.Z.ToInternal());
    }
    public static Vector3D GetNavisVector(Direction d)
    {
      return new Point3D(d.X.ToInternal(), d.Y.ToInternal(), d.Z.ToInternal()).ToVector3D().Normalize();
    }

    /// <summary>
    /// Converts meters units to feet
    /// </summary>
    /// <param name="meters">Value in feet to be converted to feet</param>
    /// <returns></returns>
    public static double ToInternal(this double meters)
    {
      return meters * _Units;
    }

    public static double FromInternal(this double intUnits)
    {
      return intUnits / _Units;
    }

    public static void  GetGunits(Document doc)
    {
      string units = doc.Units.ToString();
      double factor = 1;
      switch (units)
      {
        case "Centimeters":
          factor = 100;
          break;
        case "Feet":
          factor = 3.28084;
          break;
        case "Inches":
          factor = 39.3701;
          break;
        case "Kilometers":
          factor = 0.001;
          break;
        case "Meters":
          factor = 1;
          break;
        case "Micrometers":
          factor = 1000000;
          break;
        case "Miles":
          factor = 0.000621371;
          break;
        case "Millimeters":
          factor = 1000;
          break;
        case "Mils":
          factor = 39370.0787;
          break;
        case "Yards":
          factor = 1.09361;
          break;
        default:
          //MessageBox.Show("Units " + units + " not recognized.");
          factor = 1;
          break;
      }
      _Units = factor;
    }
  }
}
