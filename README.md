# ApiLibs

ApiLibs is a library created to give you an easy Object Oriented way to use rest-api's and to easily incorporate new ones.

## Install

Clone the repository and open with the latest version of visual studio and run `dotnet restore`

## Using any of the API's

### Authenticating

The library should be relatively self explanatory. Almost all of the Services will have a dedicated test that used to [test the Service](https://github.com/mjwsteenbergen/ApiLibs/blob/master/ApiLibsTest/Spotify/SpotifyTest.cs#L29) and will explain the expected workflow into authenticating. The short version is as follows:

 - Call the constructor of the Service with as few parameters as possible
 - Call the `Connect` method with the required parameters
 - Pass the token gotten from the above method and call the `ConvertToToken` method if exists. It will return an access token to be used with the Service. 

## Architecture

The library is built of 3 layers:

- The actual api that the user will use
- A service layer which provides an adapter to the lowest layer and the higher layer
- RestSharp which enables use make the call to the internet

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
public class AlbumService : SubService
{
    public AlbumService(SpotifyService spotify) : base(spotify) { }
}
```

and initiate it in the main Service:

```csharp
public class SpotifyService : Service
{
    public SpotifyService() : base("https://api.spotify.com/v1/")
    {
        AlbumService = new AlbumService(this);
    }
}
```

A reference will be kept of the original Service and therefore each call that the SubService makes will be routed through the main Service.

## How to connect your own rest-service

As an example how to extend the service to include your own rest-api, we are going to implement anapioficeandfire.com into our api
1. Create a new folder inside the ApiLibs folder called Iceandfire.
2. Create a class called `IceandfireService`, which is going to be the one we make calls from. Make sure this class extends the base class `Service`.
3. Make another class in the folder called `ReturnClasses`
4. Create a constructor inside your service, where we call the SetUp method from the base class with "https://anapioficeandfire.com/api/" as argument. This will be our parent url where will be basing the rest of our requests on.
5. The api asks us to set a header to make sure we have the correct version of the api. To do this we add the method AddStandardHeader with a new Param, with as arguments "Accept", the name of the header and "`1`" as argument
6. Ok we are all set. We can start to implement our first method, looking for books
7. create a new async method called `GetBooks()`
8. inside of that call `var x = await HandleRequest("books")`
9. Set a break point at this line. In the main method of your application call the GetBooks() method and Debug your application
10. x should be correct and x.Content should have some JSON in it. Copy the JSON and stop the application
11. Go to the ReturnClasses class and click on Edit -> Paste Special -> Paste JSON as classes
12. Go to your GetBooks() method and change the line we created at step to `return MakeRequest<List<Books>("books");` and change the return type of `GetBooks()` accordingly
13. You have now successfully connected to the api.