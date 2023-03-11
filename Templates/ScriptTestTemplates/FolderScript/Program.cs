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

FolderEngine eng = new();
UserScript user = new(eng);

user.ProcessOnLoad(new MagickImage());

public class UserScript
{
    private FolderEngine engine;

    public UserScript(IEngine eng)
    {
        engine = (FolderEngine)eng;
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

	/// <summary> 
	/// Preprocesses the loaded image for Front
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadFront(MagickImage image) => image;

	/// <summary> 
	/// Preprocesses the loaded image for Back
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadBack(MagickImage image) => image;

	/// <summary> 
	/// Preprocesses the loaded image for Top
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadTop(MagickImage image) => image;

	/// <summary> 
	/// Preprocesses the loaded image for Bottom
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadBottom(MagickImage image) => image;

	/// <summary> 
	/// Preprocesses the loaded image for Left
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadLeft(MagickImage image) => image;

	/// <summary> 
	/// Preprocesses the loaded image for Right
	/// </summary>
	/// <param name="image">The loaded image</param>
	/// <returns>The Processed image</returns>
	public MagickImage ProcessOnLoadRight(MagickImage image) => image;

    // ^^^ --- script ends here --- ^^^

}