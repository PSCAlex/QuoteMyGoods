using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding.Validation;

namespace QuoteMyGoods.Validation
{
    public class FootballTeamValidation : ValidationAttribute, IClientModelValidator
    {
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
    }
}
