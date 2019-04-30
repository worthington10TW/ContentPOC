FROM microsoft/dotnet:sdk AS build-env

# Copy csproj and restore as distinct layers
COPY . .
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=build-env out .
ENTRYPOINT ["dotnet", "aspnetapp.dll"]