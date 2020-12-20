using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.IO
{
    public static class FilenameResolver
    {
        static IFilenameResolver resolver;

        static FilenameResolver()
        {
            //if (System.Web.HttpContext.Current != null)
            //    resolver = new WebFilenameResolver();
            //else
                resolver = new BasicFilenameResolver();
        }

        public static string Resolve(string filename)
        {
            return resolver.ResolveFilename(filename);
        }
    }
}
