# Hyper Office

###### [Настройка Windows](docs/windows.md)

Разработано на языке `C#` на платформе .NET Framework 4.7 с использованием фреймворка `Nancy 2.0`.

## Руководство по началу работ

Для изменения параметров запуска поправьте директиву `<appSettings>` `web.config` в корне проета:

| Ключ | Значение | Описание |
|------|----------|----------|
| `env` | development | Устанавливает режим запуска приложения. Выбрать одно из потдерживаемых значений набора сред `DTAP` |
| `host` | localhost | Для запуска приложения используется имя хоста или IP-адрес |
| `port` | 8080 | Запустит TCP-сервер, прослушивающий соединения на предоставленном хосте |
| `page` | word.html | Имя страницы для преобразования в `html` |

### Настройка параметров запуска

По умолчанию запускает приложение обрабатывает запросы на http://localhost:8080.
Для изменения параметров запуска поправьте файл `web.config` в корне проета:

```xml
  <appSettings>
    <add key="host" value="localhost" />
    <add key="port" value="8080" />
  </appSettings>
```

#### Переменные окружения

Набор сред, используемых для `DTAP`:

- [x] development
- [ ] testing
- [ ] acceptance
- [x] production

Задать требуемую переменную среды:

```xml
  <appSettings>
    <add key="env" value="development" />
  </appSettings>
```

### Разрешить входящие соединения (опционально)

Позвольте обрабатывать входящий трафик 'Web Server' протокол `TCP` порт `8080`.
Для этого нужно добавить правило в Брандмауэр Windows.

В `PowerShell` выполните:

```shell
New-NetFirewallRule -DisplayName 'Web Server' -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 8080
```

### Установить зависимости

В `PowerShell` выполните:

```bash
Get-Package
```

### Сборка проекта

**Начало изучения**

https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build

В `PowerShell` выполните:

```bash
dotnet build
```

## API

### Конвертировать документ в HTML

Преобразует файл с расширением `.doc` и `.docx` в веб-страницу.

Запрос с использованием интерфейса `curl`:

```bash
curl -X POST http://localhost/api/v1/documents/convert \
  -F "file=@document.doc;type=application/msword"
```

Обязательный параметр `type`, один из:

- application/msword
- application/vnd.openxmlformats-officedocument.wordprocessingml.document

Возвращаемое значение: `Binary` - MIME-type `application/zip`.
Содержимое файла:

- `word.html` страница в формате `html` как указано в `web.config`.
- `word.files` директория, содержащая перечисления ресурсов, необходимые для отображения `word.html`.

При неверном запросе вернет статус-код `400`.

### Получить снимки страниц документа

Запрос с использованием интерфейса `curl`:

```bash
curl -X POST http://localhost/api/v1/documents/snapshot \
  -F "file=@document.doc;type=application/msword"
```

Возвращаемое значение: `Binary` - MIME-type `application/zip`.
Содержимое возвращаемого файла `zip` состоит из списка изображений в формате `.png`.
Имя файла изображения соответствует номерам страниц.

В случае неверного запроса вернет статус-код `400`.
