using System;
using System.Threading;
using System.Threading.Tasks;
using LandonApi.Models;

namespace LandonApi.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomAsync(Guid id, CancellationToken ct);
    }
}
