# BandwidthMonitorService

A .net core service that monitors bandwidth from a set list of file download urls.

Uses MongoDB as a backend data store and exposes an Api and web portal for viewing stats / reports.

## Installing dotnet core 3.1 on Raspberry Pi x64 OS

At the time of writing, the latest Arm64 version of .net core available was 3.1.302. The following instructions are to install that specific version, (Please note that these instructions install .net under the current user account which is assumed to be 'pi'.)

```
wget https://download.visualstudio.microsoft.com/download/pr/5ee48114-19bf-4a28-89b6-37cab15ec3f2/f5d1f54ca93ceb8be7d8e37029c8e0f2/dotnet-sdk-3.1.302-linux-arm64.tar.gz
sudo mkdir -p $HOME/dotnet
sudo tar zxf dotnet-sdk-3.1.302-linux-arm64.tar.gz -C $HOME/dotnet
export DOTNET_ROOT=$HOME/dotnet
export PATH=$PATH:$HOME/dotnet
```

## Publishing Compiled Binaries

```
sudo mkdir /srv/BandwidthMonitorService
sudo chown pi /srv/BandwidthMonitorService
dotnet publish -c Release -o /srv/BandwidthMonitorService
```

## Running as a service on Linux

Please refer to the following article,

https://swimburger.net/blog/dotnet/how-to-run-a-dotnet-core-console-app-as-a-service-using-systemd-on-linux

**You will find an example service unit configuration file with the source code.**