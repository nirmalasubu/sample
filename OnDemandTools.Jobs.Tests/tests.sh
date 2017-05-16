#!/bin/bash

dotnet test -xml /app/output/jobrun.xml

BOMBED=$?

xsltproc --output /app/output/jobresults.xml --stringparam use.extensions 0 /app/xunit-to-junit.xslt /app/output/jobrun.xml

exit $BOMBED