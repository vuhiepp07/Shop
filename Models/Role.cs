namespace Shop.Models{
    public class Role{
        public int RoleId {get; set;}
        public string RoleName {get; set;}

        public virtual List<User> UsersInRole {get; set;}
    }
}