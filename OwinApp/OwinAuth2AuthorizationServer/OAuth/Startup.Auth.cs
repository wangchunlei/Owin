using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace OwinOAuth2AuthorizationServer
{
    public partial class Startup
    {
        private readonly ConcurrentDictionary<string, string> _authenticationCodes =
            new ConcurrentDictionary<string, string>(StringComparer.Ordinal);

        public void ConfigureAuth(IAppBuilder app)
        {
            //Setup Authorization Server
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString("/OAuth/Authorize"),
                TokenEndpointPath = new PathString("/OAuth/Token"),
                ApplicationCanDisplayErrors = true,
#if DEBUG
                AllowInsecureHttp = true,
#endif
                //Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientRedirectUri = context =>
                    {
                        if (context.ClientId == "")
                        {
                            context.Validated("");
                        }
                        return Task.FromResult(0);
                    },
                    OnValidateClientAuthentication = context =>
                    {
                        string clientId, clientSecret;
                        if (context.TryGetBasicCredentials(out clientId, out clientSecret) ||
                            context.TryGetFormCredentials(out clientId, out clientSecret))
                        {
                            //if (clientId == Clients.Client1.Id && clientSecret == Clients.Client1.Secret)
                            {
                                context.Validated();
                            }
                            //else if (clientId == Clients.Client2.Id && clientSecret == Clients.Client2.Secret)
                            {
                                context.Validated();
                            }
                        }
                        return Task.FromResult(0);
                    },
                    OnGrantResourceOwnerCredentials = context =>
                    {
                        var identity =
                            new ClaimsIdentity(new GenericIdentity(context.UserName, OAuthDefaults.AuthenticationType),
                                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));

                        context.Validated(identity);
                        return Task.FromResult(0);
                    },
                    OnGrantClientCredentials = context =>
                    {
                        var identity =
                            new ClaimsIdentity(new GenericIdentity(context.ClientId, OAuthDefaults.AuthenticationType),
                                context.Scope.Select(x => new Claim("urn:oauth:scope", x)));

                        context.Validated(identity);
                        return Task.FromResult(0);
                    }
                },

                //Authorization code provider which creates and receives authorization code
                AuthorizationCodeProvider = new AuthenticationTokenProvider
                {
                    OnCreate = context =>
                    {
                        context.SetToken(Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n"));
                        _authenticationCodes[context.Token] = context.SerializeTicket();
                    },
                    OnReceive = context =>
                    {
                        string value;
                        if (_authenticationCodes.TryRemove(context.Token, out value))
                        {
                            context.DeserializeTicket(value);
                        }
                    }
                },

                //Refresh token provider which creates and receives referesh token
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = context =>
                    {
                        context.SetToken(context.SerializeTicket());
                    },
                    OnReceive = context =>
                    {
                        context.DeserializeTicket(context.Token);
                    }
                }
            });
        }
    }
}