// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
// http://strawberryfield.altervista.org 
// 
// This file is part of Casasoft Contemporary Carte de Visite Tools
// https://github.com/strawberryfield/Contemporary_CDV
// 
// Casasoft CCDV Tools is free software: 
// you can redistribute it and/or modify it
// under the terms of the GNU Affero General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Casasoft CCDV Tools is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See the GNU General Public License for more details.
// 
// You should have received a copy of the GNU AGPL v.3
// along with Casasoft CCDV Tools.  
// If not, see <http://www.gnu.org/licenses/>.

using Casasoft.CCDV.Engines;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Casasoft.CCDV.Scripting;

/// <summary>
/// c# scripts compiler
/// </summary>
public static class Compiler
{
    /// <summary>
    /// Compile the source code to a memory assembly
    /// </summary>
    /// <param name="sourceCode"></param>
    /// <returns>null if any compilation error</returns>
    public static Assembly Compile(string sourceCode)
    {
        MemoryStream ms = new();

        // define other necessary objects for compilation
        string assemblyName = Path.GetRandomFileName();

        string ExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string WorkPath = System.IO.Path.GetDirectoryName(ExeFilePath);

        MetadataReference[] references = new MetadataReference[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Linq.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Linq.Expressions.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Linq.Queryable.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Core.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Runtime.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Runtime.Loader.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Console.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Collections.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Collections.Concurrent.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Memory.dll")),
            MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("netstandard.dll")),
            MetadataReference.CreateFromFile(Path.Combine(WorkPath, "Magick.NET-Q16-AnyCPU.dll")),
            MetadataReference.CreateFromFile(Path.Combine(WorkPath, "Magick.NET.Core.dll")),
            MetadataReference.CreateFromFile(Path.Combine(WorkPath, "Casasoft.CCDV.Common.dll"))
        };

        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);
        // analyse and generate IL code from syntax tree
        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        // write IL code into memory
        EmitResult result = compilation.Emit(ms);

        if (!result.Success)
        {
            // handle exceptions
            IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                diagnostic.IsWarningAsError ||
                diagnostic.Severity == DiagnosticSeverity.Error);

            foreach (Diagnostic diagnostic in failures)
            {
                Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
            }
            return null;
        }

        // load this 'virtual' DLL so that we can use
        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }

    /// <summary>
    /// Execute the code
    /// </summary>
    /// <param name="assembly">Memory assembly with compiled code</param>
    /// <param name="ClassName">Fully qualified class name</param>
    /// <param name="Method">Method to execute</param>
    /// <param name="args">Array of arguments</param>
    /// <param name="eng">Engine passed to constructor</param>
    public static object Run(Assembly assembly, string ClassName, string Method, object[] args, IEngine eng)
    {
        // create instance of the desired class and call the desired function
        Type type = assembly.GetType(ClassName);
        if (type != null && type.GetMethod(Method) != null)
        {
            object obj = Activator.CreateInstance(type, new object[] { eng });
            return type.InvokeMember(Method,
                BindingFlags.Default | BindingFlags.InvokeMethod,
                null,
                obj,
                args);
        }
        else return null;
    }

    /// <summary>
    /// Executes a method of an istanced object
    /// </summary>
    /// <param name="obj">instance</param>
    /// <param name="Method">Method to execute</param>
    /// <param name="args">Array of arguments</param>
    /// <returns></returns>
    public static object Run(object obj, string Method, object[] args)
    {
        Type type = obj.GetType();
        if (type != null && type.GetMethod(Method) != null)
        {

            return type.InvokeMember(Method,
            BindingFlags.Default | BindingFlags.InvokeMethod,
            null,
            obj,
            args);
        }
        else
            return null;
    }

    /// <summary>
    /// Execute the code in the Casasoft.CCDV.Scripts.UserScript class
    /// </summary>
    /// <param name="assembly">Memory assembly with compiled code</param>
    /// <param name="Method">Method to execute</param>
    /// <param name="args">Array of arguments</param>
    /// <param name="eng">Engine passed to constructor</param>
    public static object Run(Assembly assembly, string Method, object[] args, IEngine eng) =>
        Run(assembly, "Casasoft.CCDV.Scripts.UserScript", Method, args, eng);

    /// <summary>
    /// Creates an istance of the class
    /// </summary>
    /// <param name="assembly">Memory assembly with compiled code</param>
    /// <param name="ClassName">Fully qualified class name</param>
    /// <param name="eng">Engine passed to constructor</param>
    /// <returns></returns>
    public static object New(Assembly assembly, string ClassName, IEngine eng)
    {
        Type type = assembly.GetType(ClassName);
        if (type != null)
        {
            return Activator.CreateInstance(type, new object[] { eng });
        }
        else return null;
    }

    /// <summary>
    /// Creates an istance of the Casasoft.CCDV.Scripts.UserScript class
    /// </summary>
    /// <param name="assembly">Memory assembly with compiled code</param>
    /// <param name="eng">Engine passed to constructor</param>
    /// <returns></returns>
    public static object New(Assembly assembly, IEngine eng) =>
        New(assembly, "Casasoft.CCDV.Scripts.UserScript", eng);

    /// <summary>
    /// Gets the dll complete path
    /// </summary>
    /// <param name="shortDllName"></param>
    /// <returns></returns>
    public static string FromTrustedPlatformAssembly(string shortDllName)
    {
        string dllString = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString();
        var dlls = dllString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        return dlls.Single(d => d.Contains(shortDllName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Using declarations to be included in scripts
    /// </summary>
    public static string Usings => @"
using Casasoft.CCDV;
using Casasoft.CCDV.Engines;
using Casasoft.CCDV.JSON;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
";


}
