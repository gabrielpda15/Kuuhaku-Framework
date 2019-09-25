using KuuhakuFramework.Web.Models.Attributes;
using KuuhakuFramework.Web.Models.BaseEntities;
using KuuhakuFramework.Web.Models.Validation.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace KuuhakuFramework.Web.Models.Validation
{
    public sealed class Validation<TEntity> where TEntity : class, IEntity
    {
        private IDictionary<PropertyInfo, object> Fields { get; }
        
        private string GetFieldDisplayName<TOutput>(Expression<Func<TEntity, TOutput>> selector)
        {
            if (selector?.Body == null)
            {
                throw new ArgumentException("Expression cannot be null");
            }

            if (selector.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            if (selector.Body is MethodCallExpression methodCallExpression)
            {
                return methodCallExpression.Method.Name;
            }
            
            if (selector.Body is UnaryExpression unaryExpression)
            {
                if (unaryExpression.Operand is MethodCallExpression)
                {
                    var methodExpression = (MethodCallExpression)unaryExpression.Operand;

                    return methodExpression.Method.Name;
                }

                return ((MemberExpression)unaryExpression.Operand).Member.Name;
            }

            throw new ArgumentException("Invalid Expression");
        }

        public class ValidationException : Exception
        {
            public IEnumerable<FailedValidation> FailedValidations { get; }

            public ValidationException(IEnumerable<FailedValidation> faileds) : base("Uma ou muitas validações foram falhas")
            {
                FailedValidations = faileds;
            }
        }

        public Validation() 
        {
            Fields = new Dictionary<PropertyInfo, object>();

            foreach (var prop in typeof(TEntity).GetProperties())
            {
                var attribs = prop.GetCustomAttributes();

                var StringLength = new StringLengthValidator(attribs);
                var Required = new RequiredValidator(attribs);
                var PhoneNumber = new PhoneValidator(attribs);
                var CPF = new CpfValidator(attribs);
                var CNPJ = new CnpjValidator(attribs);             

                Fields.Add(prop, new
                {
                    HasValidation = StringLength.HasValidation || Required.HasValidation || PhoneNumber.HasValidation || CPF.HasValidation || CNPJ.HasValidation,
                    StringLength,
                    Required,
                    PhoneNumber,
                    CPF,
                    CNPJ
                });
            }
        }

        public string GetDisplayName(PropertyInfo prop)
        {
            var display = (DisplayAttribute)prop.GetCustomAttributes().SingleOrDefault(x => x.GetType() == typeof(DisplayAttribute));
            return display == null ? prop.Name : display.Name;
        }

        public void Validate(TEntity entity)
        {
            var failedValidations = new List<FailedValidation>();

            foreach (var prop in typeof(TEntity).GetProperties())
            {
                dynamic validators = Fields[prop];

                if (validators.HasValidation)
                {
                    foreach (dynamic validation in ((Type)validators.GetType()).GetProperties().Select(x => x.GetValue(validators)))
                    {
                        if (validation.HasValidation)
                        {
                            if (validation.Validate(prop, entity, out FailedValidation failed))
                            {
                                failedValidations.Add(failed);
                            }
                        }
                    }
                }
            }

            if (failedValidations.Count > 0)
            {
                throw new ValidationException(failedValidations);
            }
        }
    }
}
