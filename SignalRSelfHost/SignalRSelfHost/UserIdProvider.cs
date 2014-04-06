using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRSelfHost
{
    public class UserIdProvider:IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.Headers["UserID"];
        }
    }
}
