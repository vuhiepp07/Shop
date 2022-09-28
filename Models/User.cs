namespace Shop.Models{
    public class User{
        public Guid UserId {get; set;}
        public string Username{get; set;}
        public string Password {get; set;}
        public string Email {get; set;}
        public string FullName {get; set;}
        public string Phone {get; set;}
        public bool Gender {get; set;}
        public string DateOfBirth {get; set;}
        public string ImageUrl {get; set;}
    }
}