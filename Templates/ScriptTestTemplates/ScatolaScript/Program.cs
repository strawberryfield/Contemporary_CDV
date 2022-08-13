// Sample template for Casasoft Contemporary Carte de Visite Script
// See https://strawberryfield.altervista.org/carte_de_visite for more information

// These are the usings included when compiling the script at run-time
using Casasoft.CCDV;
using Casasoft.CCDV.Engines;
using Casasoft.CCDV.JSON;
using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

ScatolaEngine eng = new();
UserScript user = new(eng);

user.ProcessOnLoad(new MagickImage());

public class UserScript
{
    private ScatolaEngine engine;

    public UserScript(IEngine eng)
    {
        engine = (ScatolaEngine)eng;
        System.Reflection.MethodInfo m = this.GetType().GetMethod("Init");
        if (m is not null) m.Invoke(this, null);
    }

    // vvv --- put you script here --- vvv

    /// <summary>
    /// Custom class initialization
    /// </summary>
    public void Init() { }

    /// <summary>
    /// Image for final output
    /// </summary>
    /// <returns></returns>
    public MagickImage OutputImage() => null;

    // ^^^ --- script ends here --- ^^^

}