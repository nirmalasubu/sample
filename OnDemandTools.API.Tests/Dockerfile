FROM microsoft/dotnet:1.1.0-sdk-projectjson

# inject hostname needed within api test container
ENV APIEndPoint http://apihost:5000

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

WORKDIR /app/OnDemandTools.API.Tests

RUN ["dotnet", "build"]

CMD ["dotnet", "test"]