using AuthenticationAndAuthorization.Models;
using BaseModule.AuditManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserModule.Repository;

namespace AuthenticationAndAuthorization.Controllers
{
    public class AuditLogController : Controller
    {
        private readonly ILogger<AuditLogController> _logger;
        private readonly AuditLogRepositoryInterface _auditLogRepo;
        private readonly UserRepositoryInterface _userRepo;
        public AuditLogController(ILogger<AuditLogController> logger,
            AuditLogRepositoryInterface auditLogRepo,
            UserRepositoryInterface userRepo)
        {
            _logger = logger;
            _auditLogRepo = auditLogRepo;
            _userRepo = userRepo;
        }
        public async Task<IActionResult> Index()
        {
            var auditLogs = await _auditLogRepo.GetAllAsync();
            var auditLogIndexModel = new List<AuditLogViewModel>();
            foreach(var log in auditLogs)
            {
                auditLogIndexModel.Add(new AuditLogViewModel
                {
                    Id = log.Id,
                    UserName = (await _userRepo.GetByIdString(log.UserId))?.Name ?? "",
                    TableName = log.TableName,
                    OldValues = log.OldValues,
                    NewValues = log.NewValues,
                    AffectedColumns = log.AffectedColumns,
                    Type = log.Type,
                    ActionOn = log.ActionOn.ToString("yyyy-MM-dd hh:mm tt")
                });
            }
            return View(auditLogIndexModel);
        }
    }
}
