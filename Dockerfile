FROM mcr.microsoft.com/dotnet/core/runtime:2.2
COPY ContentPOC/out .
CMD ["dotnet", "ContentPOC.dll"]