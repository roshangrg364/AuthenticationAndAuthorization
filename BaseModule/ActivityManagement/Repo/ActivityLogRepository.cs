using BaseModule.ActivityManagement.Entity;
using BaseModule.BaseRepo;
using BaseModule.DbContextConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.ActivityManagement.Repo
{
   public class ActivityLogRepository:BaseRepository<ActivityLog>,ActivityLogRepositoryInterface
    {

        public ActivityLogRepository(MyDbContext context):base(context)
        {

        }
    }
}
