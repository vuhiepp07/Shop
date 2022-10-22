using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Shop.Models;

namespace Shop{
    public static class Helper{
        public static string RandomString(int len){
            Random rand = new Random();
            string pattern = "qwertyuiopasdfghjklzxcvbnm1234567890";
            char[] arr = new char[len];
            for(int i = 0; i < len; i++)
            {
                arr[i]= pattern[rand.Next(pattern.Length)];
            }
            return string.Join(string.Empty, arr);
            
        }

        public static string SendEmails(EmailMessage obj, IConfiguration configuration){
            IConfiguration section = configuration.GetSection("Mails:Gmail");
            using (SmtpClient client = new SmtpClient(section.GetValue<string>("Host"), section.GetValue<int>("Port")){
                Credentials = new NetworkCredential(section.GetValue<string>("Email"), section.GetValue<string>("Password")), EnableSsl = true
            }){
                try{
                    MailMessage message = new MailMessage(new MailAddress(section.GetValue<string>("Email"),"TheShop"), new MailAddress(obj.EmailTo)){
                        IsBodyHtml = true,
                        Subject = obj.Subject,
                        Body = obj.Content
                    };
                    client.Send(message);
                    return "Send mail success";
                }
                catch(Exception ex){
                    return ex.Message;
                }
            }
        }

        public static byte[] Hash(string plaintext)
		{
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA-512");
            return algorithm.ComputeHash(Encoding.ASCII.GetBytes(plaintext));
		}

        public static string normalizePrice(string price){
            for(int i = price.Length -1, x = 0 ; i> 0; i--, x++){
                if(x == 2){
                    price = price.Insert(i, ",");
                    i++;
                    x=-2;
                }
            }
            return price;
        }

        public static string normalizeName(string name){
            string[] arr = name.Split(" ");
            string result;
            if(arr.Length >= 2){
                result = arr[arr.Length - 2] + " " + arr[arr.Length - 1];
            }
            else result = name;
            return result;
        }
    }
}