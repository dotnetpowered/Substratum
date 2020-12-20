using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public interface IDataValidationSettings
    {
        void Load(string Settings);
        void Save(out string Settings);
    }
}
