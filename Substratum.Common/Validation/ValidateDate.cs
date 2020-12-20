using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    //TODO: Add ValidateDate & ValidateTime
    //TODO: Use Parser.ParseDateTime

    public class SingleValueDateTimeSetting : IDataValidationSettings
    {
        public DateTime Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = DateTime.Parse(Settings);
        }

        public void Save(out string Settings)
        {
            Settings = Value.ToString();
        }

        #endregion
    }

    public class RangeDateTimeSetting : IDataValidationSettings
    {
        public DateTime MinValue { get; set; }
        public DateTime MaxValue { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            string[] Values = Settings.Split(';');
            MinValue = DateTime.Parse(Values[0]);
            MaxValue = DateTime.Parse(Values[1]);
        }

        public void Save(out string Settings)
        {
            Settings = string.Format("{0};{1}", MinValue, MaxValue);
        }

        #endregion
    }

    public class ValidateDateTimeMax : IDataValidator
    {
        public bool Validate(object Value, SingleValueDateTimeSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            return (DateTime) Value <= ValidationSettings.Value;
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueDateTimeSetting();
        }

        #endregion
    }

    public class ValidateDateTimeMin : IDataValidator
    {
        public bool Validate(object Value, SingleValueDateTimeSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            return (DateTime)Value >= ValidationSettings.Value;
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueDateTimeSetting();
        }

        #endregion
    }

    public class ValidateDateTimeRange : IDataValidator
    {
        public bool Validate(object Value, RangeDateTimeSetting ValidationSettings)
        {
            if (Value == null)
                return false;
            return (DateTime)Value >= ValidationSettings.MinValue &&
                (DateTime)Value <= ValidationSettings.MaxValue;
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (RangeDateTimeSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new RangeDateTimeSetting();
        }

        #endregion
    }

}
