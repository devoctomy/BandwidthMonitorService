[![Build Status](https://travis-ci.org/devoctomy/BandwidthMonitorService.svg?branch=master)](https://travis-ci.org/devoctomy/BandwidthMonitorService)
[![codecov](https://codecov.io/gh/devoctomy/BandwidthMonitorService/branch/master/graph/badge.svg)](https://codecov.io/gh/devoctomy/BandwidthMonitorService)

# BandwidthMonitorService

A .net core service that monitors bandwidth from a set list of file download urls.

Uses MongoDB as a backend data store and exposes an Api and web portal for viewing stats / reports.

## Running via Docker Compose via Visual Studio

Firstly you will have to create the network via the command line

```
docker network create -d bridge my-network
```

Then you should be able to launch the docker-compose project within Visual Studio.

## Running via Docker Compose via Arm64 Debian Linux

Starting from within the source folder, run the following,

```
sudo docker network create -d bridge my-network
cd Docker
sudo docker-compose -f docker-compose_arm64.yml -f docker-compose_arm64.override.yml up -d
```