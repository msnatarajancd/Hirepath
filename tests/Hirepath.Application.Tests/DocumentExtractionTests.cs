using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Hirepath.Application.Abstractions;
using Hirepath.Application.Common;
using Hirepath.Infrastructure.Documents;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Fonts.Standard14Fonts;
using UglyToad.PdfPig.Writer;

namespace Hirepath.Application.Tests;

public sealed class DocumentExtractionTests
{
    private static readonly DocumentTextExtractionService Service =
        new(new IDocumentTextExtractor[]
        {
            new PdfTextExtractor(),
            new DocxTextExtractor(),
            new PlainTextExtractor(),
        });

    [Fact]
    public async Task Extracts_text_from_txt()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("C# and Angular engineer"));

        var text = await Service.ExtractAsync(stream, "text/plain", "resume.txt");

        text.Should().Contain("Angular");
    }

    [Fact]
    public async Task Extracts_text_from_docx()
    {
        using var stream = BuildDocx("Senior C# and Angular engineer");

        var text = await Service.ExtractAsync(
            stream,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "resume.docx");

        text.Should().Contain("Angular");
    }

    [Fact]
    public async Task Extracts_text_from_pdf()
    {
        using var stream = BuildPdf("Cloud engineer with C# and Azure");

        var text = await Service.ExtractAsync(stream, "application/pdf", "resume.pdf");

        text.Should().Contain("Azure");
    }

    [Fact]
    public async Task Unsupported_type_throws_typed_error()
    {
        using var stream = new MemoryStream([1, 2, 3]);

        var act = async () =>
            await Service.ExtractAsync(stream, "image/png", "photo.png");

        await act.Should().ThrowAsync<UnsupportedDocumentException>();
    }

    private static MemoryStream BuildDocx(string text)
    {
        var stream = new MemoryStream();
        using (var doc = WordprocessingDocument.Create(
            stream, WordprocessingDocumentType.Document, autoSave: true))
        {
            var main = doc.AddMainDocumentPart();
            main.Document = new Document(new Body(new Paragraph(new Run(new Text(text)))));
            main.Document.Save();
        }
        stream.Position = 0;
        return stream;
    }

    private static MemoryStream BuildPdf(string text)
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddStandard14Font(Standard14Font.Helvetica);
        var page = builder.AddPage(UglyToad.PdfPig.Content.PageSize.A4);
        page.AddText(text, 12, new PdfPoint(30, 700), font);
        return new MemoryStream(builder.Build());
    }
}
