using System;

namespace Backend.ViewModels.Users
{
    public class UserAuthenticate
    {
        public Guid Id { set; get; }
        public string UserName { set; get; }
        public string FullName { set; get; }
        public string Address { set; get; }
        public string ImagePath { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
        public string AccessToken { set; get; }
    }
}