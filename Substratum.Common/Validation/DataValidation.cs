using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public class DataValidation
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IDataValidator Validator { get; private set; }
        public IDataValidationSettings ValidationSettings { get; set; }
        public string ErrorMessage { get; set; } //TODO: Localize??

        public DataValidation(string Name, string Description, IDataValidator Validator, IDataValidationSettings ValidationSettings, string ErrorMessage)
        {
            this.Name = Name;
            this.Description = Description;
            this.Validator = Validator;
            this.ValidationSettings = ValidationSettings;
            this.ErrorMessage = ErrorMessage;
        }

        public bool Validate(object Value)
        {
            return Validator.Validate(Value, this.ValidationSettings);
        }

    }
}
