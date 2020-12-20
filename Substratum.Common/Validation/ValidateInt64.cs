using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public class SingleValueInt64Setting : IDataValidationSettings
    {
        public Int64 Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = Int64.Parse(Settings);
        }

        public void Save(out string Settings)
        {
            Settings = Value.ToString();
        }

        #endregion
    }

    public class RangeInt64Setting : IDataValidationSettings
    {
        public Int64 MinValue { get; set; }
        public Int64 MaxValue { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            string[] Values = Settings.Split(';');
            MinValue = Int64.Parse(Values[0]);
            MaxValue = Int64.Parse(Values[1]);
        }

        public void Save(out string Settings)
        {
            Settings = string.Format("{0};{1}", MinValue, MaxValue);
        }

        #endregion
    }
     
    public abstract class ValidateInt64 : IDataValidator
    {

        protected abstract bool Compare(Int64 v, Int64 a);

        public bool Validate(object Value, SingleValueInt64Setting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((Int64)Value, ValidationSettings.Value);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueInt64Setting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueInt64Setting();
        }

        #endregion
    }

    public class ValidateMinInt64 : ValidateInt64
    {
        protected override bool Compare(Int64 v, Int64 a)
        {
            return v > a;
        }
    }

    public class ValidateMaxInt64 : ValidateInt64
    {
        protected override bool Compare(Int64 v, Int64 a)
        {
            return v < a;
        }
    }

    public class ValidateRangeInt64 : IDataValidator
    {
        protected bool Compare(Int64 v, Int64 a, Int64 b)
        {
            return (v >= a) && (v <= b);
        }

        public bool Validate(object Value, RangeInt64Setting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((Int64)Value, ValidationSettings.MinValue, ValidationSettings.MaxValue);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (RangeInt64Setting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new RangeInt64Setting();
        }

        #endregion
    }

}
