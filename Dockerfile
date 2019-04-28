FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /ContentPOC/out
COPY . .
CMD ["dotnet", "ContentPOC.dll"]