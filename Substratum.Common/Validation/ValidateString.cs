using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public class SingleValueStringSetting : IDataValidationSettings
    {
        public string Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = Settings;
        }

        public void Save(out string Settings)
        {
            Settings = Value;
        }

        #endregion
    }

    public class ValidateStringMaxLength : IDataValidator
    {
        public bool Validate(object Value, SingleValueIntSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            return Value.ToString().Length <= ValidationSettings.Value;
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueIntSetting();
        }

        #endregion
    }

    public class ValidateStringMinLength : IDataValidator
    {
        public bool Validate(object Value, SingleValueIntSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            return Value.ToString().Length >= ValidationSettings.Value;
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueIntSetting();
        }

        #endregion
    }

    public class ValidateStringRangeLength : IDataValidator
    {
        public bool Validate(object Value, RangeIntSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            string s = Value.ToString();
            return (s.Length >= ValidationSettings.MinValue && s.Length <= ValidationSettings.MinValue);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueIntSetting();
        }

        #endregion
    }

    public class ValidateStringPattern : IDataValidator
    {
        public bool Validate(object Value, SingleValueStringSetting ValidationSettings)
        {
            if (Value != null)
                return false;

            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(ValidationSettings.Value);
            return regEx.IsMatch(Value.ToString());
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueStringSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueStringSetting();
        }

        #endregion
    }

}
