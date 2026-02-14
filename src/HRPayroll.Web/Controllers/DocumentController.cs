using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HRPayroll.Domain.Entities.HR;
using HRPayroll.Domain.Interfaces;

namespace HRPayroll.Web.Controllers;

public class DocumentController : Controller
{
    private readonly IDocumentService _documentService;
    private readonly IDocumentCategoryService _documentCategoryService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IWebHostEnvironment _environment;

    // File upload settings
    private readonly string[] _allowedExtensions = { ".pdf", ".docx", ".doc", ".jpeg", ".jpg", ".png" };
    private readonly int _maxFileSizeMB = 10;

    public DocumentController(
        IDocumentService documentService,
        IDocumentCategoryService documentCategoryService,
        IEmployeeRepository employeeRepository,
        IWebHostEnvironment environment)
    {
        _documentService = documentService;
        _documentCategoryService = documentCategoryService;
        _employeeRepository = employeeRepository;
        _environment = environment;
    }

    // ==================== DOCUMENT CATEGORIES ====================

    public async Task<IActionResult> Categories()
    {
        var categories = await _documentCategoryService.GetAllCategoriesAsync();
        return View(categories);
    }

    public async Task<IActionResult> CreateCategory()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(DocumentCategory category)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _documentCategoryService.CreateCategoryAsync(category);
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction(nameof(Categories));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return View(category);
    }

    public async Task<IActionResult> EditCategory(long id)
    {
        var category = await _documentCategoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(long id, DocumentCategory category)
    {
        try
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _documentCategoryService.UpdateCategoryAsync(category);
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction(nameof(Categories));
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        try
        {
            await _documentCategoryService.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Categories));
    }

    // ==================== EMPLOYEE DOCUMENTS ====================

    public async Task<IActionResult> Index(long? employeeId, long? categoryId, string? documentType)
    {
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = employees;

        var categories = await _documentCategoryService.GetActiveCategoriesAsync();
        ViewBag.Categories = categories;

        IEnumerable<EmployeeDocument> documents;
        
        if (employeeId.HasValue)
        {
            documents = await _documentService.GetByEmployeeIdAsync(employeeId.Value);
            ViewBag.EmployeeId = employeeId;
        }
        else if (categoryId.HasValue)
        {
            documents = await _documentService.GetByCategoryIdAsync(categoryId.Value);
        }
        else
        {
            documents = await _documentService.GetAllDocumentsWithIncludesAsync();
        }

        // Filter by document type if specified
        if (!string.IsNullOrEmpty(documentType))
        {
            documents = documents.Where(d => d.FileExtension?.ToUpper() == documentType.ToUpper());
        }

        return View(documents);
    }

    public async Task<IActionResult> Upload(long employeeId)
    {
        var categories = await _documentCategoryService.GetActiveCategoriesAsync();
        ViewBag.Categories = categories;
        
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = employees;

        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        ViewBag.EmployeeName = employee?.FullName;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(EmployeeDocument document, IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
            }
            else
            {
                // Validate file extension
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("", $"Invalid file type. Allowed types: {string.Join(", ", _allowedExtensions)}");
                }
                else
                {
                    // Validate file size
                    var fileSizeMB = file.Length / (1024 * 1024);
                    if (fileSizeMB > _maxFileSizeMB)
                    {
                        ModelState.AddModelError("", $"File size exceeds {_maxFileSizeMB} MB limit.");
                    }
                    else
                    {
                        // Save file
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "documents");
                        Directory.CreateDirectory(uploadsFolder);

                        var fileName = $"{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Set document properties
                        document.FilePath = $"/uploads/documents/{fileName}";
                        document.FileName = file.FileName;
                        document.FileExtension = extension;
                        document.FileSize = file.Length;
                        document.ContentType = file.ContentType;

                        await _documentService.UploadDocumentAsync(document);
                        TempData["Success"] = "Document uploaded successfully!";
                        return RedirectToAction(nameof(Index), new { employeeId = document.EmployeeId });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        // Repopulate dropdowns
        var categories = await _documentCategoryService.GetActiveCategoriesAsync();
        ViewBag.Categories = categories;
        
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = employees;

        return View(document);
    }

    public async Task<IActionResult> EditDocument(long id)
    {
        var document = await _documentService.GetDocumentByIdAsync(id);
        if (document == null)
        {
            return NotFound();
        }

        var categories = await _documentCategoryService.GetActiveCategoriesAsync();
        ViewBag.Categories = categories;
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = employees;

        return View(document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditDocument(long id, EmployeeDocument document)
    {
        try
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _documentService.UpdateDocumentAsync(document);
                TempData["Success"] = "Document updated successfully!";
                return RedirectToAction(nameof(Index), new { employeeId = document.EmployeeId });
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        var categories = await _documentCategoryService.GetActiveCategoriesAsync();
        ViewBag.Categories = categories;
        var employees = await _employeeRepository.GetActiveEmployeesAsync();
        ViewBag.Employees = employees;

        return View(document);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDocument(long id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            await _documentService.DeleteDocumentAsync(id);
            TempData["Success"] = "Document deleted successfully!";
            return RedirectToAction(nameof(Index), new { employeeId = document?.EmployeeId });
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> Download(long id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, "application/octet-stream", document.FileName);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> ViewDocument(long id)
    {
        try
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath.TrimStart('/'));
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = document.FileExtension switch
            {
                ".pdf" => "application/pdf",
                ".doc" or ".docx" => "application/msword",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileBytes, contentType);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
