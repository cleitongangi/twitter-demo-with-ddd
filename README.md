# Twitter - Project demo using DDD
## Linkedin: https://www.linkedin.com/in/cleitongangi

### Disclaimer
This application is for study purposes only, it is not a production applicatio

## Instructions to run the project
### Softwares required
- Visual Studio 2022
- .Net 6
- Docker  

## Architecture
 - Responsibility separation concerns, SOLID, YAGNI and Clean Code
 - DDD - Domain Driven Design (Layers and Domain Model Pattern)
 - Repository
 - IoC

## Technologies used
 - .Net 6
 - Docker
 - Dapper
 - Entity Framework Core 6
 - SQL Server 2019 
 - Full-text search
 - AutoMapper
 - FluentValidation
 - Swagger
 - XUnit
 - Unit Test
 - Integration Test
 
## Instructions to run the project
### Softwares required
- Visual Studio 2022
- .Net 6
- Docker

### Run
Run through Visual Studio selecting Docker-compose.
or 
Execute the following command in powershell inside the "Cleiton-Gangi-2-5-web-back-end\docker" folder:
> docker-compose  -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-posterr-cg --ansi never up -d --force-recreate --remove-orphans
> Access the API using the URL: http://localhost:54904/swagger/index.html

If you need down the environment, you can use the following comand:
> docker-compose  -f "docker-compose.yml" -f "docker-compose.override.yml" -p dockercompose-posterr-cg down

### Note
 - I hard-coded userId in method "AppControllerBase.GetAuthenticatedUserId", to test is possible changed it. But if you change it to 2, Integration Tests will fail because integration tests is prepared to use the user 1 and interact with user 2. This could be improved by making a mock of this class, but I didn't think it was necessary for this assessment.

## Documentation
### Database
#### PostTypes
Stores the post type, like Post, Repost or Quote. Posts has FK to PostTypes.
#### Posts
Stores the posts, quote posts and reposts. When is quote or a repost, the ParentId column store the reference to related Post.
#### User
Stores the users informations and stores the user metrics (FollowersCount, FollowingCount and PostsCount). It would be better if these metrics were in a separate table, I commented this in self-critique section.
#### UserFollowing
Stores the record of followings.

### Endpoints
Endpoints are accessible through swagger for easy testing and understanding. 
The names and parameters are pretty clear, but follow a brief about each endpoint.
#### GET /api/Feed/Posts
To use in home page feed to return all posts.
#### GET /api/Feed/Posts/Following
To use in home page feed to return just posts by those you follow.
#### POST /api/Posts/New
Allows create a new post or quote a post. To quote a post, is required fill the optional parameter "quotePostId" with parent post id.
#### POST /api/Posts/Repost
Allows repost a post.
#### GET /api/Users/{userId}
To use in profile page to get the user informations.
#### POST /api/Users/Following
Allows follow a new user.
#### DELETE /api/Users/Following{targetUserId}
Allows unfollow a user.
#### GET /api/Users/{userId}/Posts
To use in profile page to get the posts from a user.
#### GET /api/Posts/Search
To search a post or a quote post.

## Self-critique & Scaling
### Improvements
  - Set a better standard for returning API errors. For example, NotFound return a json in a differente way that BadRequest. They could return errors in the same json structure.
  - In pagination returns, could returning the url to fetch the next and previous pages. 
  - In this project it would be better to use mongoDB or cosmoDB as database.
  - Run Integration Tests in a docker image to allow test the search with Full-text Search. As I'm using the localDb, was not possible write tests for Search funcionality because localDb don't support Full-text Search.  
  - I could have used specifications pattern to reuse validation rules without duplicating them in different Validations.  
  - I stored the user metrics (number of posts, number of folling and number of folowers in Users table). I decided do this way, because as the count of this information will be constant, it is more performatic to get this from a table than count this informations every time from Posts and UserFollowing table, because these tables will increase a lot with time. But the improvement that can be made is to separate these quantity columns in another metric table called UserMetrics. It is better to separate to don't generate concurrency in the readings of Users table with updates of these metrics, since the updates lock the table exclusively.
### Scaling 
  The main bottleneck will be the database. A better aproach is use mongoDB as Database or Azure CosmoDB. The advantage of CosmoDB over SQL server is that it is easier to scale, CosmoDB allow easily replicate the Database around de World and Azure assure a SLA of single-digit millisecond response time. 
  If wanted continues using SQL Server, is possible setup High Availability to create other read-only copies of Database, and change application to implement CQRS technique, to write in main Database and read from copies. 
  To scale up the API and front-end, is possible create a load balance to increase the performance. Is possible do this using for example Azure Application Gateway.