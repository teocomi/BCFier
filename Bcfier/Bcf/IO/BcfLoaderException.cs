using System;

namespace Bcfier.Bcf
{
  public class BcfLoaderException : Exception
  {
    public BcfLoaderException(string message, Exception innerException)
      :base(message, innerException)
    {
    }
  }
}
