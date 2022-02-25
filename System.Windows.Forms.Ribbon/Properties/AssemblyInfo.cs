using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if !NetCore
[assembly: AssemblyTitle("System.Windows.Forms.Ribbon")]
#endif
[assembly: AssemblyDescription("Ribbon Control for .NET WinForms")]
#if !NetCore
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("https://github.com/RibbonWinForms/RibbonWinForms")]
[assembly: AssemblyProduct("System.Windows.Forms.Ribbon")]
[assembly: AssemblyCopyright("https://github.com/RibbonWinForms/RibbonWinForms")]
#endif
[assembly: AssemblyTrademark("https://github.com/RibbonWinForms/RibbonWinForms")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("15499B85-9C8F-452d-947F-A0EC6E92AF53")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
// For each .NET version we need a unique AssemblyVersion. This version only change
// in case of a new interface for users
// Only AssemblyFileVersion had to change for bugfixes
#if !NetCore
#if NET2
[assembly: AssemblyVersion("2.0.0.0")]
#else
[assembly: AssemblyVersion("4.0.0.0")]
#endif
[assembly: AssemblyFileVersion("5.0.1.0")]
#endif