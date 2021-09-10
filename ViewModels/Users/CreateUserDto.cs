namespace Backend.ViewModels.Users
{
    public class CreateUserDto
    {
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }
    }
}