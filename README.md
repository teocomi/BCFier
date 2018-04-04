![](/Assets/bcfier-text.png)



## Intro

BCFier is an extendible and Open Source BCF client. Like IFC is the open standard for Building Information models, [BCF](https://github.com/BuildingSMART/BCF-XML) is the open standard for Building Issues. BCFier is a set of plugins and standalone apps (modules) that handle BCF and integrate directly with BIM tools.

Currently BCFier is composed of the following modules:
- Autodesk Revit addin
- Standalone Windows Viewer

Ready to start developing BCFier? Keep reading!

**If you are looking for a Guide on how to use the installed version of BCFier instedad, refer to the [UserGuide](http://bcfier.com/userguide/)**

## Disclaimer
The project in not actively maintained, I will regularly check issues and pull requests but cannot guarantee regular support and maintenance.

## Getting Started

To get started fork the repo, if you are going to extend the Revit Project make sure the Autodesk dlls are referenced correctly, otherwise there are no other dependencies that need to be added.

### Structure

The core of BCFier is under `Bcfier`, it contains all the logic and UI that is used by all the different integrations (modules). All modules will reference that project and extend it adding specific commands for the software they are integrating with.

The control `Bcfier.UserControls.BcfierPanel` contains the logic and UI for the main panel, while `Bcfier.UserControls.BcfReportPanel` for each BCF opened inside the TabControl.

All controls bind to ModelViews defined in `Bcfier.Bcf`, it's not a perfect MVVM models since I use the same classes to serialize/deserialize BCFs, but it works great.

### Creating a new Module

To create a new Module, for instance, an Achicad plugin, follow these steps:
- create a new project with the namespace `Bcfier.Archicad`
- reference the `Bcfier` project
- add the specific Archicad methods and structure to fire the plugin (like the Entry folder in the Revit plugin)
- create a main WPF window that contains the `Bcfier.UserControls.BcfierPanel`
- create a command for adding a new view (`data:Commands.AddView`), this will have to generate a BCF ViewPoint (see Revit plugin for reference)
- create a command for and opening a view (`data:Commands.OpenView`)
- extend the installer to copy these new dlls where needed

### Settings
The settings file is stored in `%localappdata%\BCFier\settings.config` so that it can be accessible by all modules, the Settingd Window UI will has different tabs for each module and ideally those will show up only if that specific module is installed.
The class that handles the settings file is under `Bcfier.Data.Utils.UserSettings`, and stores the file as a `ExeConfigurationFileMap` for easy management. The same class provides methods to automatically save/retrieve settings based on the UserControl name.

## Autodesk Revit Addin
The module for Autodesk Revit is in `Bcfier.Revit`,.

### Building the Revit Project

Before building the Revit project, select the corresponding build configuration. 

![image](https://user-images.githubusercontent.com/2679513/33550628-93d0661e-d8e6-11e7-819d-b486b55db05c.png)

For each there are snippets of code in Bcfier.Revit.csproj with post built event that copy the dll and manifest to the Revit Addin folder.

![image](https://user-images.githubusercontent.com/2679513/33550664-b19fb028-d8e6-11e7-8453-3210d022d0db.png)



To seamlessly debug the project set a Debug start action to start your version of revit.exe.ree and awesome [InnoSetup](http://www.jrsoftware.org/isinfo.php) to generate .exe files, extending the .iss files is pretty straightforward.

## Backlog
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

### Contact
You can contact Matteo Cominetti at: hello@teocomi.com

### License
GNU General Public License v3 Extended
This program uses the GNU General Public License v3, extended to support the use of BCFier as Plugin of the non-free main software Autodesk Revit.
See <http://www.gnu.org/licenses/gpl-faq.en.html#GPLPluginsInNF>.

Copyright (c) 2013-2016 Matteo Cominetti

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/gpl.txt>.
