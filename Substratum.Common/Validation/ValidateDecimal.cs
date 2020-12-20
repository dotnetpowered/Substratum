using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Substratum.Text;

namespace Substratum.Validation
{
    public class SingleValueDecimalSetting : IDataValidationSettings
    {
        public Decimal Value { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            Value = Parser.ParseDecimal(Settings);
        }

        public void Save(out string Settings)
        {
            Settings = Value.ToString();
        }

        #endregion
    }

    public class RangeDecimalSetting : IDataValidationSettings
    {
        public Decimal MinValue { get; set; }
        public Decimal MaxValue { get; set; }

        #region IDataValidationSettings Members

        public void Load(string Settings)
        {
            string[] Values = Settings.Split(';');
            MinValue = Parser.ParseDecimal(Values[0]);
            MaxValue = Parser.ParseDecimal(Values[1]);
        }

        public void Save(out string Settings)
        {
            Settings = string.Format("{0};{1}", MinValue, MaxValue);
        }

        #endregion
    }

    public abstract class ValidateDecimal : IDataValidator
    {

        protected abstract bool Compare(Decimal v, Decimal a);

        public bool Validate(object Value, SingleValueDecimalSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((Decimal)Value, ValidationSettings.Value);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (SingleValueDecimalSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new SingleValueDecimalSetting();
        }

        #endregion
    }

    public class ValidateMinDecimal : ValidateDecimal
    {
        protected override bool Compare(Decimal v, Decimal a)
        {
            return v > a;
        }
    }

    public class ValidateMaxDecimal : ValidateDecimal
    {
        protected override bool Compare(Decimal v, Decimal a)
        {
            return v < a;
        }
    }

    public class ValidateRangeDecimal : IDataValidator
    {
        protected bool Compare(Decimal v, Decimal a, Decimal b)
        {
            return (v >= a) && (v <= b);
        }

        public bool Validate(object Value, RangeDecimalSetting ValidationSettings)
        {
            if (Value == null)
                return true;
            return Compare((Decimal)Value, ValidationSettings.MinValue, ValidationSettings.MaxValue);
        }

        #region IDataValidator Members

        public bool Validate(object Value, IDataValidationSettings ValidationSettings)
        {
            return Validate(Value, (RangeDecimalSetting)ValidationSettings);
        }

        public IDataValidationSettings CreateSettingsInstance()
        {
            return new RangeDecimalSetting();
        }

        #endregion
    }

}
