using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace KuuhakuFramework.Web.Models.Validation.Validators
{
    public class StringLengthValidator : Validator<StringLengthAttribute, StringLengthValidator.Field>
    {
        public override string Name => "StringLength";
        public override bool HasValidation => ValidationField.Type != Field.StringLengthType.None;

        public class Field : IValidationField
        {
            public enum StringLengthType { None, OnlyMax, MaxAndMin }
            public StringLengthType Type { get; set; }
            public int? Max { get; set; }
            public int? Min { get; set; }
            public string Message { get; set; }
        }

        public StringLengthValidator(IEnumerable<Attribute> attributes) : base(attributes) 
        {
            ValidationField.Type = Attribute == null ? Field.StringLengthType.None : (Attribute.MinimumLength == 0 ? Field.StringLengthType.OnlyMax : Field.StringLengthType.MaxAndMin);
            ValidationField.Max = Attribute?.MaximumLength;
            ValidationField.Min = Attribute?.MinimumLength;
            ValidationField.Message = Attribute?.ErrorMessage;
        }

        public override bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed)
        {
            if (prop.GetValue(entity) is string value)
            {
                switch (ValidationField.Type)
                {
                    case Field.StringLengthType.MaxAndMin:
                        if (!(value.Length >= ValidationField.Min && value.Length <= ValidationField.Max))
                        {
                            failed = GetFailedValidation(prop);
                            return false;
                        }
                        break;
                    case Field.StringLengthType.OnlyMax:
                        if (value.Length > ValidationField.Max)
                        {
                            failed = GetFailedValidation(prop);
                            return false;
                        }
                        break;
                }
            }

            failed = null;
            return true;
        }
    }
}
