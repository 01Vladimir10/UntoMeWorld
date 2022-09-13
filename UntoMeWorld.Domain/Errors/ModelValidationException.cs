using UntoMeWorld.Domain.Validation;

namespace UntoMeWorld.Domain.Errors
{
    public class ModelValidationException : UserErrorException
    {
        
        public ModelValidationException(ModelValidationResult result) : base($"Validation Error: {result.ResultsToString()}")
        {
            Cause = "One or more properties of the object you are trying to create had invalid values";
            Fix = "Check the properties of the model and try again";

        }
    }
}