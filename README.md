# RespectCounter (work in progress)

Simple .NET + React application that allows users to express their opinion on various public figures, their quotes and actions.

## Target main functionalities:
- Users can propose public figures.
- Users can add quotes/activities and link them to stored public figures.
- Users can comment and react to persons, activities and other comments.
- Users can search quotes, activities and persons by tags.
- Users can report activities, comments.
- Moderators can verify public figures, quotes and activities added by users.
- Moderators can create verified public figures, quotes and activities.
- Moderators can hide comments and activities.

## To do in frontend:
- Home page - trending and latest added activities, quotes and public figures
- Person page - list of public figures
- Page with details of an activity/quote - more detailed reactions and comments
- Page with details of a public figure - description, respect, comments and activities
- Page for moderators - verifying persons and activities

## To do in backend:
- Pictures: for user avatars, for persons, for activities
- Custom user class
- Reports

## Installation
- Clone repository: `git clone https://github.com/SzaroBury/RespectCounter.git`
- Start the API `dotnet run --project .\RespectCounter.API\`
- Start the ReactApp in another terminal `cd .\RespectCounter.ReactApp\; npm start `

## Technicalities:
- .NET 8
- ASP.NET Core Web API
- ASP.NET Core Identity
- Entity Framework Core
- MediatR 12.4.1
- MS SQL Server
- React 18.2.0
- React-Bootstrap 2.8.0

## Main entities:
- Person
- Activity
- Comment
- Reaction
- Tag

![image](RespectCounterERD.png)
