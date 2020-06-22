using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace OpenProject.Themes
{
  /// <summary>
  /// A TextBox with a placeholder
  /// more code in generic.xaml
  /// </summary>
    public class TextPlaceholder : TextBox
    {

        public static DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                "Placeholder",
                typeof(string),
                typeof(TextPlaceholder));

        public static DependencyProperty PlaceholderColorProperty =
            DependencyProperty.Register(
                "PlaceholderColor",
                typeof(Brush),
                typeof(TextPlaceholder));


        private static DependencyPropertyKey HasTextPropertyKey =
            DependencyProperty.RegisterReadOnly(
                "HasText",
                typeof(bool),
                typeof(TextPlaceholder),
                new PropertyMetadata());
        public static DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;


        static TextPlaceholder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextPlaceholder),
                new FrameworkPropertyMetadata(typeof(TextPlaceholder)));
        }



        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            HasText = Text.Length != 0;

        }


        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Brush PlaceholderColor
        {
            get { return (Brush)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }



        public bool HasText
        {
            get { return (bool)GetValue(HasTextProperty); }
            private set { SetValue(HasTextPropertyKey, value); }
        }

    }
}
