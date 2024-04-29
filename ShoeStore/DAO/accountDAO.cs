
using ShoeStore.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace ManagerSstore.DAO
{
    public class accountDAO : PRN221DBContext
    {
      


        public void SendMail(String ToMail, String input, String title)
        {
            try
            {
                var frommail = new MailAddress("blancspport302@gmail.com");
                var tomail = new MailAddress(ToMail);
                String pass = "zaswsqwilvfigpmx";
                //String subject = "SStore Forgot Password";
                String body = "Your OTP:" + input;

                var smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(frommail.Address, pass);
                smtp.Timeout = 2000000;

                var mess = new MailMessage(frommail, tomail);

                mess.Subject = title;
                mess.Body = body;
                smtp.Send(mess);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Erorr Send Mail: " + e.Message);
            }

        }

        //=================================================================================================
        public String getOTP()
        {

            string UpperCase = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string LowerCase = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "1234567890";
            string allCharacters = UpperCase + LowerCase + Digits;
            //Random will give random charactors for given length  
            Random r = new Random();
            String password = "";
            for (int i = 0; i < 8; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    password += UpperCase.ToCharArray()[(int)Math.Floor(rand * UpperCase.Length)];
                }
                else
                {
                    password += allCharacters.ToCharArray()[(int)Math.Floor(rand * allCharacters.Length)];
                }
            }
            return password;
        }
        public string datenow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        //=================================================================================================
        public bool isValid(string input, string regex)
        {
            // @"(^[0]+[0-9]{9}$)" phone
            // @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" mail
            // @"\D+\z" name
            Regex re = new Regex(regex);

            if (re.IsMatch(input))
                return (true);
            else
                return (false);
        }

    }
}
