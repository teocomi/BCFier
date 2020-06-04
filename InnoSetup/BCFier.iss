;defining variables
#define Repository     "..\."
#define MyAppName      "BCFier"
#define MyAppVersion GetFileVersion("..\Bcfier\bin\Release\Bcfier.dll")
#define MyAppPublisher "Matteo Cominetti"
#define MyAppURL       "http://www.bcfier.com/"
#define MyAppExeName   "Bcfier.Win.exe"

#define RevitAppName  "Bcfier.Revit"
;#define RevitAddinFolder "{sd}\ProgramData\Autodesk\Revit\Addins"
;#define RevitFolder15 RevitAddinFolder+"\2015\"+RevitAppName
;#define RevitAddin15  RevitAddinFolder+"\2015\"
;#define RevitFolder16 RevitAddinFolder+"\2016\"+RevitAppName
;#define RevitAddin16  RevitAddinFolder+"\2016\"
;#define RevitFolder17 RevitAddinFolder+"\2017\"+RevitAppName
;#define RevitAddin17  RevitAddinFolder+"\2017\"
;#define RevitFolder18 RevitAddinFolder+"\2018\"+RevitAppName
;#define RevitAddin18  RevitAddinFolder+"\2018\"
#define RevitFolder19 "{userappdata}\Autodesk\Revit\Addins\2019\"+RevitAppName
#define RevitAddin19  "{userappdata}\Autodesk\Revit\Addins\2019\"
#define RevitFolder20 "{userappdata}\Autodesk\Revit\Addins\2020\"+RevitAppName
#define RevitAddin20  "{userappdata}\Autodesk\Revit\Addins\2020\"
#define RevitFolder21 "{userappdata}\Autodesk\Revit\Addins\2021\"+RevitAppName
#define RevitAddin21  "{userappdata}\Autodesk\Revit\Addins\2021\"

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
DefaultDirName={userpf}\{#MyAppName}
DisableDirPage=yes
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
DisableWelcomePage=no
OutputDir={#Repository}
OutputBaseFilename=BCFier
SetupIconFile={#Repository}\Assets\icon.ico
Compression=lzma
SolidCompression=yes
WizardImageFile={#Repository}\Assets\bcfier-banner.bmp
ChangesAssociations=yes
PrivilegesRequired=lowest

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: checkedonce

[Components]
;Name: revit15; Description: Addin for Autodesk Revit 2015; Types: full 
;Name: revit16; Description: Addin for Autodesk Revit 2016;  Types: full
;Name: revit17; Description: Addin for Autodesk Revit 2017;  Types: full
;Name: revit18; Description: Addin for Autodesk Revit 2018;  Types: full
Name: revit19; Description: Addin for Autodesk Revit 2019;  Types: full
Name: revit20; Description: Addin for Autodesk Revit 2020;  Types: full
Name: revit21; Description: Addin for Autodesk Revit 2021;  Types: full
Name: standalone; Description: BCFier for Windows (standalone viewer); Types: full


[Dirs]
Name: "{app}"; Permissions: everyone-full 

[Files]
;STANDALONE
Source: "{#Repository}\{#WinAppName}\bin\Release\{#WinAppName}.exe"; DestDir: "{app}"; Flags: ignoreversion; Permissions: everyone-full; Components: standalone
Source: "{#Repository}\{#WinAppName}\bin\Release\Bcfier.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: standalone
Source: "{#Repository}\{#WinAppName}\bin\Release\GongSolutions.WPF.DragDrop.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: standalone
Source: "{#Repository}\{#WinAppName}\bin\Release\RestSharp.dll"; DestDir: "{app}"; Flags: ignoreversion; Components: standalone
Source: "{#Repository}\Assets\BCF.ico"; DestDir: "{app}"; Flags: ignoreversion; Components: standalone

;REVIT 2015                                                                                                                                        
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2015\{#RevitAppName}.dll"; DestDir: "{#RevitFolder15}"; Flags: ignoreversion; Components: revit15  
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2015\{#RevitAppName}.addin"; DestDir: "{#RevitAddin15}"; Flags: ignoreversion; Components: revit15
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2015\Bcfier.dll"; DestDir: "{#RevitFolder15}"; Flags: ignoreversion; Components: revit15
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2015\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder15}"; Flags: ignoreversion; Components: revit15
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2015\RestSharp.dll"; DestDir: "{#RevitFolder15}"; Flags: ignoreversion; Components: revit15

;REVIT 2016                                                                                                                                        
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2016\{#RevitAppName}.dll"; DestDir: "{#RevitFolder16}"; Flags: ignoreversion; Components: revit16  
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2016\{#RevitAppName}.addin"; DestDir: "{#RevitAddin16}"; Flags: ignoreversion; Components: revit16
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2016\Bcfier.dll"; DestDir: "{#RevitFolder16}"; Flags: ignoreversion; Components: revit16
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2016\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder16}"; Flags: ignoreversion; Components: revit16
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2016\RestSharp.dll"; DestDir: "{#RevitFolder16}"; Flags: ignoreversion; Components: revit16

;REVIT 2017                                                                                                                                     
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2017\{#RevitAppName}.dll"; DestDir: "{#RevitFolder17}"; Flags: ignoreversion; Components: revit17  
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2017\{#RevitAppName}.addin"; DestDir: "{#RevitAddin17}"; Flags: ignoreversion; Components: revit17
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2017\Bcfier.dll"; DestDir: "{#RevitFolder17}"; Flags: ignoreversion; Components: revit17
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2017\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder17}"; Flags: ignoreversion; Components: revit17
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2017\RestSharp.dll"; DestDir: "{#RevitFolder17}"; Flags: ignoreversion; Components: revit17

;REVIT 2018                                                                                                                                    
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2018\{#RevitAppName}.dll"; DestDir: "{#RevitFolder18}"; Flags: ignoreversion; Components: revit18  
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2018\{#RevitAppName}.addin"; DestDir: "{#RevitAddin18}"; Flags: ignoreversion; Components: revit18
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2018\Bcfier.dll"; DestDir: "{#RevitFolder18}"; Flags: ignoreversion; Components: revit18
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2018\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder18}"; Flags: ignoreversion; Components: revit18
;Source: "{#Repository}\{#RevitAppName}\bin\Release-2018\RestSharp.dll"; DestDir: "{#RevitFolder18}"; Flags: ignoreversion; Components: revit18

;REVIT 2019                                                                                                                                    
Source: "{#Repository}\{#RevitAppName}\bin\Release-2019\{#RevitAppName}.dll"; DestDir: "{#RevitFolder19}"; Flags: ignoreversion; Components: revit19  
Source: "{#Repository}\{#RevitAppName}\bin\Release-2019\{#RevitAppName}.addin"; DestDir: "{#RevitAddin19}"; Flags: ignoreversion; Components: revit19
Source: "{#Repository}\{#RevitAppName}\bin\Release-2019\Bcfier.dll"; DestDir: "{#RevitFolder19}"; Flags: ignoreversion; Components: revit19
Source: "{#Repository}\{#RevitAppName}\bin\Release-2019\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder19}"; Flags: ignoreversion; Components: revit19
Source: "{#Repository}\{#RevitAppName}\bin\Release-2019\RestSharp.dll"; DestDir: "{#RevitFolder19}"; Flags: ignoreversion; Components: revit19

;REVIT 2020                                                                                                                                    
Source: "{#Repository}\{#RevitAppName}\bin\Release-2020\{#RevitAppName}.dll"; DestDir: "{#RevitFolder20}"; Flags: ignoreversion; Components: revit20  
Source: "{#Repository}\{#RevitAppName}\bin\Release-2020\{#RevitAppName}.addin"; DestDir: "{#RevitAddin20}"; Flags: ignoreversion; Components: revit20
Source: "{#Repository}\{#RevitAppName}\bin\Release-2020\Bcfier.dll"; DestDir: "{#RevitFolder20}"; Flags: ignoreversion; Components: revit20
Source: "{#Repository}\{#RevitAppName}\bin\Release-2020\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder20}"; Flags: ignoreversion; Components: revit20
Source: "{#Repository}\{#RevitAppName}\bin\Release-2020\RestSharp.dll"; DestDir: "{#RevitFolder20}"; Flags: ignoreversion; Components: revit20

;REVIT 2021                                                                                                                                    
Source: "{#Repository}\{#RevitAppName}\bin\Release-2021\{#RevitAppName}.dll"; DestDir: "{#RevitFolder21}"; Flags: ignoreversion; Components: revit21  
Source: "{#Repository}\{#RevitAppName}\bin\Release-2021\{#RevitAppName}.addin"; DestDir: "{#RevitAddin21}"; Flags: ignoreversion; Components: revit21
Source: "{#Repository}\{#RevitAppName}\bin\Release-2021\Bcfier.dll"; DestDir: "{#RevitFolder21}"; Flags: ignoreversion; Components: revit21
Source: "{#Repository}\{#RevitAppName}\bin\Release-2021\GongSolutions.WPF.DragDrop.dll"; DestDir: "{#RevitFolder21}"; Flags: ignoreversion; Components: revit21
Source: "{#Repository}\{#RevitAppName}\bin\Release-2021\RestSharp.dll"; DestDir: "{#RevitFolder21}"; Flags: ignoreversion; Components: revit21



[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{userpf}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Registry]
Root: HKCU; Subkey: "Software\Classes\.bcfzip"; ValueType: string; ValueName: ""; ValueData: "{#MyAppName}"; Flags: uninsdeletevalue;  Components: standalone
Root: HKCU; Subkey: "Software\Classes\{#MyAppName}"; ValueType: string; ValueName: ""; ValueData: "BCF File"; Flags: uninsdeletekey;  Components: standalone
Root: HKCU; Subkey: "Software\Classes\{#MyAppName}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\BCF.ico"; Components: standalone
Root: HKCU; Subkey: "Software\Classes\{#MyAppName}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}""""%1"""; Components: standalone

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