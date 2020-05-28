namespace CoreApp.Api.Options.Authorization
{
    public class CertificateOptions
    {
        public bool AllowInvalid { get; set; }

        public string Subject { get; set; }

        public string Store { get; set; }

        public string Location { get; set; }
    }
}