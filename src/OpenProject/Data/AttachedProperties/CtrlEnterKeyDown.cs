using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenProject.Data.AttachedProperties
{
  /// <summary>
  /// Binds Ctrl+Enter events to a command
  /// Edited from: http://stackoverflow.com/questions/4834227/invoke-command-when-enter-key-is-pressed-in-xaml
  /// </summary>
  public sealed class CtrlEnterKeyDown
  {
    #region Properties

    #region Command

    public static ICommand GetCommand(DependencyObject obj)
    {
      return (ICommand)obj.GetValue(CommandProperty);
    }

    public static void SetCommand(DependencyObject obj, ICommand value)
    {
      obj.SetValue(CommandProperty, value);
    }

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(CtrlEnterKeyDown),
            new PropertyMetadata(null, OnCommandChanged));

    #endregion Command

    #region CommandArgument

    public static object GetCommandArgument(DependencyObject obj)
    {
      return (object)obj.GetValue(CommandArgumentProperty);
    }

    public static void SetCommandArgument(DependencyObject obj, object value)
    {
      obj.SetValue(CommandArgumentProperty, value);
    }

    public static readonly DependencyProperty CommandArgumentProperty =
        DependencyProperty.RegisterAttached("CommandArgument", typeof(object), typeof(CtrlEnterKeyDown),
            new PropertyMetadata(null, OnCommandArgumentChanged));

    #endregion CommandArgument

    #region HasCommandArgument


    private static bool GetHasCommandArgument(DependencyObject obj)
    {
      return (bool)obj.GetValue(HasCommandArgumentProperty);
    }

    private static void SetHasCommandArgument(DependencyObject obj, bool value)
    {
      obj.SetValue(HasCommandArgumentProperty, value);
    }

    private static readonly DependencyProperty HasCommandArgumentProperty =
        DependencyProperty.RegisterAttached("HasCommandArgument", typeof(bool), typeof(CtrlEnterKeyDown),
            new PropertyMetadata(false));


    #endregion HasCommandArgument

    #endregion Propreties

    #region Event Handling

    private static void OnCommandArgumentChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      SetHasCommandArgument(o, true);
    }

    private static void OnCommandChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
    {
      FrameworkElement element = o as FrameworkElement;
      if (element != null)
      {
        if (e.NewValue == null)
        {
          element.KeyDown -= new KeyEventHandler(FrameworkElement_KeyDown);
        }
        else if (e.OldValue == null)
        {
          element.KeyDown += new KeyEventHandler(FrameworkElement_KeyDown);
        }
      }
    }

    private static void FrameworkElement_KeyDown(object sender, KeyEventArgs e)
    {
       
      if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Enter)
      {
        DependencyObject o = sender as DependencyObject;
        ICommand command = GetCommand(sender as DependencyObject);

        FrameworkElement element = e.OriginalSource as FrameworkElement;
        if (element != null)
        {
          // If the command argument has been explicitly set (even to NULL)
          if (GetHasCommandArgument(o))
          {
            object commandArgument = GetCommandArgument(o);

            // Execute the command
            if (command.CanExecute(commandArgument))
            {
              command.Execute(commandArgument);
            }
          }
          else if (command.CanExecute(element.DataContext))
          {
            command.Execute(element.DataContext);
          }
        }
      }
    }

    #endregion
  }
}
