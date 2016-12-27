# ApiLibs

ApiLibs is a library created to give you an easy Object Oriented way to use rest-api's and to easily incorporate new ones.

## Install

Clone the repository and open with the latest version of visual studio and run `nuget restore`

## Architecture

The library is built of 3 layers:

- The actual api that the user will use
- A service layer which provides an adapter to the lowest layer and the higher layer
- RestSharp which enables use make the call to the internet

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