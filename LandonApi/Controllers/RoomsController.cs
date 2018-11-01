using System;
using Microsoft.AspNetCore.Mvc;

namespace LandonApi.Controllers
{
    [Route("/[controller]")]
    public class RoomsController : Controller
    {
        [HttpGet(Name = nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }
    }
}
