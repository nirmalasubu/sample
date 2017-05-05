FROM microsoft/dotnet:1.1.0-sdk-projectjson

# inject hostnames needed within job test container
ENV JobEndPoint http://jobhost:5000
ENV APIEndPoint http://apihost:5000

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

WORKDIR /app/OnDemandTools.Jobs.Tests

RUN ["dotnet", "build"]

CMD ["dotnet", "test"]
