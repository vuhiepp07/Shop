

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
    }
}