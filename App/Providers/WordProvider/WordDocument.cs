using System;
using System.IO;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using System.Drawing;
using System.Drawing.Imaging;

namespace HyperOffice.App.Providers
{
  class WordDocument
  {
    public Word.Document Document;
    private readonly string BreakLineSymbol = System.Text.Encoding.Default.GetString(new byte[] { 11 });
    private readonly string BreakPageSymbol = System.Text.Encoding.Default.GetString(new byte[] { 12 });

    public WordDocument(Word.Document document)
    {
      this.Document = document;
    }

    public object GetInfo()
    {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;

      var range = this.Document.Range();
      var setup = range.PageSetup;

      var size = new {
        width = setup.PageWidth,
        height = setup.PageHeight
      };

      var margin = new {
        top = setup.TopMargin,
        right = setup.RightMargin,
        bottom = setup.BottomMargin,
        left = setup.LeftMargin
      };

      var pages = this.Document.ActiveWindow.ActivePane.Pages;
      int count = pages.Count;
      object[] dimensions = new object[count];

      for (int i = 1; i <= count; i++)
      {
        var page = pages[i];

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

    public void SnapshotPages(string dirName)
    {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;

      var pages = this.Document.ActiveWindow.ActivePane.Pages;
      int number = 0;

      foreach (Word.Page item in pages)
      {
        MemoryStream stream = new MemoryStream(item.EnhMetaFileBits);

        number++;

        string fileName = string.Format(@"{0}{1}{2}.png",
          Path.GetFullPath(dirName),
          Path.DirectorySeparatorChar,
          number.ToString()
        );

        Image.FromStream(stream).Save(fileName, ImageFormat.Png);
      }
    }

    private void SetupPageBreak(Word.Range range)
    {
      range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
      range.InsertBreak(Word.WdBreakType.wdLineBreak);
    }

    private void MarkTransferPage(Word.Paragraph paragraph)
    {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;

      var range = paragraph.Range;
      int page = (int)range.Words.Last.Information[Word.WdInformation.wdActiveEndPageNumber];

      for (int i = range.Words.Count - 1; i > 0; i--)
      {
        range = range.Words[i];
        int item = (int)range.Information[Word.WdInformation.wdActiveEndPageNumber];

        if (item < page)
        {
          this.SetupPageBreak(range);
          return;
        }
      }

      this.SetupPageBreak(range.Words.Last);
    }

    public void FixPageBreaks()
    {
      this.Document.ActiveWindow.View.Type = Word.WdViewType.wdPrintView;

      var paragraphs = Document.Paragraphs;
      Word.Paragraph paragraph;
      Word.Range range;

      int reduce = this.Document.ActiveWindow.ActivePane.Pages.Count;

      for (int i = this.Document.Paragraphs.Count; i > 0; i--)
      {
        paragraph = paragraphs[i];
        range = paragraph.Range;
        range.TextRetrievalMode.IncludeHiddenText = true;

        int pageBreakIndex = range.Text.IndexOf(this.BreakPageSymbol);
        int lineBreakIndex = range.Text.IndexOf(this.BreakLineSymbol);

        if (pageBreakIndex > -1)
        {
          reduce--;
          continue;
        }

        if (lineBreakIndex > -1)
        {
          range.SetRange(lineBreakIndex, lineBreakIndex + 1);
          range.Delete();
        }

        range.Collapse(Word.WdCollapseDirection.wdCollapseStart);

        int item = (int)range.Information[Word.WdInformation.wdActiveEndPageNumber];

        if (item < reduce)
        {
          this.MarkTransferPage(paragraph);
          reduce--;
        }
      }
    }

    public void SaveAsHtml(string htmlFileName)
    {
      this.Document.WebOptions.Encoding = Office.MsoEncoding.msoEncodingUTF8;
      this.Document.WebOptions.OptimizeForBrowser = true;
      this.Document.WebOptions.OrganizeInFolder = true;
      this.Document.WebOptions.UseLongFileNames = true;
      this.Document.WebOptions.AllowPNG = true;
      this.Document.WebOptions.PixelsPerInch = 96;

      object fileFormat = Word.WdSaveFormat.wdFormatFilteredHTML;
      object fileName = htmlFileName;

      this.Document.SaveAs(ref fileName, ref fileFormat);
    }

    public void Close()
    {
      object SaveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
      object OriginalFormat = Word.WdOriginalFormat.wdOriginalDocumentFormat;
      object RouteDocument = true;

      this.Document.Close(ref SaveChanges, ref OriginalFormat, ref RouteDocument);
    }
  }
}
