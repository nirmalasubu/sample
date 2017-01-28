#!/bin/bash


# Application Settings
VERSION='0.0.1'


# Create application environment file
touch envAPI;
touch envDeporter

# Add environment
if [ $CIRCLE_BRANCH = 'dev' ];
then
 STATE="DEV"
 echo "ENVIRONMENT=dev" > envAPI
 echo "ENVIRONMENT=dev" > envDeporter
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_DEV"" >> envAPI
 echo "TOKEN="$DEPORTER_SHIPMENT_BUILD_TOKEN_DEV"" >> envDeporter
fi;

if [ $CIRCLE_BRANCH = 'rc' ];
then
 STATE="QA" 
 echo "ENVIRONMENT=qa" > envAPI
 echo "ENVIRONMENT=qa" > envDeporter
 echo "TOKEN="$API_SHIPMENT_BUILD_TOKEN_QA"" >> envAPI
 echo "TOKEN="$DEPORTER_SHIPMENT_BUILD_TOKEN_QA"" >> envDeporter
fi;

# Add version 
VERSION=""$VERSION"."$CIRCLE_BUILD_NUM""
echo "VERSION="$VERSION"" >> envAPI
echo "VERSION="$VERSION"" >> envDeporter

# Add registry
echo "REGISTRY=quay.io/turner" >> envAPI
echo "REGISTRY=quay.io/turner" >> envDeporter

# Add container
echo "CONTAINER="$API_CONTAINER_NAME"" >> envAPI
echo "CONTAINER="$DEPORTER_CONTAINER_NAME"" >> envDeporter

# Add image
echo "IMAGE=quay.io/turner/"$API_CONTAINER_NAME":"$VERSION"" >> envAPI
echo "IMAGE=quay.io/turner/"$DEPORTER_CONTAINER_NAME":"$VERSION"" >> envDeporter

# Latest image
echo "LATEST=quay.io/turner/"$API_CONTAINER_NAME":latest" >> envAPI
echo "LATEST=quay.io/turner/"$DEPORTER_CONTAINER_NAME":latest" >> envDeporter

# Add Shipment
echo "SHIPMENT="$API_SHIPMENT_NAME"" >> envAPI
echo "SHIPMENT="$DEPORTER_SHIPMENT_NAME"" >> envDeporter