// Sample script for MontaggioFoto

public MagickImage ProcessOnLoad(MagickImage image)
{
    // Get the image name
    string filename = Path.GetFileNameWithoutExtension(image.FileName);

    // Resize the image and put aligned to top
    MagickImage img1 = Utils.RotateResizeAndFill(image, engine.fmt.CDV_Internal_v, engine.FillColor);
    img1.Trim();
    image = engine.img.CDV_Full_v();
    image.Composite(img1, Gravity.North, new PointD(0, engine.fmt.ToPixels(5)));

    // Format and overlay image name
    MagickReadSettings settings = new();
    settings.BackgroundColor = MagickColors.Transparent;
    MagickImage text = new(@$"pango:<span size='50000'>{filename}</span>", settings);
    image.Composite(text, Gravity.South, new PointD(0, engine.fmt.ToPixels(5)), CompositeOperator.Over);

    return image;
}