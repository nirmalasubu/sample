#!/bin/bash


# Application Settings
VERSION='1.0.0'


# Create application environment file
touch envAPI;
touch envJob;
touch envWeb;

# Add environment
if [ $CIRCLE_BRANCH = 'dev' ];
then
 STATE="DEV"
 echo "ENVIRONMENT=dev" > envAPI
 echo "ENVIRONMENT=dev" > envJob
 echo "ENVIRONMENT=dev" > envWeb
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_DEV"" >> envAPI
 echo "TOKEN="$JOB_SHIPMENT_BUILD_TOKEN_DEV"" >> envJob
 echo "TOKEN="$WEBAPP_SHIPMENT_BUILD_TOKEN_DEV"" >> envWeb
fi;

if [ $CIRCLE_BRANCH = 'sandbox' ];
then
 STATE="SANDBOX"
 echo "ENVIRONMENT=sandbox" > envAPI
 echo "ENVIRONMENT=sandbox" > envJob
 echo "ENVIRONMENT=sandbox" > envWeb
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_SANDBOX"" >> envAPI
 echo "TOKEN="$JOB_SHIPMENT_BUILD_TOKEN_SANDBOX"" >> envJob
 echo "TOKEN="$WEBAPP_SHIPMENT_BUILD_TOKEN_SANDBOX"" >> envWeb
fi;

if [ $CIRCLE_BRANCH = 'rc' ];
then
 STATE="QA" 
 echo "ENVIRONMENT=qa" > envAPI
 echo "ENVIRONMENT=qa" > envJob
 echo "ENVIRONMENT=qa" > envWeb
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_QA"" >> envAPI
 echo "TOKEN="$JOB_SHIPMENT_BUILD_TOKEN_QA"" >> envJob
 echo "TOKEN="$WEBAPP_SHIPMENT_BUILD_TOKEN_QA"" >> envWeb
fi;

# Add version 
VERSION=""$VERSION"."$CIRCLE_BUILD_NUM""
echo "VERSION="$VERSION"" >> envAPI
echo "VERSION="$VERSION"" >> envJob
echo "VERSION="$VERSION"" >> envWeb

# Add registry
echo "REGISTRY=quay.io/turner" >> envAPI
echo "REGISTRY=quay.io/turner" >> envJob
echo "REGISTRY=quay.io/turner" >> envWeb

# Add container
echo "CONTAINER="$API_CONTAINER_NAME"" >> envAPI
echo "CONTAINER="$JOB_CONTAINER_NAME"" >> envJob
echo "CONTAINER="$WEBAPP_CONTAINER_NAME"" >> envWeb

# Add image
echo "IMAGE=quay.io/turner/"$API_CONTAINER_NAME":"$VERSION"" >> envAPI
echo "IMAGE=quay.io/turner/"$JOB_CONTAINER_NAME":"$VERSION"" >> envJob
echo "IMAGE=quay.io/turner/"$WEBAPP_CONTAINER_NAME":"$VERSION"" >> envWeb

# Latest image
echo "LATEST=quay.io/turner/"$API_CONTAINER_NAME":latest" >> envAPI
echo "LATEST=quay.io/turner/"$JOB_CONTAINER_NAME":latest" >> envJob
echo "LATEST=quay.io/turner/"$WEBAPP_CONTAINER_NAME":latest" >> envWeb

# Add Shipment
echo "SHIPMENT="$API_SHIPMENT_NAME"" >> envAPI
echo "SHIPMENT="$JOB_SHIPMENT_NAME"" >> envJob
echo "SHIPMENT="$WEBAPP_SHIPMENT_NAME"" >> envWeb