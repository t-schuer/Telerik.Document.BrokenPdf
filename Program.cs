// See https://aka.ms/new-console-template for more information
using System.Text;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf;


var pdfPath = @"invoice.pdf";
var fi = new FileInfo(pdfPath);
if (!fi.Exists)
    return 1;

var xmlPath = @"factur-x.xml";
var xml = File.ReadAllText(xmlPath);


var pdfFormat = new PdfFormatProvider();
pdfFormat.ImportSettings.CopyStream = true;
pdfFormat.ImportSettings.ReadingMode = Telerik.Windows.Documents.Fixed.FormatProviders.ReadingMode.AllAtOnce;
var pdfStream = fi.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
var doc = pdfFormat.Import(pdfStream);

if (doc.EmbeddedFiles.Any(doc => doc.Name.Equals("factur-x.xml", StringComparison.OrdinalIgnoreCase)))
{
    doc.EmbeddedFiles.Remove("factur-x.xml");
}

doc.EmbeddedFiles.Add("factur-x.xml", Encoding.UTF8.GetBytes(xml));

pdfStream.Seek(0L, SeekOrigin.Begin);

var pdfOutputPath = @"invoice_factur-x.pdf";
var pdfOutStream = new FileStream(pdfOutputPath, FileMode.Create, FileAccess.ReadWrite);
pdfFormat.Export(doc, pdfOutStream);

pdfOutStream.Flush();
pdfStream.Dispose();
pdfOutStream.Dispose();

return 0;