# About

POC for content... 
Andrew can you update? (pretty please)

# Publishing

The dotnet app can be published using the following command (note that the location will be in root/publish)

run from root
 `dotnet publish -c Release -o ../publish  && chmod +x publish `

# Hosting

The app is hosted on Heroku.
Instructions to create, host, release

1. Install the Heroku CLI <https://devcenter.heroku.com/articles/heroku-cli#download-and-install> .
2. Login  `heroku login`
3. Container login `heroku container:login`
4. Create an app  `heroku apps:create {appName} --region eu`
5. Push an app (from Dockerfile)  `heroku container:push web -a {appName}`
6. Release an app  `heroku container:release web -a {appName}`
7. View release `heroku container:release web -a {appName}`
8. View logs  `heroku logs --tail -a {appName}`

Heroku dynamically picks the port to use for the app, this means we need to assign our app to this port.

The port will be assigned to the  `$PORT` variable, therefore available at run time.
To set the port at runtime in dotnet projects assign the following value
 `ASPNETCORE_URLS=http://*:$PORT`

*Note: this must be at runtime (not build), therefore will be inline with the CMD arguemnts*
