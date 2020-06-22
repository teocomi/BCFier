using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenProject.Data.Utils
{
  /// <summary>
  /// Convert a DateTime or string to a relative date
  /// </summary>
  public static class RelativeDate
  {
    public static string ToRelative(string DateString)
    {
      DateTime theDate = Convert.ToDateTime(DateString);
      var thresholds = new Dictionary<long, string>();
      int minute = 60;
      int hour = 60 * minute;
      int day = 24 * hour;
      thresholds.Add(60, "{0} seconds ago");
      thresholds.Add(minute * 2, "a minute ago");
      thresholds.Add(45 * minute, "{0} minutes ago");
      thresholds.Add(120 * minute, "an hour ago");
      thresholds.Add(day, "{0} hours ago");
      thresholds.Add(day * 2, "yesterday");
      // thresholds.Add(day * 30, "{0} days ago");
      thresholds.Add(day * 365, "{0} days ago");
      thresholds.Add(long.MaxValue, "{0} years ago");

      long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
      foreach (long threshold in thresholds.Keys)
      {
        if (since < threshold)
        {
          TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
          return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
        }
      }
      return "";
    }
    public static string ToRelative(DateTime theDate)
    {

      var thresholds = new Dictionary<long, string>();
      int minute = 60;
      int hour = 60 * minute;
      int day = 24 * hour;
      thresholds.Add(60, "{0} seconds ago");
      thresholds.Add(minute * 2, "a minute ago");
      thresholds.Add(45 * minute, "{0} minutes ago");
      thresholds.Add(120 * minute, "an hour ago");
      thresholds.Add(day, "{0} hours ago");
      thresholds.Add(day * 2, "yesterday");
      // thresholds.Add(day * 30, "{0} days ago");
      thresholds.Add(day * 365, "{0} days ago");
      thresholds.Add(long.MaxValue, "{0} years ago");

      long since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
      foreach (long threshold in thresholds.Keys)
      {
        if (since < threshold)
        {
          TimeSpan t = new TimeSpan((DateTime.Now.Ticks - theDate.Ticks));
          return string.Format(thresholds[threshold], (t.Days > 365 ? t.Days / 365 : (t.Days > 0 ? t.Days : (t.Hours > 0 ? t.Hours : (t.Minutes > 0 ? t.Minutes : (t.Seconds > 0 ? t.Seconds : 0))))).ToString());
        }
      }
      return "";
    }
  }
}
