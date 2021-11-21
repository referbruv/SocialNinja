# Social Ninja - ASP.NET Core Boilerplate

SocialNinja is a boilerplate solution, built to demonstrate social authentication and how we can handle the user data obtained as a result of these logins in our application using ASP.NET Core (.NET 6)

# How do Social Logins work?

Social Login providers implement user authentication by means of OAuth2 protocol. OAuth2 or OpenAuthentication2 is an opensource authentication protocol, which facilitates authenticating a user over an authentication provider for a requesting party and then securely exchanging user identity, without having the need for user credentials. ASP.NET Core provides seamless integration of this by means of its built-in Authorization middleware and on top of it provides Identity middlewares exclusive for popular Identity Providers such as Google, Facebook, Twitter, GitHub etc.

# Technologies

* ASP.NET Core (.NET 6)
* Entity Framework Core (EFCore 6)
* MS Authentication Plugin for Google
* MS Authentication Plugin for Facebook
* SQLite

# About the Boilerplate

This boilerplate is a perfect starter for developers looking to implement OData. The solution offers the following:

1. Onion Architecture with defined layers for API, Persistence, Contracts and Migrations
2. Implemented code for UnitOfWork with Repository
3. Preconfigured Entity Framework Core migrations with SQLite
4. Ready to use settings for Google and Facebook logins
5. Ready to use and customizable Login page

# Getting Started

To get started, follow the below steps:

1. Install .NET 6 SDK
2. Clone the Solution into your Local Directory
3. Navigate to the SocialNinja.Web directory
4. Update the ClientId/ClientSecret for Google, Facebook in appsettings.json
5. Run the solution

[Application Home](assets/homepage.png?raw=true "homepage")
[Login](assets/login.png?raw=true "login")
[LoggedIn Home](assets/loggedhome.png?raw=true "loggedIn Home")
[Claims](assets/claims.png?raw=true "claims")

Read the complete article to learn more:

https://referbruv.com/blog/posts/social-authentication-in-aspnet-core-getting-started

Leave a Star if you find the solution useful. For more detailed articles and how-to guides, visit https://referbruv.com

<a href="https://www.buymeacoffee.com/referbruv" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>
