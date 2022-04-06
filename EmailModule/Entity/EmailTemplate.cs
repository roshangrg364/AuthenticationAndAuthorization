using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailModule.Entity
{
    public class EmailTemplate
    {
        public const string TypeRegistration = "Registration";
        public const string TypePasswordReset = "ResetPassword";

        public EmailTemplate(string type, string template)
        {
            Type = type;
            Template = template;
        }
        public void Update(string type,string template)
        {
            Type = type;
            Template = template;
        }
        public long  Id { get;protected set; }
        public string Type { get; protected set; }
        public string Template { get;protected set; }

        public static IList<string> TemplateVariables => new List<string>
        {
            "{Name}"
        };
    }
    
}
