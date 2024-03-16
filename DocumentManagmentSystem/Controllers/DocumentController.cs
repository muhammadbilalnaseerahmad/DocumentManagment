using DMS.Application.Services;
using DMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DocumentManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadFolder;
        public DocumentController(IDocumentService documentService, IWebHostEnvironment environment)
        {
            _environment = environment;
            _documentService = documentService;
            _uploadFolder = Path.Combine(_environment.ContentRootPath, "Uploads");
        }

        [HttpGet("GetAllDocuments")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _documentService.GetAllDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("GetDocumentById/{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);
            if (document == null)
                return NotFound();
            return Ok(document);
        }

        [HttpPost("UploadDocument")]
        public async Task<IActionResult> CreateDocument(IFormFile document)
        {

            var createdDocument = await _documentService.CreateDocumentAsync(document, _uploadFolder);
            return Ok(createdDocument);
        }

        [HttpPut("UpdateDocument/{id}")]
        public async Task<IActionResult> UpdateDocument(int id, IFormFile document)
        {
            var updatedDocument = await _documentService.UpdateDocumentAsync(id, document, _uploadFolder);
            return Ok(updatedDocument);
        }

        [HttpDelete("DeleteDocument/{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var result = await _documentService.DeleteDocumentAsync(id);
            if (!result)
                return NotFound();

            return Ok(result);
        }


    }
}
