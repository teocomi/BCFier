using System.ComponentModel;
using System.Windows;

namespace OpenProject.Windows
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    /// <summary>
    /// passing event to the user control
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Window_Closing(object sender, CancelEventArgs e)
    {
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
    }
  }
}
