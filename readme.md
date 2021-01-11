# Hyper Office

[![License](https://img.shields.io/npm/l/express.svg)](https://github.com/woodger/hyper-office/blob/master/LICENSE)

Microsoft Office service hypervisor

###### [Windows Server](Docs/windows-server.md)

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

## API docs

### Get snapshots of document pages

Complite request. Some time later, after about 1 minute. When ready, `binary` - MIME-type `application/zip` file content will be passed to the specified `callback`. The `zip` file will contain a list of pages by numbers with the `.png` extension.

Request using the `curl` interface:

```bash
curl -X POST http://example.com/api/v1/documents/snapshot \
  -F "file=@document.doc;type=application/msword" \
  -F "callback=http://example.com/callback"
```

Return the Status Code:

- `202 Accepted` success
- `400 Bad Request` if an undefined or empty parameter is passed in `FormData`
- `415 Unsupported Media Type` if MIME type input file is not `application/msword`

## External dependencies

### Microsoft Office

Office, is a family of client software, server software, and services developed by Microsoft. Primarily used Microsoft Word and Microsoft Excel.

Download `Office` exe-file from official site https://setup.office.com/
