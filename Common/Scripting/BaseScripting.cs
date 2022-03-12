﻿// copyright (c) 2020-2022 Roberto Ceccarelli - Casasoft
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

using Casasoft.CCDV.JSON;
using System.Reflection;

namespace Casasoft.CCDV.Scripting;

internal class BaseScripting : IScripting
{
    public IParameters Parameters { get; set; }

    public virtual string Template() => @"
/// <summary>
/// Custom class initialization
/// </summary>
private void Init() { }
";

    public virtual string WrapScript(string script, string engine) => $@"{Compiler.Usings}
namespace Casasoft.CCDV.Scripts;

public class UserScript
{{
    private {engine} engine;
    public UserScript(IEngine eng) 
    {{
        engine = ({engine})eng;
        System.Reflection.MethodInfo m = this.GetType().GetMethod(""Init"");
        if (m != null) m.Invoke(this, new object[] {{}});
    }}
{ script}
}}";
    public virtual string WrapScript(string script) => WrapScript(script, "BaseEngine");

    public virtual Assembly Compile(string script) => Compiler.Compile(WrapScript(script));
}
