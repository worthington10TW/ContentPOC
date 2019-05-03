FROM microsoft/dotnet:2.2-sdk AS build
COPY out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContentPOC.dll