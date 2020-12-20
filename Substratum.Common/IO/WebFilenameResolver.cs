using System;
using System.Collections.Generic;
using System.Text;

namespace Substratum.IO
{
    public class WebFilenameResolver : IFilenameResolver
    {

        #region IFilenameResolver Members

        public string ResolveFilename(string filename)
        {
            throw new NotImplementedException();
            //return System.Web.HttpContext.Current.Server.MapPath("~/"+filename);
        }

        #endregion
    }
}
