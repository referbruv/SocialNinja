namespace SocialNinja.Contracts
{
    public class OidcProvider
    {
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LoginUrl { get; set; }
        public string Icon { get; set; }
        public string CssClass { get; set; }
        public string ButtonText { get; set; }
        public bool IsEnabled { get; set; }
    }
}