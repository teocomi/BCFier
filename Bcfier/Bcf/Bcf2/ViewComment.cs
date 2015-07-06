using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bcfier.Bcf.Bcf2;

namespace Bcfier.Bcf.Bcf2
{
  /// <summary>
  /// View Model that binds to the BcfReportPanel grouping comments and viewpoints
  /// Not part of BCF
  /// </summary>
  public class ViewComment
  {
    public ViewPoint Viewpoint
    {
      get; set; 
    }
    public ObservableCollection<Comment> Comments { get; set; }

  }
}
