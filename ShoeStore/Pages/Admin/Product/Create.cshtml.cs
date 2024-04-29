using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebRazor.Hubs;

namespace WebRazor.Pages.Admin.Product
{
   
    public class CreateModel : PageModel
    {
        private readonly ShoeStore.Models.PRN221DBContext _context;
        private readonly IHubContext<HubServer> hubContext;

        public CreateModel(ShoeStore.Models.PRN221DBContext context, IHubContext<HubServer> hubContext)
        {
            _context = context;
            this.hubContext = hubContext;
        }

        public IActionResult OnGet()
        {
			if (HttpContext.Session.GetString("Admin") == null)
			{
				return RedirectToPage("/Account/Login");
			}
			var acc1 = JsonConvert.DeserializeObject<ShoeStore.Models.Account>(HttpContext.Session.GetString("Admin"));
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public ShoeStore.Models.Product Product { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("ReloadData");
            return RedirectToPage("index");
        }
    }
}
