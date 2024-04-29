
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;
using WebRazor.Helpers;


namespace WebRazor.Pages.Account
{
   
    public class LoginModel : PageModel
    {
        private readonly PRN221DBContext dbContext;

        public LoginModel(PRN221DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [BindProperty]
        public ShoeStore.Models.Account Account { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            

            string password = Password_encryption.HashPassWord(Account.Password);
            

            string pattern = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";

            //check first string
            if (!Regex.IsMatch(Account.Email, pattern))
            {
                ViewData["msg"] = "Email is right format. Example: duy123@gmail.com";
                return Page();
            }
            var email = await dbContext.Accounts.SingleOrDefaultAsync(a => a.Email.Equals(Account.Email));
            if (email == null)
            {
                ViewData["msg"] = "Email is not exist";
                return Page();
            }

            var acc = await dbContext.Accounts
                .SingleOrDefaultAsync(a => a.Email.Equals(Account.Email) && a.Password.Equals(password));

            if (acc == null)
            {
                ViewData["msg"] = "Password is wrong. Pls re-enter password";
                return Page();
            }
            if (acc.Role == 2)
            {
                acc.Customer = null;
                HttpContext.Session.SetString("CustSession", JsonSerializer.Serialize(acc));
				HttpContext.Session.SetString("UserEmail", Account.Email);
				return RedirectToPage("/index");
            }
            else if (acc.Role == 1)
            {
                HttpContext.Session.SetString("Admin", JsonSerializer.Serialize(acc));
                HttpContext.Session.SetString("UserEmail", Account.Email);
            }
            return Redirect("/Admin/Dashboard");
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("CustSession");
            HttpContext.Session.Remove("Admin");

            return RedirectToPage("~/index");
        }

    }
}
