using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Substratum.Validation
{
    public static class ValidatorFactory
    {
        static Dictionary<string, IDataValidator> ValidatorCache = new Dictionary<string, IDataValidator>();

        public static IDataValidator GetValidator(string ValidatorClassName)
        {
            lock (ValidatorCache)
            {
                IDataValidator validator;
                if (!ValidatorCache.TryGetValue(ValidatorClassName, out validator))
                {
                    Type type = Type.GetType(ValidatorClassName);
                    validator = (IDataValidator)Activator.CreateInstance(type);
                    ValidatorCache.Add(ValidatorClassName, validator);
                }
                return validator;
            }
        }
    }
}
