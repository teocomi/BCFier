using System.Windows;
using System.Windows.Controls;

namespace OpenProject.Themes
{
  /// <summary>
  /// A control that looks like a label but can be selected
  /// More code in generic.xaml
  /// </summary>
    public class LabelSelectable : TextBox
    {
        static LabelSelectable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(LabelSelectable),
                new FrameworkPropertyMetadata(typeof(LabelSelectable)));
        }
    }
}
