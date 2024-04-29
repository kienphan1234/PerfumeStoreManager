using ManagerSstore.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using System.Text.Json;
using WebRazor.Helpers;

namespace ManagerSstore.Pages
{
    public class ChangePasswordModel : PageModel
    {
        accountDAO accountdao = new accountDAO();
        private readonly PRN221DBContext _dbContext;
		public ShoeStore.Models.Account Auth { get; set; }

		public ChangePasswordModel(PRN221DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            Session.service = "Changepassword";
        }
        public IActionResult OnPost()
        {
            if (Request.Form["oldpassword"].Equals("") || Request.Form["newpassword"].Equals("") ||
                Request.Form["repassword"].Equals("") || Request.Form["otp"].Equals(""))
            {
                Session.notification = "Infomation must not null";
                return RedirectToPage("ChangePassword");
            }
            else
            {
                try
                {
                    String oldpassword = Password_encryption.HashPassWord(Request.Form["oldpassword"]);
                    String newpassword = Request.Form["newpassword"];
                    String repasswpord = Request.Form["repassword"];
                    String otp = Request.Form["otp"];
					Auth = JsonSerializer.Deserialize<ShoeStore.Models.Account>(HttpContext.Session.GetString("CustSession"));
					if (!oldpassword.Equals(Auth.Password))
                    {
                        Session.notification = "Old password is failded";
                        return RedirectToPage("ChangePassword");
                    }

                    if (!newpassword.Equals(repasswpord))
                    {
                        Session.notification = "New password must the same re-password";
                        return RedirectToPage("ChangePassword");
                    }

                    if (!otp.Equals(Session.OTP))
                    {
                        Session.notification = "OTP is failded";
                        return RedirectToPage("ChangePassword");
                    }

                    Account a = Auth;
                    a.Password= Password_encryption.HashPassWord(newpassword);
                    _dbContext.Entry(a).State = EntityState.Modified;

                    // Save changes to the database
                    _dbContext.SaveChanges();
                    Session.accountLogin = a;
                    Session.notification = "Change password complete";
                }
                catch (Exception ex)
                {
                    Session.notification = "Change passwrod failded";
                }
                return RedirectToPage("Profile");
            }

        }
    }
}
