using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace OpenProject.Themes
{
  public partial class ViewCommentDataTemplate : ResourceDictionary
  {
    public ViewCommentDataTemplate()
       {
          InitializeComponent();
       }

    private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        //AddCommButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
      }
    }
  }
}
