using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;

namespace WebRazor.Pages.Product
{
    public class DetailModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public DetailModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Dictionary<ShoeStore.Models.Product, int> Cart { get; set; } = new Dictionary<ShoeStore.Models.Product, int>();

        [BindProperty]
        public ShoeStore.Models.Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            var cat = await dbContext.Categories.ToListAsync();

            var cart = HttpContext.Session.GetString("cart");

            Dictionary<int, int> list;
            if (cart != null)
            {
                list = JsonSerializer.Deserialize<Dictionary<int, int>>(cart);
                foreach (var item in list)
                {
                    ShoeStore.Models.Product product = dbContext.Products.FirstOrDefault(p => p.ProductId == item.Key);

                    Cart.Add(product, item.Value);
                }
            }

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == id);
            var cat = await dbContext.Categories.ToListAsync();

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }


    }
}
