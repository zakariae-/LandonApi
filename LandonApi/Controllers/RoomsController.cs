using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LandonApi.Models;
using LandonApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace LandonApi.Controllers
{
    [Route("/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly PagingOptions _defaultPagingOptions;

        public RoomsController(IRoomService roomService, IOptions<PagingOptions> defaultPagingOptions)
        {
            _roomService = roomService;
            _defaultPagingOptions = defaultPagingOptions.Value;
        }

        [HttpGet(Name = nameof(GetRoomsAsync))]
        public async Task<IActionResult> GetRoomsAsync(
            [FromQuery] PagingOptions pagingOptions,
            CancellationToken ct)
        {
            if (!ModelState.IsValid) return BadRequest(new ApiError(ModelState));
            pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
            pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

            var rooms = await _roomService.GetRoomsAsync(pagingOptions, ct);

            var collection = PagedCollection<Room>.Create(
                Link.ToCollection(nameof(GetRoomsAsync)), 
                rooms.Items.ToArray(), 
                rooms.TotalSize, 
                pagingOptions);

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
