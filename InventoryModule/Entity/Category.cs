using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InventoryModule.Entity
{
    public class Category
    {
        public const string StatusActive = "Active";
        public const string StatusInactive = "Inactive";
        protected Category()
        {
                
        }
        public Category(string name)
        {
            Name = name;
            Activate();
        }
        public void Udpate(string name)
        {
            Name = name;
        }
     
        public int Id { get;protected set; }
        public string   Name { get; protected set; }
        public string  Status { get; protected set; }
        public bool IsActive => Status == StatusActive;
         public void Activate() => Status = StatusActive;
        public void Deactivate() => Status = StatusInactive;
    }
}
