using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.AuditManagement
{
  public  class Audit
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string IpAddress { get; set; }
        public string Browser { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime ActionOn { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
        public string? PrimaryKey { get; set; }
    }
}
