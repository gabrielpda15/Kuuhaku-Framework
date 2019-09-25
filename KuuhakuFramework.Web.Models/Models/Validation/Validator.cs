using KuuhakuFramework.Web.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace KuuhakuFramework.Web.Models.Validation
{
    public abstract class Validator<TAttribute, TValidationField> 
        where TAttribute : ValidationAttribute 
        where TValidationField : class, IValidationField
    {
             
        public TAttribute Attribute { get; }
        public TValidationField ValidationField { get; }

        public abstract string Name { get; }

        public abstract bool HasValidation { get; }

        protected Validator(IEnumerable<Attribute> attributes)
        {
            Attribute = (TAttribute)attributes.SingleOrDefault(x => x.GetType() == typeof(TAttribute));
        }

        public abstract bool Validate<TEntity>(PropertyInfo prop, TEntity entity, out FailedValidation failed) where TEntity : class, IEntity;

        protected FailedValidation GetFailedValidation(PropertyInfo prop)
        {
            return new FailedValidation()
            {
                Field = prop.Name,
                Validation = Name,
                Message = ValidationField.Message
            };
        }
        
    }
}
