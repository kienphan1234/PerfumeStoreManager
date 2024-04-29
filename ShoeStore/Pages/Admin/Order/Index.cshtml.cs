using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebRazor.Pages.Admin.Order
{
   
    public class IndexModel : PageModel
    {
        private readonly ShoeStore.Models.PRN221DBContext _context;

        public IndexModel(ShoeStore.Models.PRN221DBContext context)
        {
            _context = context;
        }

        public IList<ShoeStore.Models.Order> Order { get;set; } = default!;
        public List<ShoeStore.Models.Order> orderList { get; set; }

        [BindProperty(SupportsGet = true, Name = "currentPage")]
        public int currentPage { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? dateFrom { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? dateTo { get; set; }
        public int totalPages { get; set; }

        public const int pageSize = 5;
        public async Task<IActionResult> OnGet()
        {
			if (HttpContext.Session.GetString("Admin") == null)
			{
				return RedirectToPage("/Account/Login");
			}
			var acc1 = JsonConvert.DeserializeObject<ShoeStore.Models.Account>(HttpContext.Session.GetString("Admin"));
			if (currentPage < 1)
            {
                currentPage = 1;
            }

            int totalOrders = getTotalOrders();

            totalPages = (int)Math.Ceiling((double)totalOrders / pageSize);

            orderList = getAllOrders();


            return Page();
        }

        private int getTotalOrders()
        {
            var list = from order in _context.Orders orderby order.OrderDate ascending select order;
            if (dateFrom == null || dateTo == null
               || (dateFrom == null && dateTo == null))
            {
                return list.Count();
            }

            return list.Where(o => DateTime.Compare(o.OrderDate.Value, dateFrom.Value) >= 0
                    && DateTime.Compare(o.OrderDate.Value, dateTo.Value) <= 0).ToList().Count();

        }

        private List<ShoeStore.Models.Order> getAllOrders()
        {
            var list = from order in _context.Orders
                       .Include(e => e.Employee)
                       .Include(c => c.Customer)
                       orderby order.OrderDate ascending
                       select order;
            List<ShoeStore.Models.Order> orders;
            if (dateFrom == null || dateTo == null
                || (dateFrom == null && dateTo == null))
            {
                orders = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                orders = list.Where(o => DateTime.Compare(o.OrderDate.Value, dateFrom.Value) >= 0
                    && DateTime.Compare(o.OrderDate.Value, dateTo.Value) <= 0)
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize).ToList();
            }

            return orders;
        }

    }
}
