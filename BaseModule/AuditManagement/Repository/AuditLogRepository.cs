using BaseModule.BaseRepo;
using BaseModule.DbContextConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseModule.AuditManagement.Repository
{
    public class AuditLogRepository:BaseRepository<Audit>,AuditLogRepositoryInterface
    {
        public AuditLogRepository(MyDbContext context):base(context)
        {

        }
    }
}
