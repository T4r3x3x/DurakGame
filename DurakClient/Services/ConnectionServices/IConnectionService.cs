using System;
using System.Threading.Tasks;

namespace DurakClient.Services.ConnectionServices
{
    public interface IConnectionService
    {
        public Task<Guid> Connect(string nickname);
        public Task Disconnect(Guid? guid);
    }
}