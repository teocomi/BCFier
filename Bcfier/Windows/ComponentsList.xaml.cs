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
        public ComponentsList(IEnumerable<Component> components)
        {
            InitializeComponent();

            componentsList.ItemsSource = components;
            
        }
    }
}
