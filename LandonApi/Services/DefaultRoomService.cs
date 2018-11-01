using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
    }
}
