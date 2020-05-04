;defining variables
#define Repository     "..\."
#define MyAppName      "BCFier"
#define MyAppVersion GetFileVersion("..\Bcfier\bin\Release\Bcfier.dll")
#define MyAppPublisher "Matteo Cominetti"
#define MyAppURL       "http://www.bcfier.com/"
#define MyAppExeName   "Bcfier.Win.exe"

#define RevitAppName  "Bcfier.Revit"
#define RevitAddinFolder "{sd}\ProgramData\Autodesk\Revit\Addins"
#define RevitFolder19 RevitAddinFolder+"\2019\"+RevitAppName
#define RevitAddin19  RevitAddinFolder+"\2019\"
#define RevitFolder20 RevitAddinFolder+"\2020\"+RevitAppName
#define RevitAddin20  RevitAddinFolder+"\2020\"

#define WinAppName    "Bcfier.Win"


[Setup]
AppId={{0d553633-80f8-490b-84d6-9d3d6ad4196d}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
DisableWelcomePage=no
OutputDir={#Repository}\output
OutputBaseFilename=BCFier
SetupIconFile={#Repository}\Assets\icon.ico
Compression=lzma
SolidCompression=yes
WizardImageFile={#Repository}\Assets\bcfier-banner.bmp
ChangesAssociations=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Components]
Name: revit19; Description: Addin for Autodesk Revit 2019;  Types: full
Name: revit20; Description: Addin for Autodesk Revit 2020;  Types: full
Name: standalone; Description: BCFier for Windows; Types: full

[Dirs]
Name: "{app}"; Permissions: everyone-full 

[Files]
;STANDALONE
Source: "{#Repository}\output\{#WinAppName}\{#WinAppName}.exe"; DestDir: "{app}"; Flags: ignoreversion; Permissions: everyone-full; Components: standalone
Source: "{#Repository}\output\{#WinAppName}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs; Components: standalone
Source: "{#Repository}\Assets\BCF.ico"; DestDir: "{app}"; Flags: ignoreversion; Components: standalone

;REVIT 2019                                                                                                                                    
Source: "{#Repository}\output\2019\{#RevitAppName}\*"; DestDir: "{#RevitFolder19}"; Flags: ignoreversion recursesubdirs; Components: revit19 
Source: "{#Repository}\output\2019\{#RevitAppName}\{#RevitAppName}.addin"; DestDir: "{#RevitAddin19}"; Flags: ignoreversion; Components: revit19

;REVIT 2020                                                                                                                                    
Source: "{#Repository}\output\2020\{#RevitAppName}\*"; DestDir: "{#RevitFolder20}"; Flags: ignoreversion recursesubdirs; Components: revit20 
Source: "{#Repository}\output\2020\{#RevitAppName}\{#RevitAppName}.addin"; DestDir: "{#RevitAddin20}"; Flags: ignoreversion; Components: revit20

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKCR; Subkey: ".bcfzip"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName}"; Flags: uninsdeletevalue;  Components: standalone
Root: HKCR; Subkey: "{#MyAppName}"; ValueType: string; ValueName: ""; ValueData: "BCF File"; Flags: uninsdeletekey;  Components: standalone
Root: HKCR; Subkey: "{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\BCF.ico"; Components: standalone
Root: HKCR; Subkey: "{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}""""%1"""; Components: standalone

;checks if minimun requirements are met
[Code]
function IsDotNetDetected(version: string; service: cardinal): boolean;
// Indicates whether the specified version and service pack of the .NET Framework is installed.
//
// version -- Specify one of these strings for the required .NET Framework version:
//    'v1.1.4322'     .NET Framework 1.1
//    'v2.0.50727'    .NET Framework 2.0
//    'v3.0'          .NET Framework 3.0
//    'v3.5'          .NET Framework 3.5
//    'v4\Client'     .NET Framework 4.0 Client Profile
//    'v4\Full'       .NET Framework 4.0 Full Installation
//    'v4.5'          .NET Framework 4.5
//
// service -- Specify any non-negative integer for the required service pack level:
//    0               No service packs required
//    1, 2, etc.      Service pack 1, 2, etc. required
var
    key: string;
    install, release, serviceCount: cardinal;
    check45, success: boolean;
begin
    // .NET 4.5 installs as update to .NET 4.0 Full
    if version = 'v4.5' then begin
        version := 'v4\Full';
        check45 := true;
    end else
        check45 := false;

    // installation key group for all .NET versions
    key := 'SOFTWARE\Microsoft\NET Framework Setup\NDP\' + version;

    // .NET 3.0 uses value InstallSuccess in subkey Setup
    if Pos('v3.0', version) = 1 then begin
        success := RegQueryDWordValue(HKLM, key + '\Setup', 'InstallSuccess', install);
    end else begin
        success := RegQueryDWordValue(HKLM, key, 'Install', install);
    end;

    // .NET 4.0/4.5 uses value Servicing instead of SP
    if Pos('v4', version) = 1 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Servicing', serviceCount);
    end else begin
        success := success and RegQueryDWordValue(HKLM, key, 'SP', serviceCount);
    end;

    // .NET 4.5 uses additional value Release
    if check45 then begin
        success := success and RegQueryDWordValue(HKLM, key, 'Release', release);
        success := success and (release >= 378389);
    end;

    result := success and (install = 1) and (serviceCount >= service);
end;

//Revit 2017/18 need 4.6, should update?
function InitializeSetup(): Boolean;
var
  ErrCode: integer;
begin
    if not IsDotNetDetected('v4.5', 0) then begin
      if  MsgBox('{#MyAppName} requires Microsoft .NET Framework 4.5.'#13#13
            'Do you want me to open http://www.microsoft.com/net'#13
            'so you can download it?',  mbConfirmation, MB_YESNO) = IDYES
            then begin
              ShellExec('open', 'http://www.microsoft.com/net',
                '', '', SW_SHOW, ewNoWait, ErrCode);
      end;
  
         result := false;
    end else
        result := true;
end;