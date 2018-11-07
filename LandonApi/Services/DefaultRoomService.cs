using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LandonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LandonApi.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly HotelApiContext _context;
        private readonly IMapper _mapper;

        public DefaultRoomService(HotelApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Room> GetRoomAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == id, ct);
            if (entity == null) return null;

            return _mapper.Map<Room>(entity);
        }

        public async Task<PageResults<Room>> GetRoomsAsync(PagingOptions pagingOptions, CancellationToken ct)

        {
            var rooms = await _context.Rooms.ToArrayAsync();
            var query = _context.Rooms
                                .Select(entity => _mapper.Map<Room>(entity));

            var pagedRooms = query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value);

            return new PageResults<Room>
            {
                Items = pagedRooms,
                TotalSize = rooms.Count()
            };
        }
    }
}
