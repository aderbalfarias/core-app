using CoreApp.Api.Filters;
using CoreApp.Api.Options.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CoreApp.Api.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        ///     Executes the specific action if codition parameter is true
        ///     Used to conditionally add actions to the servives
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <param name="condition">If it is true the action is executed</param>
        /// <param name="action">Action requested to be added to servives</param>
        /// <returns>Service collection</returns>
        public static IServiceCollection AddIf
        (
            this IServiceCollection services,
            bool condition,
            Func<IServiceCollection, IServiceCollection> action
        ) => services != null && action != null && condition
            ? action(services)
            : services;

        public static IServiceCollection AddSwagger
        (
            this IServiceCollection services,
            AuthenticationOptions authenticationOption
        )
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Version = "1.3",
                    Title = "Core App Template",
                    Description = "Solution to be used as a template for new .net 5 apis",
                    TermsOfService = new Uri("https://aderbalfarias.github.io/"),
                    Contact = new OpenApiContact
                    {
                        Name = "Aderbal Farias",
                        Email = "aderbalfarias@hotmail.com",
                        Url = new Uri("https://aderbalfarias.github.io/")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            //AuthorizationUrl = new Uri("/auth-server/connect/authorize", UriKind.Relative),
                            //TokenUrl = new Uri("/auth-server/connect/token", UriKind.Relative)
                            //TokenUrl = new Uri(authenticationOptions.TokenEndpoint),
                            TokenUrl = new Uri(authenticationOption.TokenEndpoint),
                            Scopes = new Dictionary<string, string>
                            {
                                //{ "readAccess", "Access read operations" },
                                //{ "writeAccess", "Access write operations" }
                            }
                        }
                    }
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new string[] { }
                    }
                });

                c.DocumentFilter<SwaggerDocumentFilter>();
            });

            return services;
        }
    }
}