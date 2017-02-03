#!/bin/bash


# Application Settings
VERSION='0.0.1'


# Create application environment file
touch envAPI;
touch envJob

# Add environment
if [ $CIRCLE_BRANCH = 'dev' ];
then
 STATE="DEV"
 echo "ENVIRONMENT=dev" > envAPI
 echo "ENVIRONMENT=dev" > envJob
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_DEV"" >> envAPI
 echo "TOKEN="$JOB_SHIPMENT_BUILD_TOKEN_DEV"" >> envJob
fi;

if [ $CIRCLE_BRANCH = 'rc' ];
then
 STATE="QA" 
 echo "ENVIRONMENT=qa" > envAPI
 echo "ENVIRONMENT=qa" > envJob
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_QA"" >> envAPI
 echo "TOKEN="$JOB_SHIPMENT_BUILD_TOKEN_QA"" >> envJob
fi;

# Add version 
VERSION=""$VERSION"."$CIRCLE_BUILD_NUM""
echo "VERSION="$VERSION"" >> envAPI
echo "VERSION="$VERSION"" >> envJob

# Add registry
echo "REGISTRY=quay.io/turner" >> envAPI
echo "REGISTRY=quay.io/turner" >> envJob

# Add container
echo "CONTAINER="$API_CONTAINER_NAME"" >> envAPI
echo "CONTAINER="$JOB_CONTAINER_NAME"" >> envJob

# Add image
echo "IMAGE=quay.io/turner/"$API_CONTAINER_NAME":"$VERSION"" >> envAPI
echo "IMAGE=quay.io/turner/"$JOB_CONTAINER_NAME":"$VERSION"" >> envJob

# Latest image
echo "LATEST=quay.io/turner/"$API_CONTAINER_NAME":latest" >> envAPI
echo "LATEST=quay.io/turner/"$JOB_CONTAINER_NAME":latest" >> envJob

# Add Shipment
echo "SHIPMENT="$API_SHIPMENT_NAME"" >> envAPI
echo "SHIPMENT="$JOB_SHIPMENT_NAME"" >> envJob