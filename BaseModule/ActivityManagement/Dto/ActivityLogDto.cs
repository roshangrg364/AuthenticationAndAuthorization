using System;
using System.Collections.Generic;
using System.Text;

namespace BaseModule.ActivityManagement.Dto
{
  public  class ActivityLogDto
    {
        public ActivityLogDto(string area, string controller,string action,string ipAddress,string pageAccessed,
            string sessionId,string userName,string userId,string urlRef,string browser,string status,string data,string queryString)
        {
            Area = area;
            ControllerName = controller;
            ActionName = action;
            IpAddress = IpAddress;
            PageAccessed = pageAccessed;
            SessionId = sessionId;
            UserName = userName;
            UserId = userId;
            UrlReferrer = urlRef;
            Browser = browser;
            Status = status;
            Data = data;
            QueryString = queryString;
        }
        public string Area { get;  protected set; }
        public string Status { get;  protected set; }
        public string QueryString { get;  protected set; }
        public string Data { get;  protected set; }
        public string ControllerName { get; protected set; }
        public string ActionName { get; protected set; }

        public string IpAddress { get; protected set; }
        
        public string PageAccessed { get; protected set; }
        public string SessionId { get; protected set; }
        public string UrlReferrer { get; protected set; }
        public string UserId { get; protected set; }
        public string UserName { get; protected set; }
        public string Browser { get; protected set; }

    }
}
