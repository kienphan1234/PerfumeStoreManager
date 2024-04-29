
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ShoeStore.Models;

namespace ShoeStore.Pages.Admin.Account
{
    
    public class IndexModel : PageModel
    {
        private readonly PRN221DBContext _accountService; // Replace with your service or repository interface

        public IndexModel(PRN221DBContext accountService) // Replace with your service or repository
        {
            _accountService = accountService;
        }

        public List<ShoeStore.Models.Account> Accounts { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToPage("/Account/Login");
            }
            var acc1 = JsonConvert.DeserializeObject<ShoeStore.Models.Account>(HttpContext.Session.GetString("Admin"));
            Accounts = _accountService.Accounts.ToList();
            return Page();

        }
    }
}
