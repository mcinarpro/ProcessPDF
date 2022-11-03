// See https://aka.ms/new-console-template for more information


using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

var fileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "File.pdf");
string outFileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "EncryptedFile.pdf");
string newFileName = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "NewFile.pdf");
Console.WriteLine(fileName);


var img1 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "img1.png");
var img2 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "img2.png");
var img3 = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "img3.png");


PdfReader reader = new PdfReader(outFileName);
try
{
PdfDocument document = new PdfDocument(reader);

}
catch
{
    Console.WriteLine("Error in reading");
}
if (reader.IsEncrypted())
{
    Console.WriteLine("PDF crypencryptedted");
    //Do something
}
else
{
    Console.WriteLine("PDF NOT encrypted");
}



string OWNER_PASSWORD = "test1";
string USER_PASSWORD = "test2";



//string initialFileName = fileName;
//PdfEncryptor encryptor = new PdfEncryptor();
//EncryptionProperties encryptionProperties = new EncryptionProperties();
//encryptionProperties.SetStandardEncryption(Encoding.UTF8.GetBytes(USER_PASSWORD), Encoding.UTF8.GetBytes(OWNER_PASSWORD), 0, 0);
//encryptor.SetEncryptionProperties(encryptionProperties);
//using (PdfReader initialFile = new PdfReader(initialFileName))
//{
//    using (FileStream outputStream = new FileStream(outFileName, FileMode.Create))
//    {
//        encryptor.Encrypt(initialFile, outputStream);
//    }
//}
//ReaderProperties readerProperties = new ReaderProperties();
//readerProperties.SetPassword();
//PdfReader outFile = new PdfReader(outFileName, readerProperties);
//PdfDocument doc = new PdfDocument(outFile);
//doc.Close();



byte[] Combine(IEnumerable<byte[]> pdfs)
{
    using (var writerMemoryStream = new MemoryStream())
    {
        using (var writer = new PdfWriter(writerMemoryStream))
        {
            using (var mergedDocument = new PdfDocument(writer))
            {
                var merger = new PdfMerger(mergedDocument);

                foreach (var pdfBytes in pdfs)
                {
                    try
                    {

                    using (var copyFromMemoryStream = new MemoryStream(pdfBytes))
                    {
                        using (var reader = new PdfReader(copyFromMemoryStream))
                        {
                            using (var copyFromDocument = new PdfDocument(reader))
                            {
                                merger.Merge(copyFromDocument, 1, copyFromDocument.GetNumberOfPages());
                            }
                        }
                    }

                    }
                    catch(Exception ex)
                    {

                             using var imageDocument = new PdfDocument(new PdfWriter($"temp_img.pdf"));
                        var imageData = ImageDataFactory.Create(pdfBytes);
                        var image = new Image(imageData);
                        imageDocument.AddNewPage(new PageSize(
                            PageSize.A4.GetWidth(),
                            image.GetImageHeight() * PageSize.A4.GetWidth() / image.GetImageWidth()
                        ));

                        using var document = new Document(imageDocument);
                        document.SetMargins(0, 0, 0, 0);
                        document.Add(image);
                        imageDocument.Close();
                        document.Close();
                        using var writtenImagePdf = new PdfDocument(new PdfReader($"temp_img.pdf"));
                        merger.Merge(writtenImagePdf, 1, 1);
                    }
                }






            }
        }

        return writerMemoryStream.ToArray();
    }
}

var fileBytes = File.ReadAllBytes(fileName);
var img1Bytes = File.ReadAllBytes(img1);
var img2Bytes = File.ReadAllBytes(img2);
var pdfList = new List<byte[]> { fileBytes, img1Bytes, fileBytes, img2Bytes };

var result = Combine(pdfList);

File.WriteAllBytes(newFileName, result);





