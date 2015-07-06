using System.Windows.Controls;
using System.Windows.Input;

namespace Bcfier.Themes
{
  public partial class IssueListStyle
  {
    protected void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
    {
      var item = (ListViewItem)sender;
      item.IsSelected = true;
    }
  }
}
