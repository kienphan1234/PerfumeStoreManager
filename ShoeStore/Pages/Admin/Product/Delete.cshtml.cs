﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebRazor.Hubs;

namespace WebRazor.Pages.Admin.Product
{
    
    public class DeleteModel : PageModel
    {
        private readonly ShoeStore.Models.PRN221DBContext _context;
        private readonly IHubContext<HubServer> hubContext;
        public DeleteModel(ShoeStore.Models.PRN221DBContext context, IHubContext<HubServer> hubContext)
        {
            _context = context;
            this.hubContext = hubContext;
        }

        [BindProperty]
      public ShoeStore.Models.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
			if (HttpContext.Session.GetString("Admin") == null)
			{
				return RedirectToPage("/Account/Login");
			}
			var acc1 = JsonConvert.DeserializeObject<ShoeStore.Models.Account>(HttpContext.Session.GetString("Admin"));
			if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                Product = product;
                _context.Products.Remove(Product);
                await _context.SaveChangesAsync();
            }

            await hubContext.Clients.All.SendAsync("ReloadData");
            return RedirectToPage("index");
        }
    }
}
