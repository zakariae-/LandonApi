using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LandonApi.Models;
using LandonApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LandonApi.Controllers
{
    [Route("/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet(Name = nameof(GetRoomsAsync))]
        public async Task<IActionResult> GetRoomsAsync(
            [FromQuery] PagingOptions pagingOptions,
            CancellationToken ct)
        {
            var rooms = await _roomService.GetRoomsAsync(pagingOptions, ct);

            var collectionLink = Link.ToCollection(nameof(GetRoomsAsync));
            var collection = new PagedCollection<Room>
            {
                Self = collectionLink,
                Value = rooms.Items.ToArray(),
                Size = rooms.TotalSize,
                Offset = pagingOptions.Offset.Value,
                Limit = pagingOptions.Limit.Value
            };

            return Ok(collection);
        }

        // /room/{roomId}
        [HttpGet("{roomId}", Name = nameof(GetRoomByIdAsync))]
        public async Task<IActionResult> GetRoomByIdAsync(Guid roomId, CancellationToken ct)
        {
            var resource = await _roomService.GetRoomAsync(roomId, ct);
            if (resource == null) return NotFound();

            return Ok(resource);
        }
    }
}
