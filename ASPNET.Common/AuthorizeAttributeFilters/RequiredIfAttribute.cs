using DOTNET.Common.Reflections;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ASPNET.Common.AuthorizeAttributeFilters
{
    public class RequiredIfAttribute : ValidationAttribute
    {

        private readonly string[] PropertiesName;

        private readonly RequiredIfConditions IfConditions;


        /// <summary>
        ///     Gets or sets a flag indicating whether the attribute should allow empty strings.
        /// </summary>
        public bool AllowEmptyStrings { get; set; }
        public bool SelfValidate { get; set; }

        public RequiredIfAttribute(params string[] PropertiesName) : this(RequiredIfConditions.PropertiesHaveValues_And, PropertiesName)
        {

        }

        public RequiredIfAttribute(RequiredIfConditions conditions, params string[] PropertiesName) : this(conditions, string.Empty, PropertiesName)
        {

        }


        public RequiredIfAttribute(RequiredIfConditions conditions, string errorMessage, params string[] PropertiesName) : base(errorMessage)
        {
            this.PropertiesName = PropertiesName;
            this.IfConditions = conditions;
            //base.RequiresValidationContext = true;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            #region Search

            //Was there a null attribute?
            bool FoundNull = false;
            //Was there a non-null property?
            bool FoundNonNull = false;
            foreach (var propertyName in PropertiesName)
            {
                if (ReflectionHelper.GetValue(validationContext.ObjectInstance, propertyName) == null)
                {
                    FoundNull = true;
                }
                else
                {
                    FoundNonNull = AllowEmptyStrings || value is not string stringValue || !string.IsNullOrWhiteSpace(stringValue);
                }
            }

            #endregion

            #region Validations

            bool IsValid = false;
            bool IsNot = false;


            if (IfConditions == RequiredIfConditions.PropertiesHaveValues_And || IfConditions == RequiredIfConditions.PropertiesHaveValues_Not_And)
            {
                IsNot = IfConditions == RequiredIfConditions.PropertiesHaveValues_Not_And;


                if (!FoundNull && FoundNonNull)
                    IsValid = true;
                else
                    IsValid = false;




            }
            else if (IfConditions == RequiredIfConditions.PropertiesHaveValues_Or || IfConditions == RequiredIfConditions.PropertiesHaveValues_Not_Or)
            {
                IsNot = IfConditions == RequiredIfConditions.PropertiesHaveValues_Not_Or;

                if (FoundNonNull)
                    IsValid = true;
                else
                    IsValid = false;

            }
            else if (IfConditions == RequiredIfConditions.PropertiesNotHaveValues_And || IfConditions == RequiredIfConditions.PropertiesNotHaveValues_Not_And)
            {
                IsNot = IfConditions == RequiredIfConditions.PropertiesNotHaveValues_Not_And;

                if (FoundNull && !FoundNonNull)
                    IsValid = true;
                else
                    IsValid = false;

            }
            else if (IfConditions == RequiredIfConditions.PropertiesNotHaveValues_Or || IfConditions == RequiredIfConditions.PropertiesNotHaveValues_Not_Or)
            {
                IsNot = IfConditions == RequiredIfConditions.PropertiesNotHaveValues_Not_Or;

                if (FoundNull)
                    IsValid = true;
                else
                    IsValid = false;
            }


            #endregion

            #region Not

            if (IsNot)
                IsValid = !IsValid;


            #endregion

            #region Result

            bool SelfIsValid = true;

            if (SelfValidate) 
            {
                RequiredAttribute requiredAttribute = new RequiredAttribute() { AllowEmptyStrings = this.AllowEmptyStrings };
                SelfIsValid = requiredAttribute.IsValid(value);
            }

            if (IsValid && SelfIsValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(base.ErrorMessage, PropertiesName);
            }

            #endregion
        }
    }

    public enum RequiredIfConditions
    {
        PropertiesHaveValues_And,
        PropertiesHaveValues_Not_And,

        PropertiesHaveValues_Or,
        PropertiesHaveValues_Not_Or,

        PropertiesNotHaveValues_And,
        PropertiesNotHaveValues_Not_And,


        PropertiesNotHaveValues_Or,
        PropertiesNotHaveValues_Not_Or,

    }
}
