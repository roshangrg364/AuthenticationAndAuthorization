using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserModule.Entity
{
    public class User:IdentityUser
    {
        protected User()
        {
                
        }
        public User(string name, string userName, string password,string mobile,string email) : base(userName) 
        {
            PasswordHash = password;
            Name = name;
            Email = email;
            PhoneNumber = mobile;
            CreatedOn = DateTime.Now;
            Activate();
        }

        public string Name { get; protected set; }
        private const string StatusActive = "ACTIVE";
        private const string StatusInactive = "INACTIVE";
        public DateTime CreatedOn { get; protected set; }
        public string Status { get; protected set; }
        public bool IsActive => Status == StatusActive;
        public void Activate() => Status = StatusActive;
        public void Deactivate() => Status = StatusInactive;

    }
}
