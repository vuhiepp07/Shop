using System.Security.Cryptography;
using System.Text;

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
    }
}