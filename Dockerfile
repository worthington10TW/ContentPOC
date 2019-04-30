FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY publish .
CMD ["ASPNETCORE_URLS=http://*:$PORT dotnet ContentPOC.dll"]