// Sample script for MontaggioFoto

public MagickImage ProcessOnLoad(MagickImage image)
{
    string filename = image.FileName;
    return Utils.RotateResizeAndFill(image, engine.fmt.CDV_Internal_v, engine.FillColor);
}