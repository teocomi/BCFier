#define Repository     "./Installer"
#define AppName      "IPA BCFier Revit Plugin"
#define AppPublisher "Dangl IT GmbH"
#define AppURL       "https://www.dangl-it.com"

#define RevitAddinFolder "{sd}\ProgramData\Autodesk\Revit\Addins"
#define RevitAddin24  RevitAddinFolder+"\2024\"

[Setup]
AppId="cfd740a5-8089-4323-a6e5-c86cfee9cca2"
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
DefaultDirName={#RevitAddin24}
DisableDirPage=yes
DefaultGroupName={#AppName}
DisableProgramGroupPage=yes
DisableWelcomePage=no
OutputDir={#Repository}\output
OutputBaseFilename=IpaBcfierRevitPlugin
SetupIconFile={#Repository}\InstallerAssets\BCF.ico
Compression=lzma
SolidCompression=yes
WizardImageFile={#Repository}\InstallerAssets\banner.bmp
ChangesAssociations=yes

[Components]
Name: revit24; Description: Addin for Autodesk Revit 2024;  Types: full

[Files]

;REVIT 2024
Source: "{#Repository}\IPA.Bcfier.Revit.addin"; DestDir: "{#RevitAddin24}"; Flags: ignoreversion; Components: revit24
Source: "{#Repository}\DecimalEx.dll"; DestDir: "{#RevitAddin24}\Ipa.BCFier"; Flags: ignoreversion; Components: revit24
Source: "{#Repository}\Dangl.BCF.dll"; DestDir: "{#RevitAddin24}\Ipa.BCFier"; Flags: ignoreversion; Components: revit24
Source: "{#Repository}\IPA.Bcfier.dll"; DestDir: "{#RevitAddin24}\Ipa.BCFier"; Flags: ignoreversion; Components: revit24
Source: "{#Repository}\IPA.Bcfier.Revit.dll"; DestDir: "{#RevitAddin24}\Ipa.BCFier"; Flags: ignoreversion; Components: revit24
Source: "{#Repository}\bcfier-app\**\*"; DestDir: "{#RevitAddin24}\Ipa.BCFier\ipa-bcfier-app"; Flags: ignoreversion; Components: revit24
