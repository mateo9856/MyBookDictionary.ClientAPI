namespace MyBookDictionary.Model.User
{
    public class RolesAttribute : Attribute
    {
        public string[] Roles { get; set; }

        public RolesAttribute(params string[] roles) => Roles = roles;

    }
}
