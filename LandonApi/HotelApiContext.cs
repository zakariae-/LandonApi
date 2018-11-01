using System;
using LandonApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LandonApi
{
    public class HotelApiContext : DbContext
    {
        public HotelApiContext(DbContextOptions options) : base(options) { }

        public DbSet<RoomEntity> Rooms { get; set; }
    }
}
