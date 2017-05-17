#!/bin/bash

curl --retry 1 --retry-delay 2 -f http://apihost:5000/healthcheck
curl --retry 1 --retry-delay 2 -f http://jobhost:5000/healthcheck

dotnet test -xml /app/output/jobrun.xml

BOMBED=$?

xsltproc --output /app/output/jobresults.xml --stringparam use.extensions 0 /app/xunit-to-junit.xslt /app/output/jobrun.xml

exit $BOMBED