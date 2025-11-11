URL Shortner application.
Inspired by the coding challenge of John Cricket https://codingchallenges.fyi/

- Run the docker compose for dev on local machine 
"docker compose -f docker-compose.dev.yml up --build"

- Run EFCore migration 
Navigate to the src folder and run the command
"dotnet ef migrations add InitialCreate --project UrlShortner.Infrastructure --startup-project UrlShortner.RestApi"
