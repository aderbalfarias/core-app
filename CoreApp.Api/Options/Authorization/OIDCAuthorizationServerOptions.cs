namespace CoreApp.Api.Options.Authorization
{
    public class OidcAuthorizationServerOptions
    {
        public Client[] Clients { get; set; }

        public CertificateOptions SigningCertificate { get; set; }

        public int AccessTokenExpiration { get; set; }

        public string Audience { get; set; }
    }
}
