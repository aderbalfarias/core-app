using System.ComponentModel.DataAnnotations;

namespace CoreApp.Api.Options.Authorization
{
    public class ApplicationOptions
    {
        [Required] public AuthenticationOptions Authentication { get; set; }
        [Required] public OidcAuthorizationServerOptions OidcAuthorizationServer { get; set; }
    }
}
