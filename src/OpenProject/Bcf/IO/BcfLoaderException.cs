using System;

namespace OpenProject.Bcf
{
  public class BcfLoaderException : Exception
  {
    public BcfLoaderException(string message, Exception innerException)
      :base(message, innerException)
    {
    }
  }
}
