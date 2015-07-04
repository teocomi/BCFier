![]({{site.baseurl}}/Assets/bcfier-text.png)
##Intro
BCFier is an Open Source tool developed to help you create better buildings. 
Like IFC is the open standard for Building Information models, [BCF](http://www.buildingsmart-tech.org/specifications/bcf-releases) is the open standard for Building Issues. BCFier is a set of plugins and standalone apps (modules) that handle BCF and integrate directly with BIM tools.

Currently BCFier is composed of:
- Autodesk Revit 2015 and 2016 addin
- Standalone Windows Viewer

Ready to start developing BCFier? Keep reading!

**If you are looking for a Guide on how to use the installed version of BCFier instedad, refer to: Guide_To_Be_Creaded.**

##Disclaimer
I have only recently started to document this project, please be comprehensive and help by pointing out what is not clear or missing.

##Getting Started

To get started fork the repo, if you are going to extend the Revit Project make sure the dlls are referenced correctly, otherwise there are no other dependencies that need to be added. 

###Structure

The core of BCFier is under `Teocomi.Bcfier`, it contains all the logic and UI that is used by all the different integrations (modules). All modules will reference that project and extend it adding specific commands for the software they are integrating with.

The control `Teocomi.Bcfier.UserControls.BcfierPanel` contains the logic and UI for the main panel, while `Teocomi.Bcfier.UserControls.BcfReportPanel` for each BCF opened inside the TabControl.

All controls bind to ModelViews defined in `Teocomi.Bcfier.Bcf.Data`, it's not a perfect MVVM models since I use the same classes to serialize/deserialize BCFs, but it works great.

###Creating a new Module
To create a new Module, for instance, an Achicad plugin, follow these steps:
- create a new project with the namespace `Teocomi.Bcfier.Archicad`
- reference the `Teocomi.Bcfier` project
- add the specific Archicad methods and structure to fire the plugin (like the Entry folder in the Revit plugin)
- create a main WPF window that contains the `Teocomi.Bcfier.UserControls.BcfierPanel`
- create a command for adding a new view (`data:Commands.AddView`), this will have to generate a BCF ViewPoint (see Revit plugin for reference)
- create a command for and opening a view (`data:Commands.OpenView`)
- extend the installer to copy these new dlls where needed

###Settings
The settings file is stored in `%localappdata%\BCFier` so that it can be accessible by all modules, ideally the Settingd Window UI will have different tabs for each module and those will show up only if that specific module is installed.
The class that handles the settings file is under `Teocomi.Bcfier.Data.Utils.UserSettings`, and stores the file as a `ExeConfigurationFileMap` for easy management. The same class provides methods to automatically save/retrieve settings based on the UserControl name.

##Installer
The installer uses the free and awesome [InnoSetup](http://www.jrsoftware.org/isinfo.php) to generate .exe files, extending the .iss files is pretty straightforward.

##Backlog
A more detailed list of things that need to be done can be found in the [issues page](https://github.com/teocomi/BCFier/issues), but to start:

**New BCFier features**
- support of the BCF REST API
- integration with issue tracking platforms (JIRA, redmine...)

**New modules**
- Archicad
- Navisworks

**New Autodesk Revit module Features**
- support for crop boxes
- a setting to apply vew templates to new view




