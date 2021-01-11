using System;
using Word = Microsoft.Office.Interop.Word;

namespace HyperOffice.App.Providers
{
  class WordApplication
  {
    private Word.Application Application;

    public WordApplication()
    {
      Word.WdAlertLevel wdAlertLevel = Word.WdAlertLevel.wdAlertsMessageBox;

      this.Application = new Word.Application
      {
        Visible = true,
        DisplayAlerts = wdAlertLevel
      };
    }

    public WordDocument OpenDocument(object FileName)
    {
      object ConfirmConversions = false;
      object ReadOnly = false;
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
