using backend_Master.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PDFController : ControllerBase

    {
        private readonly MyDbContext _context;

        public PDFController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // استخراج البيانات من ملف PDF
            var extractedData = await ExtractDataFromPdf(pdfFile);

            // عرض البيانات المستخرجة
            return Ok(extractedData);
        }

        // دالة لاستخراج النصوص من PDF باستخدام PdfPig
        private async Task<List<string>> ExtractDataFromPdf(IFormFile pdfFile)
        {
            List<string> extractedData = new List<string>();

            using (var stream = new MemoryStream())
            {
                await pdfFile.CopyToAsync(stream);
                stream.Position = 0;

                using (var pdfDocument = PdfDocument.Open(stream))
                {
                    foreach (var page in pdfDocument.GetPages())
                    {
                        extractedData.Add(page.Text); // استخراج النص من كل صفحة
                    }
                }
            }

            return extractedData;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitPdfData([FromBody] List<string> extractedData)
        {
            if (extractedData == null || extractedData.Count == 0)
            {
                return BadRequest("No data to submit.");
            }

            foreach (var data in extractedData)
            {
                var pdfDataEntity = new User
                {
                    BiographicaldetailsCv = data
                };
                _context.Users.Add(pdfDataEntity);
            }

            await _context.SaveChangesAsync();

            return Ok("Data has been saved successfully.");
        }
    }
    }

