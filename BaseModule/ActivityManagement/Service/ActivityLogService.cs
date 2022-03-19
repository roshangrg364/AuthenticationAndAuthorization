using BaseModule.ActivityManagement.Dto;
using BaseModule.ActivityManagement.Entity;
using BaseModule.ActivityManagement.Repo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BaseModule.ActivityManagement.Service
{
    public class ActivityLogService : ActivityLogServiceInterface
    {
        private readonly ActivityLogRepositoryInterface _activityLogRepo;
        public ActivityLogService(ActivityLogRepositoryInterface activityLogRepo)
        {
            _activityLogRepo = activityLogRepo;
        }
        public async Task Create(ActivityLogDto dto)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var activityLog = new ActivityLog(dto.Area, dto.ControllerName, dto.ActionName, dto.IpAddress,
                dto.PageAccessed, dto.SessionId, dto.UserName, dto.UserId, dto.UrlReferrer,dto.Browser,dto.Status,dto.Data,dto.QueryString);
            await _activityLogRepo.InsertWithoutTrackingAsync(activityLog).ConfigureAwait(false);
            tx.Complete();
        }
    }
}
