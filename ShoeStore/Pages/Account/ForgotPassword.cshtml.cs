using ManagerSstore.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using WebRazor.Helpers;

namespace ManagerSstore.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        accountDAO accountdao = new accountDAO();
        private readonly PRN221DBContext _dbContext;

        public ForgotPasswordModel(PRN221DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            Session.service = "Forgotpassword";
        }

        public IActionResult OnPost()
        {

            if (Request.Form["newpassword"].Equals("") ||
                Request.Form["repassword"].Equals("") || Request.Form["otp"].Equals(""))
            {
                Session.notification = "Infomation must not null";
                return RedirectToPage("ForgotPassword");
            }
            else
            {
                try
                {
                    String newpassword = Request.Form["newpassword"];
                    String repasswpord = Request.Form["repassword"];
                    String otp = Request.Form["otp"];

                    if (!newpassword.Equals(repasswpord))
                    {
                        Session.notification = "New password must the same re-password";
                        return RedirectToPage("ForgotPassword");
                    }

                    if (!otp.Equals(Session.OTP))
                    {
                        Session.notification = "OTP is failded";
                        return RedirectToPage("ForgotPassword");
                    }

                    Account a = Session.accountForgot;
                    a.Password = Password_encryption.HashPassWord(newpassword);
                    _dbContext.Entry(a).State = EntityState.Modified;

                    // Save changes to the database
                    _dbContext.SaveChanges();

                    Session.accountLogin = a;
                    Session.notification = "Forgot password complete";
                }
                catch (Exception ex)
                {
                    Session.notification = "Forgot password failded";
                }
                return RedirectToPage("Login");
            }
        }
    }
}
