using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public class ValidatePresence : IDataValidator
    {
        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return (Value != null && Value.ToString().Trim().Length != 0);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return null;
        }

        #endregion
    }
}
