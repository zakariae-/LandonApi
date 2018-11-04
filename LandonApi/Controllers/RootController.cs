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
                Self = Link.To(nameof(RootController.GetRoot)), // TODO Url.Link(nameof(GetRoot), null)
                Rooms = Link.To(nameof(RoomsController.GetRoomsAsync)),
                Info = Link.To(nameof(InfoController.GetInfo))
            };

            return Ok(response);
        }
    }
}
