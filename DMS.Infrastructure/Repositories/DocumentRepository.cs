using DMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Repositories
{
    public class DocumentRepository : IRepository<Document>
    {
        private readonly DmsDbContext _context;

        public DocumentRepository(DmsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> GetByIdAsync(int id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<Document> AddAsync(Document entity)
        {
            _context.Documents.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Document> UpdateAsync(Document entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Document entity)
        {
            _context.Documents.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
