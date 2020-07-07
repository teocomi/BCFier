using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OpenProject.Api
{
  // Taken from https://github.com/GeorgDangl/WebDocu/blob/master/src/Dangl.WebDocumentation/Services/Version.cs

  public class Version : IComparable<Version>
  {
    private readonly string[] _splitted;

    public Version(string version)
    {
      Value = version;
      _splitted = version.Split('.', '-');
      if (_splitted.Any()
        && _splitted[0].Length > 0
        && _splitted[0].StartsWith("v", StringComparison.InvariantCultureIgnoreCase))
      {
        _splitted[0] = _splitted[0].Substring(1);
      }
    }

    public string Value { get; }
    private const string _semVerRegex = @"^(\d+\.\d+\.\d+)-[a-zA-Z0-9]+-?(\d\d\d\d)$";

    public int CompareTo(Version other)
    {
      if (ReferenceEquals(this, other)) return 0;
      if (ReferenceEquals(null, other)) return 1;

      // Same base versions from a preview release
      // will be order by their commit number, e.g.:
      // 1.0.1-alpha-0003
      // 1.0.1-beta0004
      // For these two, they would be ordered by "0003" and "0004"
      if (Regex.IsMatch(Value, _semVerRegex)
          && Regex.IsMatch(other.Value, _semVerRegex))
      {
        var semVerComparison = CompareSemVer(other);
        if (semVerComparison != 0)
        {
          return semVerComparison;
        }
      }

      for (var i = 0; i < _splitted.Length; i++)
      {
        if (other._splitted.Length <= i)
        {
          return -1;
        }
        if (int.TryParse(_splitted[i], out var thisInt) && int.TryParse(other._splitted[i], out var otherInt))
        {
          if (thisInt != otherInt)
          {
            return thisInt > otherInt
                ? 1
                : -1;
          }
        }
        var stringComparison = string.CompareOrdinal(_splitted[i], other._splitted[i]);
        if (stringComparison != 0)
        {
          return stringComparison;
        }
      }
      return 1;
    }

    /// <summary>
    /// This will return 0 if the base version is different for the strings.
    /// Otherwise, it will compare by their commit counter
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private int CompareSemVer(Version other)
    {
      var thisRegexMatch = Regex.Match(Value, _semVerRegex);
      var otherRegexMatch = Regex.Match(other.Value, _semVerRegex);

      var thisBaseVersion = thisRegexMatch.Groups[1].Value;
      var otherBaseVersion = otherRegexMatch.Groups[1].Value;
      if (thisBaseVersion != otherBaseVersion)
      {
        return 0;
      }

      var thisCommitCount = thisRegexMatch.Groups[2].Value;
      var otherCommitCount = otherRegexMatch.Groups[2].Value;

      return thisCommitCount.CompareTo(otherCommitCount);
    }
  }
}
