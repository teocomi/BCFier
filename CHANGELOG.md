#Changelog
### BCFier 2.2.0 - 07/08/2017

#### New Features

- implemented BCF 2.1 schema [#29](https://github.com/teocomi/BCFier/issues/29)
- added *topic status* and *topic type* to UI and settings
- removed *comment status* and *verbal status*
- issues can be rearranged via [drag&drop interface](https://github.com/punker76/gong-wpf-dragdrop)
- added support for *topic* *index* and *viewpoint* *index*: this value is set on save [#14](https://github.com/teocomi/BCFier/issues/14)
- upgraded solution to Visual Studio 2017
- merged and linkedAssemblyInfo files

#### Bug Fixes

- exception raised when view is captured from schedule (Revit): now a friendly warning message will show, no workaround has been implemented to grab a snapshot of the schedule  [#12](https://github.com/teocomi/BCFier/issues/12)


#### To Do

- support of extension.xsd
- support of ReferenceLink
- support of BimSnippet