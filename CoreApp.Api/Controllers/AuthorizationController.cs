using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using CoreApp.Api.Options.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Mvc.Internal;
using OpenIddict.Server;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreApp.Api.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthorizationController : ControllerBase
    {
        private readonly OIDCAuthorizationServerOptions _serverOptions;
        private readonly OpenIddictApplicationManager<OpenIddictApplication> _applicationManager;

        public AuthorizationController
        (
            OIDCAuthorizationServerOptions options,
            OpenIddictApplicationManager<OpenIddictApplication> applicationManager
        )
        {
            _serverOptions = options;
            _applicationManager = applicationManager;
        }

        [HttpPost]
        [Route("/connect/token")]
        public async Task<IActionResult> Exchange([ModelBinder(typeof(OpenIddictMvcBinder))]
            OpenIdConnectRequest request)
        {
            if (request.IsClientCredentialsGrantType())
            {
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var application = await _applicationManager
                    .FindByClientIdAsync(request.ClientId, HttpContext.RequestAborted);

                if (application == null)
                    return BadRequest(new OpenIdConnectResponse
                    {
                        Error = OpenIdConnectConstants.Errors.InvalidClient,
                        ErrorDescription = "The client application was not found in the database."
                    });

                // Create a new authentication ticket.
                var ticket = CreateTicket(application);

                return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
            }

            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                ErrorDescription = "The specified grant type is not supported."
            });
        }

        private AuthenticationTicket CreateTicket(OpenIddictApplication application)
        {
            // Create a new ClaimsIdentity containing the claims that
            // will be used to create an id_token, a token or a code.
            var identity = new ClaimsIdentity(OpenIddictServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name, OpenIdConnectConstants.Claims.Role);

            // Use the client_id as the subject identifier.
            identity.AddClaim(OpenIdConnectConstants.Claims.Subject, application.ClientId,
                OpenIdConnectConstants.Destinations.AccessToken,
                OpenIdConnectConstants.Destinations.IdentityToken);

            var clients = _serverOptions.Clients
                .Where(x => x.ClientId == application.ClientId)
                .ToList();

            foreach (var client in clients)
            {
                var roles = client.Roles;

                if (roles != null)
                    foreach (var role in roles)
                        identity.AddClaim(OpenIdConnectConstants.Claims.Role,
                            role, OpenIdConnectConstants.Destinations.AccessToken,
                            OpenIdConnectConstants.Destinations.IdentityToken);
            }

            // Create a new authentication ticket holding the user identity.
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity),
                new AuthenticationProperties(), OpenIddictServerDefaults.AuthenticationScheme);

            ticket.SetResources(_serverOptions.Audience);

            return ticket;
        }
    }
}