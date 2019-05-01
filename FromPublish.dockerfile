FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY ContentPOC/out .
RUN ls
CMD ["ASPNETCORE_URLS=http://*:$PORT dotnet ContentPOC.dll"] 
