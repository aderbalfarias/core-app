namespace CoreApp.Api.Options.Authorization
{
    public class AuthenticationOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string TokenEndpoint { get; set; }
    }
}
