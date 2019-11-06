# Hyper Office

###### [Настройка Windows](Docs/windows.md)

Разработано с использованием `Nancy` - фреймворк для .Net.

## Руководство по началу работ

По умолчанию запускает приложение обрабатывает запросы на http://localhost:8080
Для изменения параметров запуска поправьте файл `web.config` в корне проета:

```xml
  <appSettings>
    <add key="address" value="localhost" />
    <add key="port" value="8080" />
  </appSettings>
```

Создать правило для Брандмауэр Windows:

Открыть `PowerShell`:
  Пуск -> Поиск -> PowerShell

Разрешить входящий трафик для 'Web Server' протокол `TCP` порт `80`

```shell
New-NetFirewallRule -DisplayName 'Web Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 80
```

## API

### Конвертировать документ

Преобразует файл с расширением `.doc` и `.docx` в веб-страницу.

Запрос:

```bash
curl -X POST http://localhost/api/v1/documents/convert \
  -F "file=@document.doc;type=application/msword"
```
