#!/bin/bash
containerName="$(docker ps --filter="name=api-test" -a --format '{{.Names}}')"
echo $containerName

containerStatus="$(docker inspect -f {{.State.ExitCode}} $containerName)"
echo $containerStatus
if [ "$containerStatus" != "0" ];
then
  exit 1
else
  exit 0
fi	