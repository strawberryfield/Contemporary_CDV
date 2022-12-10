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

using Mono.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

namespace Casasoft.CCDV.UI;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public enum Tools { Menu, MontaggioFoto, MontaggioDorsi, Scatola, Cartella, CreditCard, Cubetti, Flexagon }

    private Dictionary<Tools, string> tools = new()
    {
        { Tools.Menu, "Main menu form (default)" },
        { Tools.MontaggioFoto, "Assembly of images on 10x15 cards" },
        { Tools.MontaggioDorsi, "Assembly of photo support papers on 20x27 print" },
        { Tools.Scatola, "Box kit on 20x27 print" },
        { Tools.Cartella, "Folder kit on 20x27 print" },
        { Tools.CreditCard, "Credit card kit on 10x15 card" },
        { Tools.Cubetti, "Cubes puzzle" },
        { Tools.Flexagon, "CDV Size TetraFlexagons" }
    };

    /// <summary>
    /// Gets the name of currently running assembly
    /// </summary>
    /// <returns></returns>
    public static string ExeName => Path.GetFileNameWithoutExtension(AppDomain.CurrentDomain.FriendlyName);

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        string startupWindow = "menu";
        bool shouldShowHelp = false;
        bool shouldShowLicense = false;
        List<string> extra = new();

        var optionSet = new OptionSet
        {
            { "t|tool=", "start the specified tool",  w => startupWindow = w },
            { "h|help", "show this message and exit", h => shouldShowHelp = h != null },
            { "license", "show program license (AGPL 3.0)", h => shouldShowLicense = h != null }
        };

        try
        {
            extra = optionSet.Parse(e.Args);
        }
        catch (OptionException err)
        {
            AttachConsole(-1);
            Console.WriteLine(WelcomeBannerText);
            Console.Error.WriteLine($"{ExeName}: {err.Message}");
            Console.Error.WriteLine($"Try '{ExeName} --help' for more informations.");
            Shutdown();
        }

        if (shouldShowHelp)
        {
            AttachConsole(-1);
            Console.WriteLine(WelcomeBannerText);
            Console.WriteLine($"Usage: {ExeName} [options]");
            Console.WriteLine("\nOptions:");
            optionSet.WriteOptionDescriptions(Console.Out);
            Console.WriteLine();

            Console.WriteLine("Available tools are:");
            foreach (var item in tools)
            {
                Console.WriteLine($" - {item.Key.ToString(),-18}{item.Value}");
            }
            Console.WriteLine("Tools names are case insensitive.");
            Shutdown();
        }

        if (shouldShowLicense)
        {
            AttachConsole(-1);
            Console.WriteLine(WelcomeBannerText);
            Console.WriteLine(Casasoft.CCDV.Utils.GetLicense());
            Shutdown();
        }

        object? startTool;
        Tools startWindow = Tools.Menu;
        if (Enum.TryParse(typeof(Tools), startupWindow, true, out startTool))
        {
            if(startTool != null)
            {
                startWindow = (Tools)startTool;
            }
        }

        Window w = startWindow switch
        {
            Tools.Menu => new MainWindow(),
            Tools.MontaggioFoto => new MontaggioFotoForm(),
            Tools.MontaggioDorsi => new MontaggioDorsiForm(),
            Tools.Scatola => new BoxBuilderForm(BoxTypes.Box),
            Tools.Cartella => new BoxBuilderForm(BoxTypes.Folder),
            Tools.CreditCard => new CreditCardForm(),
            Tools.Cubetti => new CubettiForm(),
            Tools.Flexagon => new FlexagonForm(),
            _ => new MainWindow(),
        };
        w.Show();
    }

    /// <summary>
    /// Text for welcome banner
    /// </summary>
    public static string WelcomeBannerText =>
        $"\nCasasoft Contemporary Carte de Visite {ExeName}\nCopyright (c) 2020-2022 Roberto Ceccarelli - Casasoft\n";

    /// <summary>
    /// Attach console to process
    /// </summary>
    /// <param name="processId">-1 for current process</param>
    /// <returns></returns>
    [DllImport("Kernel32.dll")]
    public static extern bool AttachConsole(int processId);
}
