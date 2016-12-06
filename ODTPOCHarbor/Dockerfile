FROM microsoft/aspnetcore
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
COPY out .
ENTRYPOINT ["dotnet", "ODTPOCHarbor.dll"]