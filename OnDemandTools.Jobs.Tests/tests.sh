#!/bin/bash

sleep 10 && curl --retry 2 --retry-delay 10 -v http://apihost:5000/healthcheck
sleep 10 && curl --retry 2 --retry-delay 10 -v http://jobhost:5000/healthcheck

dotnet test -xml /app/output/jobrun.xml

BOMBED=$?

xsltproc --output /app/output/jobresults.xml --stringparam use.extensions 0 /app/xunit-to-junit.xslt /app/output/jobrun.xml

exit $BOMBED