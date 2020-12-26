using System;
using System.Configuration;
using Word = Microsoft.Office.Interop.Word;

namespace HyperOffice.App
{
  class WordApplication
  {
    public Word.Application Application;

    public WordApplication()
    {
      bool dev = ConfigurationManager.AppSettings.Get("env") == "development";

      Word.WdAlertLevel wdAlertLevel = dev ?
          Word.WdAlertLevel.wdAlertsMessageBox : Word.WdAlertLevel.wdAlertsNone;

      this.Application = new Word.Application
      {
        Visible = dev,
        DisplayAlerts = wdAlertLevel
      };
    }

    public WordDocument OpenDocument(object FileName)
    {
      // Открыть диалоговое окно Преобразование файла, если файл не находится в формате Microsoft Word
      object ConfirmConversions = false;

      // Открыть документ только для чтения
      object ReadOnly = false;

      // Добавить имя файла в список недавно использованных файлов
      object AddToRecentFiles = false;

      Word.Document document = this.Application.Documents.Open(
        ref FileName,
        ref ConfirmConversions,
        ref ReadOnly,
        ref AddToRecentFiles
      );

      return new WordDocument(document);
    }

    public void Quit()
    {
      this.Application.Quit();
    }
  }
}
