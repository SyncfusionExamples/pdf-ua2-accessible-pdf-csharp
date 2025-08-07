// See https://aka.ms/new-console-template for more information

using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

#region content string

string pdfChapter =
    "We�ll begin with a conceptual overview of a simple PDF document. This chapter is designed to be a brief orientation before diving in and creating a real document from scratch \r\n \r\n A PDF file can be divided into four parts: a header, body, cross-reference table, and trailer. The header marks the file as a PDF, the body defines the visible document, the cross-reference table lists the location of everything in the file, and the trailer provides instructions for how to start reading the file.";
string header =
    "The header is simply a PDF version number and an arbitrary sequence of binary data.The binary data prevents na�ve applications from processing the PDF as a text file. This would result in a corrupted file, since a PDF typically consists of both plain text and binary data (e.g., a binary font file can be directly embedded in a PDF).";
string body =
    "The page tree serves as the root of the document. In the simplest case, it is just a list of the pages in the document. Each page is defined as an independent entity with metadata (e.g., page dimensions) and a reference to its resources and content, which are defined separately. Together, the page tree and page objects create the �paper� that composes the document.\r\n \r\n  Resources are objects that are required to render a page. For example, a single font is typically used across several pages, so storing the font information in an external resource is much more efficient. A content object defines the text and graphics that actually show up on the page. Together, content objects and resources define theappearance of an individual page. \r\n \r\n  Finally, the document�s catalog tells applications where to start reading the document. Often, this is just a pointer to the root page tree.";
string crossRef =
    "After the header and the body comes the cross-reference table. It records the byte location of each object in the body of the file. This enables random-access of the document, so when rendering a page, only the objects required for that page are read from the file. This makes PDFs much faster than their PostScript predecessors, which had to read in the entire file before processing it.";
string trailer =
    "Finally, we come to the last component of a PDF document. The trailer tells applications how to start reading the file. At minimum, it contains three things: \r\n 1. A reference to the catalog which links to the root of the document. \r\n 2. The location of the cross-reference table. \r\n 3. The size of the cross-reference table. \r\n \r\n Since a trailer is all you need to begin processing a document, PDFs are typically read back-to-front: first, the end of the file is found, and then you read backwards until you arrive at the beginning of the trailer. After that, you should have all the information you need to load any page in the PDF.";
#endregion

//Create a new PDF/UA-2 document
CreatePDFUA2(pdfChapter, header, body, crossRef, trailer);
//Create a new WTPDF document
CreateWTPDF(pdfChapter, header, body, crossRef, trailer);

static void CreatePDFUA2(string pdfChapter, string header, string body, string crossRef, string trailer)
{

    //Create a new PDF document
    using (PdfDocument document = new PdfDocument())
    {

        //Set the PDF file version to 2.0
        document.FileStructure.Version = PdfVersion.Version2_0;

        //Enable the auto tag property as true
        document.AutoTag = true;

        //Set the document title
        document.DocumentInformation.Title = "PDF/UA-2";

        //Add a new page to the document
        PdfPage page1 = document.Pages.Add();

        //Load the image from the disk
        FileStream imageStream =
            new FileStream("data/page1.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        PdfBitmap image = new PdfBitmap(imageStream);

        //Draw the image to page 1
        page1.Graphics.DrawImage(image, 0, 0, page1.GetClientSize().Width, page1.GetClientSize().Height - 20);

        //Create a second page
        PdfPage page2 = document.Pages.Add();

        //Read the font from the disk
        FileStream fontStream = new FileStream("data/arial.ttf", FileMode.Open, FileAccess.Read);

        //Create a font object for adding content text
        PdfTrueTypeFont contentFont = new PdfTrueTypeFont(fontStream, 9);

        //Create a font object for adding title text
        PdfTrueTypeFont TitleFont = new PdfTrueTypeFont(contentFont, 22);

        //Create a font object for adding header text
        PdfTrueTypeFont headerFont = new PdfTrueTypeFont(contentFont, 16);

        PdfLayoutResult result = null;

        //Draw the title text
        DrawText("Chapter 1 Conceptual Overview", TitleFont, page2, new RectangleF(100, 0, page2.GetClientSize().Width - 100, 40), out result);

        //Draw the title paragraph text
        DrawText(pdfChapter, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 70), out result);

        //Draw the remaining texts
        DrawText("1.0 Header", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(header, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 40), out result);
        DrawText("1.2 Body", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(body, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 120), out result);
        DrawText("1.3 Cross-reference Table", headerFont, page2, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(crossRef, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 50), out result);
        DrawText("1.4 Trailer", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(trailer, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 110), out result);

        //Save the document
        using (FileStream outputFileStream = new FileStream("PDF_UA_2.pdf", FileMode.Create, FileAccess.ReadWrite))
        {
            //Save the document
            document.Save(outputFileStream);
        }
    }
}

static void DrawText(string text, PdfFont font, PdfPage page, RectangleF bounds, out PdfLayoutResult result)
{
    //Draw text
    PdfTextElement textElement =
               new PdfTextElement(text, font, PdfBrushes.Black);
    result = textElement.Draw(page, bounds);
}

static void CreateWTPDF(string pdfChapter, string header, string body, string crossRef, string trailer)
{

    //Create a new PDF document
    using (PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A4))
    {

        //Set the PDF file version to 2.0
        document.FileStructure.Version = PdfVersion.Version2_0;

        //Enable the auto tag property as true
        document.AutoTag = true;

        //Set the document title
        document.DocumentInformation.Title = "WTPDF";

        //Add a new page to the document
        PdfPage page1 = document.Pages.Add();

        //Load the image from the disk
        FileStream imageStream =
            new FileStream("data/page1.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        PdfBitmap image = new PdfBitmap(imageStream);

        //Draw the image to page 1
        page1.Graphics.DrawImage(image, 0, 0, page1.GetClientSize().Width, page1.GetClientSize().Height - 20);

        //Create a second page
        PdfPage page2 = document.Pages.Add();

        //Read the font from the disk
        FileStream fontStream = new FileStream("data/arial.ttf", FileMode.Open, FileAccess.Read);

        //Create a font object for adding content text
        PdfTrueTypeFont contentFont = new PdfTrueTypeFont(fontStream, 9);

        //Create a font object for adding title text
        PdfTrueTypeFont TitleFont = new PdfTrueTypeFont(contentFont, 22);

        //Create a font object for adding header text
        PdfTrueTypeFont headerFont = new PdfTrueTypeFont(contentFont, 16);

        PdfLayoutResult result = null;

        //Draw the title text
        DrawText("Chapter 1 Conceptual Overview", TitleFont, page2, new RectangleF(100, 0, page2.GetClientSize().Width - 100, 40), out result);

        //Draw the title paragraph text
        DrawText(pdfChapter, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 70), out result);

        //Draw the remaining texts
        DrawText("1.0 Header", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(header, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 40), out result);
        DrawText("1.2 Body", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(body, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 120), out result);
        DrawText("1.3 Cross-reference Table", headerFont, page2, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(crossRef, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 50), out result);
        DrawText("1.4 Trailer", headerFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 30, result.Page.GetClientSize().Width, 40), out result);
        DrawText(trailer, contentFont, result.Page, new RectangleF(0, result.Bounds.Bottom + 10, result.Page.GetClientSize().Width, 110), out result);

        //Save the document
        using (FileStream outputFileStream = new FileStream("WTPDF.pdf", FileMode.Create, FileAccess.ReadWrite))
        {
            //Save the document
            document.Save(outputFileStream);
        }
    }
}

