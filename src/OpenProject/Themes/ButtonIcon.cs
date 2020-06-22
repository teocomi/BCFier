using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OpenProject.Themes
{
  /// <summary>
  /// A custom button with an icon and different settings
  /// Can be Vertical
  /// </summary>
  public class ButtonIcon : Button
  {
    static ButtonIcon()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
          typeof(ButtonIcon),
          new FrameworkPropertyMetadata(typeof(ButtonIcon)));
    }

    public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register("IconColor", typeof(Brush), typeof(ButtonIcon));
    public Brush IconColor
    {
      get { return (Brush)GetValue(IconColorProperty); }
      set { SetValue(IconColorProperty, value); }
    }
    public static readonly DependencyProperty DisabledIconColorProperty = DependencyProperty.Register("DisabledIconColor", typeof(Brush), typeof(ButtonIcon));
    public Brush DisabledIconColor
    {
      get { return (Brush)GetValue(DisabledIconColorProperty); }
      set { SetValue(DisabledIconColorProperty, value); }
    }

    public static readonly DependencyProperty HighlightColorProperty = DependencyProperty.Register("HighlightColor", typeof(Brush), typeof(ButtonIcon));
    public Brush HighlightColor
    {
      get { return (Brush)GetValue(HighlightColorProperty); }
      set { SetValue(HighlightColorProperty, value); }
    }

    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(int), typeof(ButtonIcon), new UIPropertyMetadata(15));
    public int IconSize
    {
      get { return (int)GetValue(IconSizeProperty); }
      set { SetValue(IconSizeProperty, value); }
    }

    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(ButtonIcon), new UIPropertyMetadata(new Thickness(5, 0, 4, 0)));
    public Thickness IconMargin
    {
      get { return (Thickness)GetValue(IconMarginProperty); }
      set { SetValue(IconMarginProperty, value); }
    }

    public static readonly DependencyProperty IconSizeVerticalProperty = DependencyProperty.Register("IconSizeVertical", typeof(int), typeof(ButtonIcon), new UIPropertyMetadata(25));
    public int IconSizeVertical
    {
      get { return (int)GetValue(IconSizeVerticalProperty); }
      set { SetValue(IconSizeVerticalProperty, value); }
    }

    public static readonly DependencyProperty IconMarginVerticalProperty = DependencyProperty.Register("IconMarginVertical", typeof(Thickness), typeof(ButtonIcon), new UIPropertyMetadata(new Thickness(5)));
    public Thickness IconMarginVertical
    {
      get { return (Thickness)GetValue(IconMarginVerticalProperty); }
      set { SetValue(IconMarginVerticalProperty, value); }
    }

    public static readonly DependencyProperty IconPathProperty = DependencyProperty.Register("IconPath", typeof(Geometry), typeof(ButtonIcon));
    public System.Windows.Media.Geometry IconPath
    {
      get { return (Geometry)GetValue(IconPathProperty); }
      set { SetValue(IconPathProperty, value); }
    }

    public static readonly DependencyProperty IsVerticalProperty = DependencyProperty.Register(
        "IsVertical",
        typeof(Boolean),
        typeof(ButtonIcon),
         new PropertyMetadata(false));

    public Boolean IsVertical
    {
      get { return (Boolean)GetValue(IsVerticalProperty); }
      set { SetValue(IsVerticalProperty, value); }
    }

  }
}
