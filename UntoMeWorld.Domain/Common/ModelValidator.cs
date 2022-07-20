using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UntoMeWorld.Domain.Common
{
    public class ModelValidationResult
    {
        public bool IsValid { get; set; }
        public List<ValidationResult> Results { get; set; }

        public override string ToString()
        {
            return $"{nameof(IsValid)}: {IsValid}, {nameof(Results)}: {string.Join(",", Results.Select(r =>r.MemberNames))}";
        }

        public string ResultsToString()
        {
            return string.Join(",",
                Results.Select(r => $"{{Property: {string.Join(",", r.MemberNames)}, Error: {r.ErrorMessage} }}"));
        }
    }
    
    
    public static class ModelValidator
    {
        public static ModelValidationResult Validate(object model)
        {
            try
            {

                var context = new ValidationContext(model, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();

                return new ModelValidationResult
                {
                    IsValid = Validator.TryValidateObject(model, context, validationResults, true),
                    Results = validationResults
                };
            }
            catch (ValidationException e)
            {
                return new ModelValidationResult
                {
                    IsValid = false,
                    Results = new List<ValidationResult> {e.ValidationResult}
                };
            }
        }
    }
}