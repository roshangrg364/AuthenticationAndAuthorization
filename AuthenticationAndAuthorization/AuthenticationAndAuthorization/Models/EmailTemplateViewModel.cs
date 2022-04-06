using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmailModule.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuthenticationAndAuthorization.Models
{
    public class EmailTemplateIndexViewModel
    {
        public long Id { get; set; }
        public string Type { get; set; }
       
    }

    public class EmailTemplateCreateViewModel
    {
        [Required(ErrorMessage ="Type is required")]
        public string Type { get; set; }
        private IList<string> TypeList { get; set; } = new List<string> {
        EmailTemplate.TypeRegistration,
        EmailTemplate.TypePasswordReset
        };

        public SelectList TypeSelectList => new SelectList(TypeList);
        [Required(ErrorMessage = "Template is required")]
        public string Template { get; set; }
    }

    public class EmailTemplateUpdateViewModel:EmailTemplateCreateViewModel
    {
        public long Id { get; set; }
    }

}
