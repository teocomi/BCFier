using OpenProject.ViewModels;
using System.Windows;

namespace OpenProject.Windows
{
  /// <summary>
  /// Interaction logic for OpenProjectSync.xaml
  /// </summary>
  public partial class OpenProjectSync : Window
  {
    public OpenProjectSync(OpenProjectSyncViewModel viewModel)
    {
      InitializeComponent();
      DataContext = viewModel;
    }
  }
}
