using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.IO
{
    public class BasicFilenameResolver : IFilenameResolver
    {
        #region IFilenameResolver Members

        public string ResolveFilename(string filename)
        {
            return System.IO.Path.GetFullPath(filename);
        }

        #endregion
    }
}
