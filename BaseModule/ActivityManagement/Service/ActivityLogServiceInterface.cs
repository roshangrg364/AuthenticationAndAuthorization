using BaseModule.ActivityManagement.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BaseModule.ActivityManagement.Service
{
    public interface ActivityLogServiceInterface
    {
        Task Create(ActivityLogDto dto);
    }
}
