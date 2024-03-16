using DMS.Domain.Entities;
using DMS.Infrastructure.Repositories;
using DocumentManagmentSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Configuration;
using System.Security.AccessControl;

namespace DMS.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRepository<Document> _documentRepository;
        private readonly IConfiguration _configuration;

        public DocumentService(IRepository<Document> documentRepository, IConfiguration configuration)
        {
            _documentRepository = documentRepository;
            _configuration = configuration;
        }
        public async Task<DocumentResponse> CreateDocumentAsync(IFormFile document, string destinationPath)
        {
            try
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(document.FileName)}";
                var filePath = Path.Combine(destinationPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }
                string baseUrl = _configuration["ApplicationURL"];

                string accessUrl = $"{baseUrl}/Uploads/{fileName}";

                Document doc = new Document()
                {
                    Title = Path.GetFileNameWithoutExtension(document.FileName),
                    CreationDate = DateTime.Now,
                    FileExtension = Path.GetExtension(document.FileName),
                    FileSize = document.Length,
                    AccessURL = accessUrl,
                    IsDeleted = false,
                };
                await _documentRepository.AddAsync(doc);
                return new DocumentResponse
                {
                    Success = true,
                    Message = $"Document uploaded: {document.FileName}"
                };
            }
            catch (Exception e)
            {
                return new DocumentResponse
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        public async Task<bool> DeleteDocumentAsync(int id)
        {
            Document document = await _documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                return false;
            }
            document.IsDeleted = true;
            await _documentRepository.UpdateAsync(document);
            return true;
        }



        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            return (await _documentRepository.GetAllAsync()).Where(x => x.IsDeleted == false);
        }

        public async Task<Document> GetDocumentByIdAsync(int id)
        {
            return await _documentRepository.GetByIdAsync(id);
        }

        public async Task<DocumentResponse> UpdateDocumentAsync(int id, IFormFile document, string destinationPath)
        {
            try
            {
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(document.FileName)}";

                var filePath = Path.Combine(destinationPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await document.CopyToAsync(stream);
                }
                string baseUrl = _configuration["ApplicationURL"];

                string accessUrl = $"{baseUrl}/Uploads/{fileName}";

                Document doc = await _documentRepository.GetByIdAsync(id);

                if (doc is null)
                {
                    return new DocumentResponse
                    {
                        Success = false,
                        Message = $"Document does not exist!"
                    };
                }
                doc.Title = Path.GetFileNameWithoutExtension(document.FileName);
                doc.CreationDate = DateTime.Now;
                doc.FileExtension = Path.GetExtension(document.FileName);
                doc.FileSize = document.Length;
                doc.AccessURL = accessUrl;
                doc.LastModifiedDate = DateTime.Now;
                await _documentRepository.UpdateAsync(doc);

                return new DocumentResponse
                {
                    Success = true,
                    Message = $"Document updated: {document.FileName}"
                };
            }
            catch (Exception e)
            {
                return new DocumentResponse
                {
                    Success = false,
                    Message = e.Message
                };
            }

        }
    }
}
