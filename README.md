# Vision Health Care Assessment

## Description
A small web api written in Visual Studio 2022 using .NET 6. Products can be retrieved, created, updated, deleted and imported through this api. 

## Using the api
When running the project, swagger will be reachable via https://localhost:7118/swagger/index.html. Included in the project is a "requests.http" file, which can be used with the [REST Client extension](https://marketplace.visualstudio.com/items?itemName=humao.rest-client) in visual studio code, the file are sample calls for all implemented methods. <br/>
The database should be automatically created in the "Data" folder to be used in the API.


## What's there
- CRUD operations via the API.
- Import from json

## What's left to do
- Logging could be implemented, get insights into the usage, warnings etc.
- The json path for import is pretty much hardcoded, ideally a file/url should be passed, which would require more security/validation.
- GET Products endpoint could use pagination with limit/offset/continuationToken and more advanced filtering.
- A different DBMS might have been a better choice, sqlite is easy/lightweight but doesnt seem as mature as others. 
- More tests could be implemented
- Integration testing, seeing end-to-end that a product can be created/imported and retrieved from the db/api.

