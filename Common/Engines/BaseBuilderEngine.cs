using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casasoft.CCDV.Engines;

public class BaseBuilderEngine : BaseEngine
{
    public BaseBuilderEngine()
    {
    }

    public BaseBuilderEngine(ICommandLine par) : base(par)
    {
    }

    public override MagickImage GetResult(bool quiet)
    {
        return base.GetResult(quiet);
    }

    protected IBuilder builder;
    protected ICommandLine par;
}
