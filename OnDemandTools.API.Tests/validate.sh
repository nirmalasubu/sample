#!/bin/bash
containerName="$(docker ps --filter="name=api-test" -a --format '{{.Names}}')"

containerStatus="$(docker inspect -f {{.State.ExitCode}} $containerName)"
if [ "$containerStatus" != "0" ];
then
  exit 1
else
  exit 0
fi