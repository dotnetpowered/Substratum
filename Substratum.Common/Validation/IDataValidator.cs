using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public interface IDataValidator
    {
        bool Validate(object Value, IDataValidationSettings ValidationSettings);
        IDataValidationSettings CreateSettingsInstance();
    }
}
