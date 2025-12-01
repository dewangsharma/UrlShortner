URL Shortner application.
Inspired by the coding challenge of John Cricket https://codingchallenges.fyi/

- Run the docker compose for dev on local machine 
"docker compose -f docker-compose.dev.yml up --build"

- Run EFCore migration 
Navigate to the src folder and run the command
"dotnet ef migrations add InitialCreate --project UrlShortner.Infrastructure --startup-project UrlShortner.RestApi"

- Localstack for local AWS testing
Run localstack in docker with the following command:
"docker run -d --name localstack -p 4566:4566 -p 4571:4571 -e SERVICES="kms,secretsmanager,s3,sqs,sns" -e DEBUG=1 localstack/localstack"
Create KMS Key 
- "aws --endpoint-url=http://localhost:4566 kms create-key --profile localstack"