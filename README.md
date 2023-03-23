## Sample informal proposed Redis as PubSub on a Docker Compose C# Microservices
An implementation sample, prototyping a solution based on:

* Web App (aspnetcore) with embedded React based form
* Administrative Console (blazor) - integrated into Web App
* Docker Compose
* Redis for pub-sub
* Web API
* Workers

Flow

* (anon) visitor will post a message on a board at the Web App level. Will get a posting ok confirmation but the message will remain hidden until review.
* The administrator will review the message looking for different parameters. If the post passes all filters, will get published.

All calls will be async...
We'll use Messaging Req/Resp dirty implementation as a fast-code approach..

### Run
in a Terminal, head (`cd` to the project root dir, whereever you have cloned/copied it)
```
cd src
docker compose -f docker-compose.override.yml -f docker-compose.yml up --build
```
then:
* `CTRL + c` to end
* `docker compose down` to remove containers from docker (full cleanup)