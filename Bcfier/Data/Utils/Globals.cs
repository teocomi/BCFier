using System;
using System.Collections.Generic;
using System.Linq;

namespace Bcfier.Data.Utils
{
  public static class Globals 
  {
    private static IEnumerable<string> availStatuses = new List<string>();
    //custom comment statuses, available from any part of the application
    public static IEnumerable<string> AvailStatuses
    {
      get { return availStatuses; }
    }

    public static void SetStatuses(string statusString)
    {
      availStatuses = new List<string>(statusString.Split(new char[] { ',' }).Select(o => o.Trim()));
    }

  }
}
