using LandonApi.Models;
using Microsoft.AspNetCore.Mvc;
namespace LandonApi.Controllers
{
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new RootResponse
            {
                Href = null,
                Rooms = Link.To(nameof(RoomsController.GetRooms)),
                Info = Link.To(nameof(InfoController.GetInfo))
            };

            return Ok(response);
        }
    }
}
