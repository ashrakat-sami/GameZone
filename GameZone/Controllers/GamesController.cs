
using GameZone.Models;
using GameZone.Services;
using Microsoft.AspNetCore.Authorization;

namespace GameZone.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
       
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;

        public GamesController(IDevicesService devicesService, ICategoriesService categoriesService, IGamesService gamesService)
        {
            _devicesService = devicesService;
            _categoriesService = categoriesService;
            _gamesService = gamesService;
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var games = _gamesService.GetAll();
            return View(games);
        }
        public IActionResult Details(int id)
        {
            var game = _gamesService.GetById(id);
            if(game is null)
                return NotFound();
            return View(game);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectLists()
            };  
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectLists();
                return View(model);
            }
           await _gamesService.Create(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var game = _gamesService.GetById(id);
            if (game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d => d.DeviceId).ToList(),
                Categories = _categoriesService.GetSelectList(),
                Devices = _devicesService.GetSelectLists(),
                CurrentCover=game.Cover
            };
            

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectList();
                model.Devices = _devicesService.GetSelectLists();
                return View(model);
            }


            var game= await _gamesService.Update(model);

            if (game is null)
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) 
        {
            
            var isDeleted= _gamesService.Delete(id);
           

            return isDeleted ?Ok() : BadRequest();
        
        }

        public ActionResult SearchItems(string searchTerm)
        {
            var games = _gamesService.GetAll();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                games = games.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            return View(games);
        }

    }

}
