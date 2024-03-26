using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace DotNetIdentity.Application.Core.Settings.User;

/// <summary>
/// Represents the identity configuration class.
/// </summary>
public static class IdentityConfiguration
{
    /// <summary>
    /// The api scopes list.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("DotNetIdentity.Api", "DotNetIdentity Api")
        };

    /// <summary>
    /// The identity resources list.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    /// <summary>
    /// The api resources list.
    /// </summary>
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("DotNetIdentity.Api", "DotNetIdentity Api", new []
                { JwtClaimTypes.Name})
            {
                Scopes = {"DotNetIdentity.Api"}
            }
        };

    /// <summary>
    /// The clients list.
    /// </summary>
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "DotNetIdentity.Api",
                ClientName = "DotNetIdentity.Api",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris =
                {
                    "http://localhost:7276/signin"
                },
                AllowedCorsOrigins =
                {
                    "http://localhost:7276"
                },
                PostLogoutRedirectUris =
                {
                    "http://localhost:7276/signout"
                },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "DotNetIdentity.Api"
                },
                AllowAccessTokensViaBrowser = true
            }
        };
}