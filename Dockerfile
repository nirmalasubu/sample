FROM microsoft/dotnet:latest

COPY . /app

WORKDIR /app

dotnet -v 1>/dev/null 2>/dev/null

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]