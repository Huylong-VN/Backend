namespace Backend.Application.Authorization
{
    public static class Permissions
    {
        public static class Posts
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Update = "Permissions.Products.Update";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Update = "Permissions.Users.Update";
            public const string Delete = "Permissions.Users.Delete";
        }
    }
}