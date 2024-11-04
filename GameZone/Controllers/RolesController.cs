using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        //Create a Role
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> New(RoleViewModel newRole)
        {
            if (ModelState.IsValid) 
            {
                IdentityRole role = new IdentityRole();
                role.Name = newRole.RoleName;

              IdentityResult result= await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return View(new RoleViewModel());
                }
                foreach (var item in result.Errors)
                { 
                    ModelState.AddModelError("",item.Description);
                }
            }
            return View(newRole);


        }

       

    }
}
