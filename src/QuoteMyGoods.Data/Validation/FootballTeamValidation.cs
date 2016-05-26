using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace QuoteMyGoods.Validation
{
    public class FootballTeamValidation : ValidationAttribute, IClientModelValidator
    {
        public void AddValidation(ClientModelValidationContext context)
        {
            throw new NotImplementedException();
        }

        /*
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ClientModelValidationContext context)
        {
            var rule = new ModelClientValidationRule("footballteam","People called Alex Logan must support Manchester United");
            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            MegaFormModel megaForm = (MegaFormModel)validationContext.ObjectInstance;

            if(megaForm.FirstName == "Alex" && megaForm.LastName == "Logan "&& megaForm.Team != "Manchester United")
            {
                return new ValidationResult("People called Alex Logan must support Manchester United");
            }
            return ValidationResult.Success;
        }
        */
    }
}
