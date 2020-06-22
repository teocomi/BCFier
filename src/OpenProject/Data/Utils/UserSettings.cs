using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using OpenProject.Themes;

namespace OpenProject.Data.Utils
{
  public static class UserSettings
  {
    public static string BCFierAppDataFolder { get; } = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BCFier");

    /// <summary>
    /// Retrives the user setting with the specified key, if nothing is found returns an empty string
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string Get(string key)
    {
      try
      {
        Configuration config = GetConfig();

        if (config == null)
          return string.Empty;


        KeyValueConfigurationElement element = config.AppSettings.Settings[key];
        if (element != null)
        {
          string value = element.Value;
          if (!string.IsNullOrEmpty(value))
            return value;
        }
        else
        {
          config.AppSettings.Settings.Add(key, "");
          config.Save(ConfigurationSaveMode.Modified);
        }
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return string.Empty;
    }
    /// <summary>
    /// Sets the user setting with the specified key and value, if it doesn't exists it is created
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private static void Set(string key, string value)
    {
      try
      {
        Configuration config = GetConfig();
        if (config == null)
          return;

        KeyValueConfigurationElement element = config.AppSettings.Settings[key];
        if (element != null)
          element.Value = value;
        else
          config.AppSettings.Settings.Add(key, value);

        config.Save(ConfigurationSaveMode.Modified);

      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
    }
    /// <summary>
    /// Retrives the user setting with the specified key and converts it to bool
    /// </summary>
    /// <param name="key"></param>
    /// <param name="defValue">If the key is not found or invalid return this</param>
    /// <returns></returns>
    public static bool GetBool(string key, bool defValue = false)
    {
      bool value = defValue;
      try
      {
        //if it doesn't exist, use the optional default value
        if(!Boolean.TryParse(Get(key), out value))
        value = defValue;
      }
      catch (System.Exception ex1)
      {
        MessageBox.Show("exception: " + ex1);
      }
      return value;
    }

    /// <summary>
    /// The configuration file used to store our settings
    /// Saved in a location accessible by all modules
    /// </summary>
    /// <returns></returns>
    private static Configuration GetConfig()
    {
      string _settings =
        System.IO.Path.Combine(BCFierAppDataFolder,
          "settings.config");
      var configMap = new ExeConfigurationFileMap {ExeConfigFilename = _settings};
      var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

      if (config == null)
        MessageBox.Show("Error loading the Configuration file.", "Configuration Error", MessageBoxButton.OK, MessageBoxImage.Error);
      return config;
    }

     /// <summary>
    /// Tries to set user controls to what they were last time the user used the app
    /// </summary>
    public static void LoadControlSettings(Control control)
    {
      try
      {
        if (control.GetType() == typeof(TextBox))
        {
          var textbox = control as TextBox;
          if (textbox == null)
            return;
          var value = Get(textbox.Name);
          if (!string.IsNullOrEmpty(value))
            textbox.Text = value;
        }
        else if (control.GetType() == typeof(PasswordBox))
        {
          var passwordBox = control as PasswordBox;
          if (passwordBox == null)
            return;
          var value = Get(passwordBox.Name);
          if (!string.IsNullOrEmpty(value))
            passwordBox.Password = value;
        }
        else if (control.GetType() == typeof(TextPlaceholder))
        {
          var textbox = control as TextPlaceholder;
          if (textbox == null)
            return;
          var value = Get(textbox.Name);
          if (!string.IsNullOrEmpty(value))
            textbox.Text = value;
        }
        else if (control.GetType() == typeof(ComboBox))
        {
          var combobox = control as ComboBox;
          if (combobox == null)
            return;
          var value = Get(combobox.Name);
          if (!string.IsNullOrEmpty(value))
          {
            int elemIndex = 0;
            int.TryParse(value, out elemIndex);
            if (combobox.Items.Count > elemIndex)
              combobox.SelectedIndex = elemIndex;
          }     
        }
        else if (control.GetType() == typeof(CheckBox))
        {
          var checkbox = control as CheckBox;
          if (checkbox == null)
            return;
          bool value;
          if (Boolean.TryParse(Get(checkbox.Name), out value))
            checkbox.IsChecked = value;
        }
        else if (control.GetType() == typeof(TabControl))
        {
          var tabcontrol = control as TabControl;
          if (tabcontrol == null)
            return;
          int value;
          if (int.TryParse(Get(tabcontrol.Name), out value))
            tabcontrol.SelectedIndex = value;
        }
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
      }
    }
    /// <summary>
    /// Tries to set user controls to what they were last time the user used the app
    /// </summary>
    public static void SaveControlSettings(Control control)
    {
      try
      {
        if (control.GetType() == typeof(TextBox))
        {
          var textbox = control as TextBox;
          if (textbox == null)
            return;
          Set(textbox.Name, textbox.Text);
        }
        else if (control.GetType() == typeof(PasswordBox))
        {
          var passwordBox = control as PasswordBox;
          if (passwordBox == null)
            return;
          Set(passwordBox.Name, passwordBox.Password);
        }
        else if (control.GetType() == typeof(TextPlaceholder))
        {
          var textbox = control as TextPlaceholder;
          if (textbox == null)
            return;
          Set(textbox.Name, textbox.Text);
        }
        else if (control.GetType() == typeof(ComboBox))
        {
          var combobox = control as ComboBox;
          if (combobox == null)
            return;
          var elemIndex = combobox.SelectedIndex;
          if (elemIndex != -1)
            Set(combobox.Name, elemIndex.ToString());
        }
        else if (control.GetType() == typeof(CheckBox))
        {
          var checkbox = control as CheckBox;
          if (checkbox == null || !checkbox.IsChecked.HasValue)
            return;
          Set(checkbox.Name, checkbox.IsChecked.Value.ToString());
        }
        else if (control.GetType() == typeof(TabControl))
        {
          var tabcontrol = control as TabControl;
          if (tabcontrol == null)
            return;
          Set(tabcontrol.Name, tabcontrol.SelectedIndex.ToString());
        }
      }
      catch (Exception ex)
      {
        Console.Write(ex.Message);
      }
    }
  }
}
