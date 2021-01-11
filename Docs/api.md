# API docs

## Get snapshots of document pages

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
