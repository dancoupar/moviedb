using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MovieDb.UI.Models;
using System.Diagnostics;

namespace MovieDb.UI.Controllers
{
	public class HomeController(IOptions<ApiSettings> apiSettings) : Controller
	{
		private readonly ApiSettings _apiSettings = apiSettings.Value;

		public IActionResult Index()
		{
			ViewData["ApiUrl"] = _apiSettings.ApiUrl;
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
