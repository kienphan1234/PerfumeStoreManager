using ManagerSstore.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace ManagerSstore.Pages
{
    public class GetOTPModel : PageModel
    {
        accountDAO accountdao = new accountDAO();
		public ShoeStore.Models.Account Auth { get; set; }
		public IActionResult OnGet()
        {
            if (Session.service.Equals("Changepassword"))
            {
				Auth = JsonSerializer.Deserialize<ShoeStore.Models.Account>(HttpContext.Session.GetString("CustSession"));
				Session.OTP = accountdao.getOTP();
                accountdao.SendMail(Auth.Email, Session.OTP, "Blanc Send OTP code to Change password");

                return RedirectToPage("ChangePassword");
            }

            if (Session.service.Equals("Forgotpassword"))
            {
                Session.OTP = accountdao.getOTP();
                accountdao.SendMail(Session.accountForgot.Email, Session.OTP, "Blanc Send OTP code to Forgot Password");

                return RedirectToPage("ForgotPassword");
            }
            return RedirectToPage("GetOTP");

        }
    }
}
