using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Substratum.Text;

namespace Substratum.Validation
{
    public class SingleValueIntSetting : IDataValidationSettings
    {
        public int Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = Parser.ParseInt(Settings);
        }

        public void Save(out string Settings)
        {
            Settings = Value.ToString();
        }

        #endregion
    }

    public class RangeIntSetting : IDataValidationSettings
    {
        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            string[] Values = Settings.Split(';');
            MinValue = Parser.ParseInt(Values[0]);
            MaxValue = Parser.ParseInt(Values[1]);
        }

        public void Save(out string Settings)
        {
            Settings = string.Format("{0};{1}", MinValue, MaxValue);
        }

        #endregion
    }

    public abstract class ValidateInt : IDataValidator
    {

        protected abstract bool Compare(int v, int a);

        public bool Validate(object Value, SingleValueIntSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((int)Value, ValidationSettings.Value);
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

    public class ValidateMinInt : ValidateInt
    {
        protected override bool Compare(int v, int a)
        {
            return v > a;
        }
    }

    public class ValidateMaxInt : ValidateInt
    {
        protected override bool Compare(int v, int a)
        {
            return v < a;
        }
    }

    public class ValidateRangeInt : IDataValidator
    {
        protected bool Compare(int v, int a, int b)
        {
            return (v >= a) && (v <= b);
        }

        public bool Validate(object Value, RangeIntSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((int)Value, ValidationSettings.MinValue, ValidationSettings.MaxValue);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (RangeIntSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new RangeIntSetting();
        }

        #endregion
    }

}
