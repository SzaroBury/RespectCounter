# RespectCounter (work in progress)

Simple React + ASP.NET Core Web API application that allows users to express their opinion on various public figures, their quotes and actions.

## Target main functionalities:
- Regular users can add quotes, comments, reactions and propositons for persons and activities.
- Moderators can accept propositions created by users
- Users can vote for other users propositions
- Moderators can additionaly add new public figures
- Searching activities by people tags

## To do in frontend:
- Activity component - event or quote, reactions
- Home page - latest and most reacted activities, persons
- Detailed activity page - more detailed reactions and comments
- Details of person page- description, respect, comments and activities
- Page for moderators - verifying persons and activities

## To do in backend:
- Person controller
    - ~~creating an endpoint for getting only verified persons~~
    - ~~creating an endpoint for getting all persons~~
    - ~~creating an endpoint for getting a person by id~~
    - ~~creating an endpoint for proposing a new person~~
    - creating an endpoint for veryfing a person
    - creating an endpoint for adding a reaction to a person
    - creating an endpoint for adding a comment to a person
    - creating an endpoint for adding a tag to a person
    - creating an endpoint for proposing a change in person data
    - creating an endpoint for hiding a person
- To understand and implement .NET Identity
- MockRepo
- Activity and Comment controllers
    - write down the list of enpoints to implement
- Custom user class
- Pictures: for user avatars, for person, for activities

## To run the app:
- dotnet-ef update datebase
- dotnet run
- npm start

## Technicalities:
- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- MediatR 12.4.1
- MS SQL Server
- React 18.2.0
- React-Bootstrap 2.8.0

## Entities:
- Person
- Activity
- Comment
- Reaction
- Tag
  
![image](https://github.com/SzaroBury/PublicFigures/assets/37550354/c066d9c8-13db-40f7-855b-50c6465b85c6)
