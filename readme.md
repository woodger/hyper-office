# Hyper Office

###### [��������� Windows](docs/windows.md)

����������� �� ����� `C#` �� ��������� .NET Framework 4.7 � �������������� ���������� `Nancy 2.0`.

## ����������� �� ������ �����

��� ��������� ���������� ������� ��������� ��������� `<appSettings>` `web.config` � ����� ������:

| ���� | �������� | �������� |
|------|----------|----------|
| `env` | development | ������������� ����� ������� ����������. ������� ���� �� �������������� �������� ������ ���� `DTAP` |
| `address` | localhost | ��� ������� ���������� ������������ ��� ����� ��� IP-����� |
| `port` | 80 | �������� TCP-������, �������������� ���������� �� ��������������� ����� |
| `page` | document.html | ��� �������� ��� �������������� � `html` |

### ��������� ���������� �������

�� ��������� ��������� ���������� ������������ ������� �� http://localhost:80.
��� ��������� ���������� ������� ��������� ���� `web.config` � ����� ������:

```xml
  <appSettings>
    <add key="address" value="localhost" />
    <add key="port" value="80" />
  </appSettings>
```

#### ���������� ���������

����� ����, ������������ ��� `DTAP`:

- [x] development
- [ ] testing
- [ ] acceptance
- [x] production

������ ��������� ���������� �����:

```xml
  <appSettings>
    <add key="env" value="development" />
  </appSettings>
```

### ��������� �������� ���������� (�����������)

��������� ������������ �������� ������ 'Web Server' �������� `TCP` ���� `80`.
��� ����� ����� �������� ������� � ���������� Windows. 

� `PowerShell` ���������:

```shell
New-NetFirewallRule -DisplayName 'Web Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 80
```

## API

### �������������� �������� � HTML

����������� ���� � ����������� `.doc` � `.docx` � ���-��������.

������ � �������������� ���������� `curl`:

```bash
curl -X POST http://localhost/api/v1/documents/convert \
  -F "file=@document.doc;type=application/msword"
```

������������ �������� `type`, ���� ��:

- application/msword
- application/vnd.openxmlformats-officedocument.wordprocessingml.document

������������ ��������: `Binary` - MIME-type `application/zip`.
���������� �����:

- `document.html` �������� � ������� `html` ��� ������� � `web.config`.
- `document.files` ����������, ���������� ������������ ��������, ����������� ��� ����������� `document.html`.

��� �������� ������� ������ ������-��� `400`.

### �������� ������ ������� ���������

������ � �������������� ���������� `curl`:

```bash
curl -X POST http://localhost/api/v1/documents/snapshot \
  -F "file=@document.doc;type=application/msword"
```

������������ ��������: `Binary` - MIME-type `application/zip`.
���������� ������������� ����� `zip` ������� �� ������ ����������� � ������� `.png`.
��� ����� ����������� ������������� ������� �������.

� ������ ��������� ������� ������ ������-��� `400`.