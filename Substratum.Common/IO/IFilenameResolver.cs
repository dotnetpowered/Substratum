using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.IO
{
    public interface IFilenameResolver
    {
        string ResolveFilename(string filename);
    }
}
