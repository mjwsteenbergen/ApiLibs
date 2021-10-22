# ApiLibs

ApiLibs is a library to easily implement REST-API's in CSharp in an Object Oriented way.

It is both a jumping off point for your own api and a collection of API's I am personally using. Current list of implemented API's can be found [here](https://github.com/mjwsteenbergen/ApiLibs/tree/master/ApiLibs)

## Install

Clone the repository and open with the latest version of your favorite editor and run `dotnet restore`

## Using any of the API's

### Authenticating

The library should be relatively self explanatory. Almost all of the Services will have a dedicated test that used to [test the Service](https://github.com/mjwsteenbergen/ApiLibs/blob/master/ApiLibsTest/Spotify/SpotifyTest.cs#L29) and will explain the expected workflow into authenticating. The short version is as follows:

 - Call the constructor of the Service with as few parameters as possible
 - Call the `Connect` method with the required parameters
 - Pass the token gotten from the above method and call the `ConvertToToken` method if exists. It will return an access token to be used with the Service. 

## The life of an ApiLibs request

To make development easy, the library is built up into layers.
When the end user makes a call, they will only see the final layer, which will look as follows: `service.MakeCall(stringParam1, stringParam2)`.

The second layer is the layer that you will probably need to work with. You will create a class extending the [Service class](https://github.com/mjwsteenbergen/ApiLibs/blob/master/ApiLibs/General/Service.cs). This will add the handrails and batteries you need to get your work done. These days, many API's are extensive. To enable splitting of the API, a notion of [SubService](#Subservice) is introduced. Then you start implementing the API, by using the `MakeRequest` method. For most simple request, you can just fill in all parameters. For more difficult queries, you can use the `Request` class. For request, where you expect a response, use the `MakeRequest<T>` and `Request<T>` classes. Are you expecting multiple responses? Set the `RequestHandler` property in Request.

In the next layer, `HandleRequest` handles all the other overhead needed (are you calling from a SubService? Here's where your url part is added). 

Then it is passed along to the `ICallImplementation`, the final layer. This is a generic implementation used to make the actual call to the the endpoint. By default, this library uses the great [RestSharp](https://github.com/restsharp/RestSharp) library.

### MakeRequest

MakeRequest is the method that you will probably use most of the time. It will make the request and deserialize the response automatically for you.

| Parameter | Explanation |
|:--|:--|
| T | The class we expect the response text to be |
| url | url to call |
| method | method to use when calling the endpoint |
| parameters | the list of parameters in the url to add. (See [parameters](#parameters)) |
| headers | the headers to add to the request |
| content | See [content](#content) |
| statuscode | The expected returned statuscode |


#### Content

There are 3 types of content:
 - HtmlContent
 - PlainTextContent
 - Default: Will automatically serialize the text into json and add it to the request body

#### Parameters

There are 3 types of parameters: 
 - Param (the default parameter)
 - OParam (optional parameter. Will not be added if the parameter value is null)
 - DParam (parameter with a default value)


### Subservice

In many cases, an API is so large, it warrents splitting up the original service into catagories. ApiLibs supports this in the way of SubServices.

Create a subservice like so:

```csharp
public class AlbumService : SubService<SpotifyService>
{
    public AlbumService(SpotifyService spotify) : base(spotify) { }
}
```

and initiate it in the main Service:

```csharp
public class SpotifyService : RestSharpService
{
    public SpotifyService() : base("https://api.spotify.com/v1/")
    {
        AlbumService = new AlbumService(this);
    }
}
```

A reference will be kept of the original Service and therefore each call that the SubService makes will be routed through the main Service.