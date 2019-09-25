using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace KuuhakuFramework.Web.Models.Validation.Validators
{
    public class RequiredValidator : Validator<RequiredAttribute, RequiredValidator.Field>
    {
        public override string Name => "Required";

        public override bool HasValidation => ValidationField.IsRequired;

        public class Field : IValidationField
        {
            public bool IsRequired { get; set; }
            public string Message { get; set; }
        }

        public RequiredValidator(IEnumerable<Attribute> attributes) : base(attributes)
        {
            ValidationField.IsRequired = Attribute == null ? false : true;
            ValidationField.Message = Attribute?.ErrorMessage;
        }

        public override bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed)
        {
            if (ValidationField.IsRequired && (prop.GetValue(entity) == null || IsNothing(prop.GetValue(entity).ToString())))
            {
                failed = GetFailedValidation(prop);
                return false;
            }

            failed = null;
            return true; 
        }

        private bool IsNothing(string str)
        {
            return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
        }
    }
}
