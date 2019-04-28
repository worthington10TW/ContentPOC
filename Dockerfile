FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY ContentPOC/out .
RUN ls
CMD ["dotnet", "ContentPOC.dll"]

