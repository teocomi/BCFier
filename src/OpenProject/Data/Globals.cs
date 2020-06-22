using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProject.Data
{
  public static class Globals
  {
    public static List<string> OpenStatuses = new List<string>();
    public static List<string> OpenTypes = new List<string>();

    private static IEnumerable<string> availStatuses = new List<string>();
    private static IEnumerable<string> availTypes = new List<string>();

    //join with the list of statuses from other open BCFs
    public static IEnumerable<string> AvailStatuses
    {
      get { return availStatuses.Union(OpenStatuses); }
    }

    public static void SetStatuses(string statusString)
    {
      availStatuses = new List<string>(statusString.Split(new char[] { ',' }).Select(o => o.Trim()));
    }

    //join with the list of types from other open BCFs
    public static IEnumerable<string> AvailTypes
    {
      get { return availTypes.Union(OpenTypes); }
    }

    public static void SetTypes(string statusTypes)
    {
      availTypes = new List<string>(statusTypes.Split(new char[] { ',' }).Select(o => o.Trim()));
    }
  }
}
