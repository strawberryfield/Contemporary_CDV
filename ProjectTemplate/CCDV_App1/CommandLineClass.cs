﻿using Casasoft.CCDV;

namespace CCDV_App1;

internal class CommandLineClass : CommandLine
{
    public CommandLineClass(string outputname, string desc = "") : base(outputname, desc)
    {
    }

    public CommandLineClass(string exename, string outputname, string desc = "") : base(exename, outputname, desc)
    {
    }
}
