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

MontaggioFotoEngine eng = new();
UserScript user = new(eng);

user.ProcessOnLoad(new MagickImage());

public class UserScript
{
    private MontaggioFotoEngine engine;
	
    public UserScript(IEngine eng) 
    {
        engine = (MontaggioFotoEngine)eng;
        System.Reflection.MethodInfo m = this.GetType().GetMethod("Init");
        if (m != null) m.Invoke(this, null);
    }

    // vvv --- put you script here --- vvv

    /// <summary>
    /// Preprocesses the loaded image
    /// </summary>
    /// <param name=""image"">The loaded image</param>
    /// <returns>The Processed image</returns>
    public MagickImage ProcessOnLoad(MagickImage image) => image;

    // ^^^ --- script ends here --- ^^^

}