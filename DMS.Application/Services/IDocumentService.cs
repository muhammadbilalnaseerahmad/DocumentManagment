using DMS.Domain.Entities;
using DocumentManagmentSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Application.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<Document>> GetAllDocumentsAsync();
        Task<Document> GetDocumentByIdAsync(int id);
        Task<DocumentResponse> CreateDocumentAsync(IFormFile document, string destinationPath);
        Task<DocumentResponse> UpdateDocumentAsync(int id, IFormFile document, string destinationPath);
        Task<bool> DeleteDocumentAsync(int id);

    }
}
