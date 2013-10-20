using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Bugzilla.NET")]
[assembly: AssemblyDescription(".NET library for interacting with Bugzilla's Web Service API.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Bugzilla.NET")]
[assembly: AssemblyCopyright("Copyright © Lee Millward 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

//These assemblies are the in-memory assemblies containing the XmlRpc proxy implementations. This is required
//to allow the interface declarations to remain private to the assembly.
[assembly: InternalsVisibleTo("IBugProxy")]
[assembly: InternalsVisibleTo("IUserProxy")]
[assembly: InternalsVisibleTo("IProductProxy")]
[assembly: InternalsVisibleTo("IBugzillaProxy")]
[assembly: InternalsVisibleTo("IGroupProxy")]
[assembly: InternalsVisibleTo("IClassificationProxy")]

//This is the name given to the in-memory assembly used to hold dynamically generated types used when creating
//new or updating existing bugs to store the custom field values
[assembly: InternalsVisibleTo("BugDynamicCreateUpdateParams")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("c97cd0a7-5d38-4510-bfb1-b4f866d7e22c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
