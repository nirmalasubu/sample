FROM microsoft/dotnet:latest

COPY . /app

WORKDIR /app

RUN bash dotnet.sh restore

RUN ["dotnet", "build"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
