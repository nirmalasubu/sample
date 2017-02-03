FROM microsoft/dotnet:1.1.0-sdk-projectjson

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

WORKDIR /app/OnDemandTools.API.Tests

RUN ["dotnet", "build"]

CMD ["dotnet", "test"]