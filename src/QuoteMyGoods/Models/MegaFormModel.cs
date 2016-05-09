using QuoteMyGoods.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Models
{
    public class MegaFormModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(55)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        [Url]
        public string Url { get; set; }
        [CreditCard]
        public string CreditCard { get; set; }
        [FootballTeamValidation]
        public string Team { get; set; }
    }
}
