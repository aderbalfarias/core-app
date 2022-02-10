namespace CoreApp.Api.Options.Authorization
{
    public class Client
    {
        /// <summary>
        /// Client identifier associated with the application
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret associated with the application
        /// Note: Depending on the application manager used when creating it,
        /// this property may be hashed or encrypted for security reasons
        /// </summary>
        public string ClientSecret { get; set; }

        public List<string> Roles { get; set; }

        //public OpenIddictApplicationDescriptor[] ApplicationDescriptors { get; set; }
    }
}