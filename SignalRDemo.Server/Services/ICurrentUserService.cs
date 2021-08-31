using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Server.Services
{
    public interface ICurrentUserService<out TKey>
    {
        TKey UserId { get; }
        string FullName { get; set; }
        bool IsAuthenticated { get; }
    }
}
