using Napos.Core.Exceptions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Napos.Core.Helpers
{
    public static class ValidationHelper
    {
        public static IList<ValidationResult> ValidateModel(this object model, ValidationContext context = null)
        {
            var result = new List<ValidationResult>();
            var ctx = context ?? new ValidationContext(model);
            Validator.TryValidateObject(model, ctx, result, true);
            return result;
        }

        public static bool IsModelValid(this object model, out List<ValidationResult> result, ValidationContext context = null)
        {
            result = new List<ValidationResult>();
            var ctx = context ?? new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, ctx, result, true);
            return isValid;
        }

        public static void ValidateModelAndThrow(this object model, ValidationContext context = null)
        {
            var result = new List<ValidationResult>();
            var ctx = context ?? new ValidationContext(model);
            if (!Validator.TryValidateObject(model, ctx, result, true))
                throw new UserException(result.Select(x => x.ErrorMessage).ToArray(), result.Select(x => x.MemberNames.FirstOrDefault()).ToArray());
        }
    }
}
