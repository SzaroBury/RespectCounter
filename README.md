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
- Detail activity component - more detailed reactions and comments
- Home page - latest and most reacted posts
- Details of person - description, comments and activities

## To do in backend:
- All controlers
- CQRS pattern

## To run the app:
- dotnet-ef update datebase
- dotnet run
- npm start

## Technicalities:
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
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
