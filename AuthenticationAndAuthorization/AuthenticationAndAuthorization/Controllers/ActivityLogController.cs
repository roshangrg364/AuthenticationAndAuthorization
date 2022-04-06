using BaseModule.ActivityManagement.Repo;
using InventorySystemMysql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventorySystemMysql.Controllers
{
    public class ActivityLogController : Controller
    {
        private readonly ActivityLogRepositoryInterface _activityLogRepo;
        public ActivityLogController(ActivityLogRepositoryInterface activityLogRepo)
        {
            _activityLogRepo = activityLogRepo;
        }
        public async Task<IActionResult> Index()
        {
            var activityLogs = await _activityLogRepo.GetQueryable().OrderByDescending(a=>a.Id).ToListAsync();
            var activityLogModel = new List<ActivityLogViewModel>();
                foreach (var activityLog in activityLogs)
            {
                activityLogModel.Add(new ActivityLogViewModel
                {
                    Area = activityLog.Area,
                    ActionName = activityLog.ActionName,
                    ControllerName = activityLog.ControllerName,
                    IpAddress = activityLog.IpAddress,
                    PageAccessed = activityLog.PageAccessed,
                    SessionId = activityLog.SessionId,
                    Browser = activityLog.Browser,
                    UrlReferrer = activityLog.UrlReferrer,
                    UserName = activityLog.UserName,
                    UserId = activityLog.UserId,
                    Status = activityLog.Status,
                    Data = activityLog.Data,
                    ActionOn = activityLog.ActionOn.ToString("yyyy-MM-dd hh:mm tt")
                });

            }
            return View(activityLogModel);
        }
    }
}
