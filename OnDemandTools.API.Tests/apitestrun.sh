#!/bin/bash

dotnet test -xml /app/output/apirun.xml

xsltproc --output /app/output/apiresults.xml --stringparam use.extensions 0 /app/xunit-to-junit.xslt /app/output/apirun.xml