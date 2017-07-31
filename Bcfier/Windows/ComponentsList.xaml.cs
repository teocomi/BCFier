using System.Collections.Generic;
using System.Windows;
using Bcfier.Bcf.Bcf2;


namespace Bcfier.Windows
{
  /// <summary>
  /// Interaction logic for Components.xaml
  /// </summary>
  public partial class ComponentsList : Window
  {
    public ComponentsList(Components components)
    {
      InitializeComponent();
      //TODO: properly display components info
      componentsList.ItemsSource = components.Selection;

    }
  }
}
