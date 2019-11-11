using System;
using System.IO;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Drawing.Imaging;

namespace HyperOffice.App {
  class WordDocument {
    public Word.Document Document;
    private readonly string BreakLineSymbol = System.Text.Encoding.Default.GetString(new byte[] { 11 });
    private readonly string BreakPageSymbol = System.Text.Encoding.Default.GetString(new byte[] { 12 });

    public WordDocument(Word.Document document) {
      this.Document = document;
    }

    public object GetInfo() {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;

      Word.Range range = this.Document.Range();
      Word.PageSetup setup = range.PageSetup;

      object size = new {
        width = setup.PageWidth,
        height = setup.PageHeight
      };

      object margin = new {
        top = setup.TopMargin,
        right = setup.RightMargin,
        bottom = setup.BottomMargin,
        left = setup.LeftMargin
      };

      Word.Pages pages = this.Document.ActiveWindow.ActivePane.Pages;
      int count = pages.Count;
      object[] dimensions = new object[count];

      for (int i = 1; i <= count; i++) {
        Word.Page page = pages[i];

        dimensions[(i - 1)] = new {
          width = page.Width,
          height = page.Height
        };
      }

      return new {
        page = new {
          size,
          margin
        },
        dimensions
      };
    }

    public void SnapshotPages(string dirName) {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;
      Word.Pages pages = this.Document.ActiveWindow.ActivePane.Pages;

      for (int i = 0; i > pages.Count; i++) {
        Word.Page page = pages[i];
        dynamic bytes = page.EnhMetaFileBits;
        MemoryStream stream = new MemoryStream(bytes);

        string fileName = string.Format(@"{0}{1}{2}.png",
          Path.GetFullPath(dirName),
          Path.DirectorySeparatorChar,
          i.ToString()
        );

        Image.FromStream(stream).Save(fileName, ImageFormat.Png);
      }
    }

    private void MarkTransferPage(Word.Paragraph paragraph) {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;
      Word.Range range = paragraph.Range;
      Word.Words words = range.Words;
      Word.Range last = words.Last;

      int page = (int)last.Information[Word.WdInformation.wdActiveEndPageNumber];

      for (int i = words.Count - 1; i > 0; i--) {
        range = words[i];
        int item = (int)range.Information[Word.WdInformation.wdActiveEndPageNumber];

        if (item < page) {
          range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
          range.InsertBreak(Word.WdBreakType.wdLineBreak);

          return;
        }
      }

      last.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
      last.InsertBreak(Word.WdBreakType.wdPageBreak);
    }

    public void FixPageBreaks() {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;
      Word.Paragraphs paragraphs = Document.Paragraphs;
      Word.Paragraph paragraph;
      Word.Range range;

      int reduce = this.Document.ActiveWindow.ActivePane.Pages.Count;

      for (int i = this.Document.Paragraphs.Count; i > 0; i--) {
        paragraph = paragraphs[i];
        range = paragraph.Range;
        range.TextRetrievalMode.IncludeHiddenText = true;
        int pageBreakIndex = range.Text.IndexOf(this.BreakPageSymbol);
        int lineBreakIndex = range.Text.IndexOf(this.BreakLineSymbol);

        if (pageBreakIndex > -1) {
          reduce--;
          continue;
        }

        if (lineBreakIndex > -1) {
          range.SetRange(lineBreakIndex, lineBreakIndex + 1);
          range.Delete();
        }

        range.Collapse(Word.WdCollapseDirection.wdCollapseStart);

        int item = (int)range.Information[Word.WdInformation.wdActiveEndPageNumber];

        if (item < reduce) {
          this.MarkTransferPage(paragraph);
          reduce--;
        }
      }
    }

    public void SaveAsHtml(string htmlFileName) {
      this.Document.WebOptions.Encoding = Office.MsoEncoding.msoEncodingUTF8;
      this.Document.WebOptions.OptimizeForBrowser = true;
      this.Document.WebOptions.OrganizeInFolder = true;
      this.Document.WebOptions.UseLongFileNames = true;
      this.Document.WebOptions.AllowPNG = true;
      //this.Document.WebOptions.ScreenSize = Office.MsoScreenSize.msoScreenSize1152x900;
      this.Document.WebOptions.PixelsPerInch = 96; // 72, 96, 120

      object fileFormat = Word.WdSaveFormat.wdFormatFilteredHTML;
      object fileName = htmlFileName;

      this.Document.SaveAs(ref fileName, ref fileFormat);
    }

    public void Close() {

      // Сохранить измененные документы перед закрытием Word
      object SaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;

      // Задает формат сохранения для документа
      object OriginalFormat = Word.WdOriginalFormat.wdOriginalDocumentFormat;

      // Отправить документ к следующему получателю (не учитывается)
      object RouteDocument = true;

      this.Document.Close(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
    }
  }
}
