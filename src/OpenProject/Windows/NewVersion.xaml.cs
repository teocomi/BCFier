using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OpenProject.Windows
{
  /// <summary>
  /// Interaction logic for NewVersion.xaml
  /// </summary>
  public partial class NewVersion : Window
  {
    public NewVersion()
    {
      InitializeComponent();
    }

    private void Button_Cancel(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
    }

    private void Button_OK(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
    }
  }
}
