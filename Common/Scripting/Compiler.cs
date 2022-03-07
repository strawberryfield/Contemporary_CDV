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
        MetadataReference[] references = new MetadataReference[]
        {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Runtime.dll")),
                MetadataReference.CreateFromFile(FromTrustedPlatformAssembly("System.Console.dll"))
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
    public static void Run(Assembly assembly, string ClassName, string Method, object[] args)
    {
        // create instance of the desired class and call the desired function
        Type type = assembly.GetType(ClassName);
        object obj = Activator.CreateInstance(type);
        type.InvokeMember(Method,
            BindingFlags.Default | BindingFlags.InvokeMethod,
            null,
            obj,
            args);
    }

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
}
