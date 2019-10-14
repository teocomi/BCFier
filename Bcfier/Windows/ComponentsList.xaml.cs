using System.Windows;
using Bcfier.ViewModels.Bcf;

namespace Bcfier.Windows
{
  /// <summary>
  /// Interaction logic for Components.xaml
  /// </summary>
  public partial class ComponentsList : Window
  {
    //disabled for now
    public ComponentsList(BcfViewpointViewModel bcfViewpoint)
    {
      InitializeComponent();
      this.DataContext = bcfViewpoint;
      //componentsList.ItemsSource = components.Selection;
    }
  }
}
