using ClosedXML.Excel;
using ExcelDataReader;
using ISSTTP.Data;
using ISSTTP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using ClosedXML.Excel;
using System.Data;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ISSTTP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbcarShopContext _context;
        private readonly CancellationToken cancellationToken;
       


        public HomeController(ILogger<HomeController> logger, DbcarShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (XLWorkbook workBook = new XLWorkbook(stream))
                    {
                        //перегляд усіх листів (в даному випадку категорій книг)
                        foreach (IXLWorksheet worksheet in workBook.Worksheets)
                        {
                            //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову

                            var catname = worksheet.Name;
                            var category = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name == catname, cancellationToken);
                            if (category == null)
                            {
                                category = new Category();
                                category.Name = catname;
                                category.Info = "from EXCEL";
                                //додати в контекст
                                _context.Categories.Add(category);
                            }
                        }
                    }


                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                var detname = reader.GetValue(1).ToString();
                                var detail = await _context.Details.FirstOrDefaultAsync(det => det.Name == detname, cancellationToken);
                                if (detail == null)
                                {
                                    Detail d = new Detail();
                                    d.Name = reader.GetValue(1).ToString();
                                    d.Info = reader.GetValue(2).ToString();
                                    d.Price = Convert.ToInt32(reader.GetValue(3).ToString());

                                    _context.Add(d);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";

            return View();
        }
       
        private const string RootWorksheetName = "";


        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "Назва",
                "Адміністратор",
                "Інформація",
                "Ціна",
            };
       

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteBook(IXLWorksheet worksheet, ISSTTP.Data.Detail detail, int rowIndex)
        {
            var columnIndex = 1;
            worksheet.Cell(rowIndex, columnIndex++).Value = detail.Name;

            var administratordetail = _context.AdministratorDetails.Where(ab => ab.DetailId == detail.Id)
                                                    .Include(ab => ab.Administrator)
                                                    .Distinct();
            //book.AuthorBooks.ToList();
            foreach (var ab in administratordetail)
            {
                    var admin = ab.Administrator;
                    worksheet.Cell(rowIndex, columnIndex++).Value = admin.Name;
                
            }
            worksheet.Cell(rowIndex, 3).Value = detail.Info;
            worksheet.Cell(rowIndex, 4).Value = detail.Price;
        }

        private void WriteBooks(IXLWorksheet worksheet, ICollection<ISSTTP.Data.Detail> details)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var detail in details)
            {
                WriteBook(worksheet, detail, rowIndex);
                rowIndex++;
            }
        }

        private void WriteCategories(XLWorkbook workbook, ICollection<ISSTTP.Data.Category> categories)
        {
            //для усіх категорій формуємо сторінки
            foreach (var cat in categories)
            {

                if (cat is not null)
                {
                    var worksheet = workbook.Worksheets.Add(cat.Name);
                    WriteBooks(worksheet, cat.Details.ToList());
                }
            }
        }


        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }
            //тут для прикладу пишемо усі книги в усіх категоріях, в своїх проєктах потрібно писати лише вибрані категорії та книги
            var categories = await _context.Categories
                .Include(category => category.Details)
                .ToListAsync(cancellationToken);

            var workbook = new XLWorkbook();

            WriteCategories(workbook, (ICollection<Category>)categories);
            workbook.SaveAs(stream);
            
        }
        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      CancellationToken cancellationToken = default)
        {
            

            var memoryStream = new MemoryStream();

            await WriteToAsync(memoryStream, cancellationToken);

            await memoryStream.FlushAsync(cancellationToken);
            memoryStream.Position = 0;


            return new FileStreamResult(memoryStream, contentType)
            {
                FileDownloadName = $"categiries_{DateTime.UtcNow.ToShortDateString()}.xlsx"
            };
        }





    }

}

