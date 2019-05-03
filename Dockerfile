FROM microsoft/dotnet:2.2-sdk AS build
COPY publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ContentPOC.dll