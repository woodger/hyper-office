# Hyper Office

###### [��������� Windows](Docs/windows.md)

����������� � �������������� `Nancy` - ��������� ��� .Net.

## ����������� �� ������ �����

### ��������� �������

�� ��������� ��������� ���������� ������������ ������� �� http://localhost:8080
��� ��������� ���������� ������� ��������� ���� `web.config` � ����� ������:

```xml
  <appSettings>
    <add key="address" value="localhost" />
    <add key="port" value="8080" />
  </appSettings>
```

### ��������� �������� ���������� (�����������)

��� ����������� ������������ �������� ������ ��� 'Web Server' �������� `TCP` �� ����� `80`, ����� ������� ������� ��� ���������� Windows. 
� `PowerShell` ���������:

```shell
New-NetFirewallRule -DisplayName 'Web Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 80
```

## API

### �������������� ��������

����������� ���� � ����������� `.doc` � `.docx` � ���-��������.
������:

```bash
curl -X POST http://localhost/api/v1/documents/convert \
  -F "file=@document.doc;type=application/msword"
```
