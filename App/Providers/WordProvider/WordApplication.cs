using System;
using Word = Microsoft.Office.Interop.Word;

namespace HyperOffice.App.Providers
{
  class WordApplication
  {
    private Word.Application Application;

    public WordApplication()
    {
      this.Application = new Word.Application
      {
        Visible = true,
        DisplayAlerts = Word.WdAlertLevel.wdAlertsNone
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
