using System.Collections.Generic;

namespace OidcApp.Models.Entities
{
    public class SocialAuthenticationDefaults
    {
        public const string AuthenticationScheme = "External";
    }

    public static class OidcProviderType
    {
        public const string Google = "google";
        public const string Facebook = "facebook";
        public const string Twitter = "twitter";
        public const string Legacy = "legacy";
    }

    public class OidcProviders
    {
        public List<OidcProvider> Providers { get; set; }
    }

    public class OidcProvider
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}