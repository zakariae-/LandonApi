using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LandonApi.Models;

namespace LandonApi.Services
{
    public interface IRoomService
    {
        Task<Room> GetRoomAsync(
            Guid id, 
            CancellationToken ct);

        Task<PageResults<Room>> GetRoomsAsync(
            PagingOptions pagingOptions, 
            SortOptions<Room, RoomEntity> sortOptions, 
            CancellationToken ct);
    }
}
