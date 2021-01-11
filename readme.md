# Hyper Office

[![License](https://img.shields.io/npm/l/express.svg)](https://github.com/woodger/hyper-office/blob/master/LICENSE)

Microsoft Office service hypervisor.

![yuml diagram](https://yuml.me/woodger/diagram/scruffy;dir:LR/class/[Document{bg:lightsteelblue}]->parse[Hypervisor],[Hypervisor]->publish[Queue{bg:yellow}],[Queue]->[SqliteDB],[Queue]<>-.->thread[Task{bg:yellowgreen}],[Task]->process[Hypervisor])

###### [Windows Server](Docs/api.md) | [Windows Server](Docs/windows-server.md)

This solution implemented in `C#` language on the .NET Framework 4.7 using the `Nancy 2.0` framework.

## Getting Started

Clone this project

```sh
git clone https://github.com/woodger/hyper-office
```

### Install dependencies

Run `PowerShell` and type:

```sh
Get-Package
```

### Build .NET

More guide (.NET Core CLI dotnet build)[https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build]

```sh
dotnet build
```

### Allow incomming conection (optional)

Allow to process incoming traffic 'Web Server' protocol 'TCP' port '8080'. To do this, add a rule to Windows Firewall.

```sh
New-NetFirewallRule -DisplayName 'Web Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 8080
```

### Usage CLI

```
Usage:
  HyperOffice.exe [COMMAND = up] [OPTIONS]

Commands:
  up               Start the Http server
  snapshot         Make screenshot in Office document

Options:
  -d               Detached mode. Run application in the background
  -q, --quiet      Quiet mode
  -p, --port       (Default: 8080) Port of listen server
  -t, --threads    (Default: 1)Numbers worker threads of Queue. Limited by the number of CPU cores
  --help           Display more information on a specific command
  --version        Display version information
```

## External dependencies

### Microsoft Office

Office, is a family of client software, server software, and services developed by Microsoft. Primarily used Microsoft Word and Microsoft Excel.

Download `Office` exe-file from official site https://setup.office.com/
