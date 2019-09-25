using KuuhakuFramework.Web.Models.Attributes;
using KuuhakuFramework.Web.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace KuuhakuFramework.Web.Models.Validation.Validators
{
    public class CpfValidator : Validator<CPFAttribute, CpfCnpjValidator.Field>
    {
        public override string Name => "CPF";

        public override bool HasValidation => ValidationField.Type == CpfCnpjValidator.Field.CpfCnpjType.CPF;

        public CpfValidator(IEnumerable<Attribute> attributes) : base(attributes)
        {
            CpfCnpjValidator.SetField(ValidationField, Attribute);
        }

        public override bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed)
        {
            failed = CpfCnpjValidator.Validate(ValidationField.Type, prop, entity, GetFailedValidation(prop));
            return failed == null ? true : false;
        }
    }

    public class CnpjValidator : Validator<CNPJAttribute, CpfCnpjValidator.Field>
    {
        public override string Name => "CNPJ";

        public override bool HasValidation => ValidationField.Type == CpfCnpjValidator.Field.CpfCnpjType.CNPJ;

        public CnpjValidator(IEnumerable<Attribute> attributes) : base(attributes)
        {
            CpfCnpjValidator.SetField(ValidationField, Attribute);
        }

        public override bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed)
        {
            failed = CpfCnpjValidator.Validate(ValidationField.Type, prop, entity, GetFailedValidation(prop));
            return failed == null ? true : false;
        }
    }

    public static class CpfCnpjValidator
    {

        public static readonly Regex CPF_CNPJ_REGEX = new Regex(@"^([0-9]{2}[\.][0-9]{3}[\.][0-9]{3}[\/][0-9]{4}[-][0-9]{2})|([0-9]{3}[\.][0-9]{3}[\.][0-9]{3}[-][0-9]{2})$");

        public static FailedValidation Validate<TEntity>(Field.CpfCnpjType type, PropertyInfo prop, TEntity entity, FailedValidation failed) where TEntity : class, IEntity
        {
            if (type != Field.CpfCnpjType.None && prop.GetValue(entity) is string cpfcnpj)
            {
                var valid = true;

                do
                {
                    if (!CPF_CNPJ_REGEX.IsMatch(cpfcnpj)) { valid = false; break; }

                    if (type == Field.CpfCnpjType.CPF && !IsValidCPF(cpfcnpj)) valid = false;
                    else if (!IsValidCNPJ(cpfcnpj)) valid = false;

                } while (false);

                if (!valid) return failed;
            }

            return null;
        }

        public static void SetField(Field field, ValidationAttribute attribute)
        {
            field.Type = attribute == null ? Field.CpfCnpjType.None : (attribute is CPFAttribute ? Field.CpfCnpjType.CPF : Field.CpfCnpjType.CNPJ);
            field.Message = attribute?.ErrorMessage;
        }

        public class Field : IValidationField
        {
            public enum CpfCnpjType { None, CPF, CNPJ }
            public CpfCnpjType Type { get; set; }
            public string Message { get; set; }
        }

        public static bool IsValidCPF(string cpf)
        {
            var valueValidLength = 11;
            var multipliersForFirstDigit = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multipliersForSecondDigit = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            return IsValid(cpf.Replace(".", "").Replace("-", ""), valueValidLength, multipliersForFirstDigit, multipliersForSecondDigit);
        }
        public static bool IsValidCNPJ(string cpnj)
        {
            var valueValidLength = 14;
            var multipliersForFirstDigit = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multipliersForSecondDigit = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            return IsValid(cpnj.Replace(".", "").Replace("-", "").Replace("/", ""), valueValidLength, multipliersForFirstDigit, multipliersForSecondDigit);
        }

        private static bool IsValid(
        string value,
        int valueValidLength,
        int[] multipliersForFirstDigit,
        int[] multipliersForSecondDigit)
        {
            var isInvalid =
                IsInvalidLength(value, valueValidLength) ||
                IsNotNumbersOnly(value) ||
                IsInvalidMod11(multipliersForFirstDigit, multipliersForSecondDigit, value);

            return !isInvalid;
        }

        private static bool IsInvalidLength(string value, int valueValidLength)
        {
            return value.Length != valueValidLength;
        }

        private static bool IsNotNumbersOnly(string value)
        {
            return !Regex.IsMatch(value, @"\d+");
        }
        private static bool IsInvalidMod11(int[] multipliersForFirstDigit, int[] multipliersForSecondDigit, string value)
        {
            var firstDigit = GetFirstDigit(multipliersForFirstDigit, value);
            var secondDigit = GetSecondDigit(multipliersForSecondDigit, value, firstDigit);
            var expectedSufix = string.Concat(firstDigit, secondDigit);
            var isInvalid = !value.EndsWith(expectedSufix);
            return isInvalid;
        }

        private static int GetFirstDigit(int[] multipliers, string value)
        {
            var valueToWork = value.Substring(0, multipliers.Length);
            var sum = multipliers
                .Select((d, i) => new
                {
                    Value = int.Parse(valueToWork[i].ToString()),
                    Multiplier = multipliers[i]
                })
                .Sum(d => d.Value * d.Multiplier);
            var rest = sum % 11;
            var firstDigit = rest < 2 ? 0 : 11 - rest;
            return firstDigit;
        }

        private static int GetSecondDigit(int[] multipliers, string value, int firstDigit)
        {
            var valueToWork = string.Concat(value.Substring(0, multipliers.Length - 1), firstDigit);
            var sum = multipliers
                .Select((d, i) => new
                {
                    Value = int.Parse(valueToWork[i].ToString()),
                    Multipler = d
                })
                .Sum(d => d.Value * d.Multipler);
            var rest = sum % 11;
            var secondDigit = rest < 2 ? 0 : 11 - rest;
            return secondDigit;
        }
    }
}
