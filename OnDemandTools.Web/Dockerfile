FROM microsoft/dotnet:1.1.0-sdk-projectjson

## Install Node 
RUN wget http://nodejs.org/dist/v6.5.0/node-v6.5.0-linux-x64.tar.gz
RUN tar -C /usr/local --strip-components 1 -xzf node-v6.5.0-linux-x64.tar.gz 
RUN cd $(npm root -g)/npm && npm install fs-extra && sed -i -e s/graceful-fs/fs-extra/ -e s/fs.rename/fs.move/ ./lib/utils/rename.js

## Prepare environment for portal
COPY . /app
WORKDIR /app
RUN ["dotnet", "restore"]

## Switch to working directory
WORKDIR /app/OnDemandTools.Web

RUN npm run portal
RUN ["dotnet", "build"]

EXPOSE 5000/tcp

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
