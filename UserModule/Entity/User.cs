using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Entity
{
    public class User:IdentityUser
    {
        public const string TypeSuperAdmin = "SUPERADMIN";
        public const string TypeAdmin = "ADMIN";
        public const string TypeGeneral = "GENERAL";
        public const string TypeExternal = "EXTERNAL";
        public const string TypeCustomer = "CUSTOMER";
        
        protected User()
        {
                
        }
        public User( string userName,string email,string type) : base(userName) 
        {
            UserName = userName;
            Email = email;
            CreatedOn = DateTime.Now;
            Type = type;
            Activate();
        }

        public string Name { get;  set; }
        private const string StatusActive = "ACTIVE";
        private const string StatusInactive = "INACTIVE";
        public DateTime CreatedOn { get; protected set; }
        public string Status { get; protected set; }
        public string Type { get; set; }
        public bool IsActive => Status == StatusActive;
        public void Activate() => Status = StatusActive;
        public void Deactivate() => Status = StatusInactive;

    }
}
