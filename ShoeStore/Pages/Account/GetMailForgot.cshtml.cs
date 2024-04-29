
using ManagerSstore.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;

namespace ManagerSstore.Pages
{
    public class GetMailForgotModel : PageModel
    {
        accountDAO dao = new accountDAO();
        private readonly PRN221DBContext _dbContext;

        public GetMailForgotModel(PRN221DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {

            if (Request.Form["email"].Equals(""))
            {
                Session.notification = "Email must not null";
                return RedirectToPage("GetMailForgot");
            }
            String email = Request.Form["email"];

            if (!dao.isValid(email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                Session.notification = "Format email failded";
                return RedirectToPage("GetMailForgot");
            }
            Account acc = _dbContext.Accounts.FirstOrDefault(a => a.Email == email);
            if (acc == null)
            {
                Session.notification = "Email dont register";
                return RedirectToPage("GetMailForgot");
            }
            Session.accountForgot = acc;
            return RedirectToPage("ForgotPassword");
        }
    }
}
