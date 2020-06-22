using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace OpenProject.Data.AttachedProperties
{
  //Copied from http://stackoverflow.com/questions/861409/wpf-making-hyperlinks-clickable
  /// <summary>
  /// Renders the text of the comments as clickable links if detects URLs or local PATHs
  /// </summary>
  public static class NavigationService
  {
    //URL
    // (?#Protocol)(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)?(?#Username:Password)(?:\w+:\w+@)?(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?
    //PATH
    //((\\\\[a-zA-Z0-9-]+\\[a-zA-Z0-9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([ ]+[^ \\/:*?""<>|]+)*)*\\?
    private static readonly Regex RE_URL = new Regex(@"(?<url>(?:(?:ht|f)tp(?:s?)\:\/\/|~/|/)(?#Subdomains)(?:(?:[-\w]+\.)+(?#TopLevel Domains)(?:com|org|net|gov|mil|biz|info|mobi|name|aero|jobs|museum|travel|[a-z]{2}))(?#Port)(?::[\d]{1,5})?(?#Directories)(?:(?:(?:/(?:[-\w~!$+|.,=]|%[a-f\d]{2})+)+|/)+|\?|#)?(?#Query)(?:(?:\?(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)(?:&(?:[-\w~!$+|.,*:]|%[a-f\d{2}])+=(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)*)*(?#Anchor)(?:#(?:[-\w~!$+|.,*:=]|%[a-f\d]{2})*)?)|(?<path>\[(((\\\\[a-zA-Z0-9-]+\\[a-zA-Z0-9`~!@#$%^&(){}'._-]+([ ]+[a-zA-Z0-9`~!@#$%^&(){}'._-]+)*)|([a-zA-Z]:))(\\[^ \\/:*?""<>|]+([ ]+[^ \\/:*?""<>|]+)*)*\\?)\])");

    public static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
        "Text",
        typeof(string),
        typeof(NavigationService),
        new PropertyMetadata(null, OnTextChanged)
    );

    public static string GetText(DependencyObject d)
    { return d.GetValue(TextProperty) as string; }

    public static void SetText(DependencyObject d, string value)
    { d.SetValue(TextProperty, value); }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      try { 
      var text_block = d as TextBlock;
      if (text_block == null)
        return;

      text_block.Inlines.Clear();

      var new_text = (string)e.NewValue;
      if (string.IsNullOrEmpty(new_text))
        return;

      // Find all URLs using a regular expression
      int last_pos = 0;
      foreach (Match match in RE_URL.Matches(new_text))
      {
        var matchString = match.Value.Replace("[","").Replace("]","");
        Uri url = null;
        if (!Uri.TryCreate(matchString, UriKind.RelativeOrAbsolute, out url))
          continue;
        // Copy raw string from the last position up to the match
        if (match.Index != last_pos)
        {
          var raw_text = new_text.Substring(last_pos, match.Index - last_pos);
          text_block.Inlines.Add(new Run(raw_text));
        }
       
        // Create a hyperlink for the match
        var link = new Hyperlink(new Run(matchString))
        {
          NavigateUri = url
        };
        link.Click += OnUrlClick;

        text_block.Inlines.Add(link);

        // Update the last matched position
        last_pos = match.Index + match.Length;
      }

      // Finally, copy the remainder of the string
      if (last_pos < new_text.Length)
        text_block.Inlines.Add(new Run(new_text.Substring(last_pos)));
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }

    private static void OnUrlClick(object sender, RoutedEventArgs e)
    {
      var link = (Hyperlink)sender;
      // Do something with link.NavigateUri like:
      try { 
        Process.Start(link.NavigateUri.ToString());
      }
      catch (System.Exception ex1)
      {
      Console.WriteLine(ex1.Message);
      }
    }
  }
}
