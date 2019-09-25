using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace KuuhakuFramework.Web.Models.Validation.Validators
{
    public class PhoneValidator : Validator<PhoneAttribute, PhoneValidator.Field>
    {
        public static readonly Regex PHONE_REGEX = new Regex(@"^[+]{1}[0-9]{1,3} \(?[0-9]{1,4}\)? [-\s\./0-9]{3,}$");

        public override string Name => "PhoneNumber";

        public override bool HasValidation => ValidationField.IsPhoneNumber;

        public class Field : IValidationField
        {
            public bool IsPhoneNumber { get; set; }
            public string Message { get; set; }
        }

        public PhoneValidator(IEnumerable<Attribute> attributes) : base(attributes)
        {
            ValidationField.IsPhoneNumber = Attribute == null ? false : true;
            ValidationField.Message = Attribute?.ErrorMessage;
        }

        public override bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed)
        {
            if (ValidationField.IsPhoneNumber && prop.GetValue(entity) is string phone)
            {
                if (!PHONE_REGEX.IsMatch(phone))
                {
                    failed = GetFailedValidation(prop);
                    return false;
                }
            }

            failed = null;
            return true;
        }
    }
}
