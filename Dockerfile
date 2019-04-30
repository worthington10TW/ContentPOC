FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY . .
RUN dotnet publish -c Release -o ./out 
WORKDIR /out