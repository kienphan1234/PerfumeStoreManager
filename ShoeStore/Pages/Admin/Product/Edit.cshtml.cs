﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebRazor.Hubs;

namespace WebRazor.Pages.Admin.Product
{
   
    public class EditModel : PageModel
    {
        private readonly ShoeStore.Models.PRN221DBContext _context;
        private readonly IHubContext<HubServer> hubContext;

        public EditModel(ShoeStore.Models.PRN221DBContext context, IHubContext<HubServer> hubContext)
        {
            _context = context;
            this.hubContext = hubContext;
        }

        [BindProperty]
        public ShoeStore.Models.Product Product { get; set; } = default!;

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

            var product =  await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
           ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await hubContext.Clients.All.SendAsync("ReloadData");
            return RedirectToPage("index");
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
