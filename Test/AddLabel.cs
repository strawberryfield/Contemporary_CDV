// Sample script for MontaggioFoto

public void ProcessOnLoad(MagickImage image)
{
    string filename = image.FileName;
    image = Utils.RotateResizeAndFill(image, engine.fmt.CDV_Internal_v, engine.FillColor);
}