#!/bin/bash

sleep 10 && curl --retry 2 --retry-delay 10 -v http://apihost:5000/healthcheck

dotnet test -xml /app/output/apirun.xml

BOMBED=$?

xsltproc --output /app/output/apiresults.xml --stringparam use.extensions 0 /app/xunit-to-junit.xslt /app/output/apirun.xml

exit $BOMBED