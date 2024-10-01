using backend_Master.Models;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly MyDbContext _context;

        public EventController(MyDbContext context)
        {
            _context = context;
        }

        // إضافة فعالية جديدة
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] Event newEvent)
        {
            if (newEvent == null)
            {
                return BadRequest("بيانات الفعالية غير صالحة.");
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEventById), new { id = newEvent.EventId }, newEvent);
        }

        // عرض كافة الفعاليات
        [HttpGet]
        public IActionResult GetAllEvents()
        {
            var events = _context.Events.ToList();
            return Ok(events);
        }
        [HttpGet("randomEvent")]
        public IActionResult GetRandomEvents()
        {
            var randomEvents = _context.Events
                                       .OrderBy(e => Guid.NewGuid())
                                       .Take(4)
                                       .ToList();

            return Ok(randomEvents);
        }

        // عرض فعالية واحدة باستخدام المعرف
        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            var eventItem = _context.Events.Find(id);
            if (eventItem == null)
            {
                return NotFound("الفعالية غير موجودة.");
            }

            return Ok(eventItem);
        }

        // تعديل بيانات فعالية
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] Event updatedEvent)
        {
            if (id != updatedEvent.EventId)
            {
                return BadRequest("معرف الفعالية غير متطابق.");
            }

            var eventInDb = _context.Events.Find(id);
            if (eventInDb == null)
            {
                return NotFound("الفعالية غير موجودة.");
            }

            // تعديل كافة الحقول
            eventInDb.EventTitle = updatedEvent.EventTitle;
            eventInDb.EventDate = updatedEvent.EventDate;
            eventInDb.EventTime = updatedEvent.EventTime;
            eventInDb.Location = updatedEvent.Location;
            eventInDb.Participants = updatedEvent.Participants;
            eventInDb.Speaker = updatedEvent.Speaker;
            eventInDb.Summary = updatedEvent.Summary;
            eventInDb.Learnings = updatedEvent.Learnings;
            eventInDb.Features = updatedEvent.Features;
            eventInDb.SeatsAvailable = updatedEvent.SeatsAvailable;
            eventInDb.Topics = updatedEvent.Topics;
            eventInDb.Exams = updatedEvent.Exams;
            eventInDb.Articles = updatedEvent.Articles;
            eventInDb.Certificates = updatedEvent.Certificates;
            eventInDb.ImagePath = updatedEvent.ImagePath;
            eventInDb.MapUrl = updatedEvent.MapUrl;
            eventInDb.ZoomLink = updatedEvent.ZoomLink;
            eventInDb.ZoomPassword = updatedEvent.ZoomPassword;

            _context.Events.Update(eventInDb);
            await _context.SaveChangesAsync();

            return NoContent(); // تم التعديل بنجاح بدون إرجاع بيانات
        }

        // حذف فعالية
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = _context.Events.Find(id);
            if (eventItem == null)
            {
                return NotFound("الفعالية غير موجودة.");
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent(); // تم الحذف بنجاح
        }

        // API لتحميل ملف PDF أو كتابة البيانات يدويًا
        [HttpPost("CreateOrUploadEvent")]
        public async Task<IActionResult> CreateOrUploadEvent([FromForm] IFormFile file, [FromForm] string eventName, [FromForm] string eventDate, [FromForm] string startTime, [FromForm] string location, [FromForm] int participants, [FromForm] string speaker, [FromForm] string summary, [FromForm] string learnings, [FromForm] string features, [FromForm] int seatsAvailable, [FromForm] int topics, [FromForm] int exams, [FromForm] int articles, [FromForm] int certificates, [FromForm] string imagePath, [FromForm] string mapUrl, [FromForm] string zoomLink, [FromForm] string zoomPassword)
        {
            Event newEvent;

            // إذا تم رفع ملف PDF
            if (file != null && file.Length > 0)
            {
                // استخراج النصوص من ملف PDF
                var extractedText = await ExtractTextFromPdf(file);

                if (string.IsNullOrEmpty(extractedText))
                {
                    return BadRequest("Unable to extract text from PDF.");
                }

                // تحويل النص إلى بيانات صالحة وإدخالها في قاعدة البيانات
                newEvent = ParsePdfTextToEvent(extractedText);
                if (newEvent == null)
                {
                    return BadRequest("Invalid data extracted from PDF.");
                }
            }
            else // إذا تم إدخال البيانات يدويًا
            {
                if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(eventDate) || string.IsNullOrEmpty(startTime) || string.IsNullOrEmpty(location))
                {
                    return BadRequest("Please provide valid event details.");
                }

                // إنشاء كائن فعالية بناءً على المدخلات اليدوية
                newEvent = new Event
                {
                    EventTitle = eventName,
                    EventDate = DateOnly.Parse(eventDate),
                    EventTime = TimeOnly.Parse(startTime),
                    Location = location,
                    Participants = participants,
                    Speaker = speaker,
                    Summary = summary,
                    Learnings = learnings,
                    Features = features,
                    SeatsAvailable = seatsAvailable,
                    Topics = topics,
                    Exams = exams,
                    Articles = articles,
                    Certificates = certificates,
                    ImagePath = imagePath,
                    MapUrl = mapUrl,
                    ZoomLink = zoomLink,
                    ZoomPassword = zoomPassword
                };
            }

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync(); // استخدام المعالجة غير المتزامنة

            return Ok(newEvent); // إرجاع البيانات التي تم إدخالها
        }

        // دالة لاستخراج النص من ملف PDF
        private async Task<string> ExtractTextFromPdf(IFormFile pdfFile)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await pdfFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    using (PdfReader reader = new PdfReader(memoryStream))
                    {
                        StringBuilder text = new StringBuilder();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                        }
                        return text.ToString();
                    }
                }
            }
            catch (Exception)
            {
                // التعامل مع الأخطاء المتعلقة بقراءة PDF
                return null;
            }
        }

        // دالة لتحويل النص المستخرج إلى كائن فعالية (Event)
        private Event ParsePdfTextToEvent(string pdfText)
        {
            try
            {
                // استخدام Regex لاستخراج بعض الحقول من النص
                var titleMatch = Regex.Match(pdfText, @"عنوان الفعالية: (.+)");
                var dateMatch = Regex.Match(pdfText, @"تاريخ: (.+)");
                var locationMatch = Regex.Match(pdfText, @"الموقع: (.+)");
                var speakerMatch = Regex.Match(pdfText, @"المتحدث: (.+)");
                var summaryMatch = Regex.Match(pdfText, @"الملخص: (.+)");

                // تحويل النصوص إلى أعداد صحيحة أو قيم افتراضية
                int.TryParse("100", out int seatsAvailable); // هنا يجب عليك تعديل النص ليكون من ملف PDF
                int.TryParse("0", out int topics); // تعديل النص كما هو موجود في ملف PDF
                int.TryParse("0", out int exams);
                int.TryParse("0", out int articles);
                int.TryParse("0", out int certificates);

                // إنشاء كائن جديد من Event
                var newEvent = new Event
                {
                    EventTitle = titleMatch.Success ? titleMatch.Groups[1].Value.Trim() : "عنوان افتراضي",
                    EventDate = dateMatch.Success ? DateOnly.Parse(dateMatch.Groups[1].Value.Trim()) : DateOnly.FromDateTime(DateTime.Now),
                    EventTime = TimeOnly.Parse("10:00"), // يمكنك تحسين استخراج الوقت من النص
                    Location = locationMatch.Success ? locationMatch.Groups[1].Value.Trim() : "المكان الافتراضي",
                    Participants = 0, // يمكن تعديل هذا إذا كانت المعلومات متوفرة
                    Speaker = speakerMatch.Success ? speakerMatch.Groups[1].Value.Trim() : "المتحدث الافتراضي",
                    Summary = summaryMatch.Success ? summaryMatch.Groups[1].Value.Trim() : "الملخص الافتراضي",
                    Learnings = "التعلم الافتراضي",
                    Features = "الميزات الافتراضية",
                    SeatsAvailable = seatsAvailable, // تحويل العدد من النص إلى int
                    Topics = topics, // تحويل العدد من النص إلى int
                    Exams = exams, // تحويل العدد من النص إلى int
                    Articles = articles, // تحويل العدد من النص إلى int
                    Certificates = certificates, // تحويل العدد من النص إلى int
                    ImagePath = "/images/default.png", // صورة افتراضية
                    MapUrl = "https://google.com/maps",
                    ZoomLink = "https://zoom.us/j/123456789",
                    ZoomPassword = "12345"
                };

                return newEvent; // إرجاع الكائن الجديد
            }
            catch (Exception)
            {
                // التعامل مع الأخطاء في حالة فشل استخراج البيانات
                return null;
            }
        }

    }
}
