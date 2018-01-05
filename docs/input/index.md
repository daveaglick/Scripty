Title: NetlifySharp
---
NetlifySharp is a .NET API Client for [Netlify](https://www.netlify.com). [Source code on GitHub](https://github.com/daveaglick/NetlifySharp).

The goal is to implement [the entire Netlify API](https://www.netlify.com/docs/api/), but only the sites endpoints and a few others are implemented at the moment. Work is ongoing to support additional endpoints.

# Usage

All operations are performed through a `NetlifyClient` instance. Use [a personal access token](https://app.netlify.com/account/applications) to create the client:

```
NetlifyClient client = new NetlifyClient("123456789abcdef");
```

The `NetlifyClient` contains methods for each of the endpoints. All endpoints are asynchronous and use a fluent interface. You must call `.SendAsync()` to initiate communication with the Netlify API.

For example, to get a list of all configured sites for the account:

```
IEnumerable<Site> sites = await client.ListSites().SendAsync();
```

To create a new site:

```
Site site = await client
    .CreateSite(
        new SiteSetup(client)
        {
            Name = "mynewsite"
        })
    .SendAsync();
```

You'll notice that the `NetlifyClient` instance was required in the `SiteSetup` constructor in the above example. All models require the client to be provided when directly instantiating them. If they're created as a result of an API call (like the sites in the first example) then the client is already set. This is so every model can initiate their own API requests through the client.

For example, to delete an existing site:

```
Site deleteme = await client.GetSite("sitetodelete.netlify.com").SendAsync();
await deleteme.DeleteSite().SendAsync();
```

# Site IDs

Whenever a site ID is required you can use the UUID of the site obtained through the API or the domain of the site as shown in the above examples.

# Deploying A Site

The `UpdateSite` endpoints support supplying a directory name or `Stream`. If a directory name is provided, all the files in the directory will be zipped and sent to Netlify as a new deployment of the site. If a `Stream` is provided, it must represent a zip file. If a `Stream` is provided, the zip file stream will be sent to Netlify as a new deployment of the site.

# Errors

If something goes wrong the client will throw an `ErrorResponseException` that contains an `Error` object returned from the API (if there is one).

# Customization

You can customize every operation at the client or operation level by providing handlers.

For example, to log every request:

```
client.RequestHandler = x =>
{
    Console.WriteLine(x.Method.Method);
    Console.WriteLine(x.RequestUri.ToString());
}
```

The request handler exposes a `HttpRequestMessage` and the response handler exposes a `HttpResponseMessage`.

# Using In Cake

If you want to use NetlifySharp in [Cake](https://cakebuild.net) you'll need to use Cake.CoreClr until the .NET Framework version of Cake targets a version of the framework that's compatible with .NET Standard 1.6. You'll also need to add all of the required NetlifySharp dependencies to your build script since Cake doesn't resolve transitive dependencies:

```
#addin "System.Runtime.Serialization.Formatters"
#addin "Newtonsoft.Json"
#addin "NetlifySharp"
```

NetlifySharp itself does this to deploy the website and docs so you can check the NetlifySharp repository for an example.