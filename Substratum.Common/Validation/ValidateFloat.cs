using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public class SingleValueFloatSetting : IDataValidationSettings
    {
        public float Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = float.Parse(Settings);
        }

        public void Save(out string Settings)
        {
            Settings = Value.ToString();
        }

        #endregion
    }

    public class RangeFloatSetting : IDataValidationSettings
    {
        public float MinValue { get; set; }
        public float MaxValue { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            string[] Values = Settings.Split(';');
            MinValue = float.Parse(Values[0]);
            MaxValue = float.Parse(Values[1]);
        }

        public void Save(out string Settings)
        {
            Settings = string.Format("{0};{1}", MinValue, MaxValue);
        }

        #endregion
    }

    public abstract class ValidateFloat : IDataValidator
    {

        protected abstract bool Compare(float v, float a);

        public bool Validate(object Value, SingleValueFloatSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((float)Value, ValidationSettings.Value);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueFloatSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueFloatSetting();
        }

        #endregion
    }

    public class ValidateMinFloat : ValidateFloat
    {
        protected override bool Compare(float v, float a)
        {
            return v > a;
        }
    }

    public class ValidateMaxFloat : ValidateFloat
    {
        protected override bool Compare(float v, float a)
        {
            return v < a;
        }
    }

    public class ValidateRangeFloat : IDataValidator
    {
        protected bool Compare(float v, float a, float b)
        {
            return (v >= a) && (v <= b);
        }

        public bool Validate(object Value, RangeFloatSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((float)Value, ValidationSettings.MinValue, ValidationSettings.MaxValue);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (RangeFloatSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new RangeFloatSetting();
        }

        #endregion
    }

}
