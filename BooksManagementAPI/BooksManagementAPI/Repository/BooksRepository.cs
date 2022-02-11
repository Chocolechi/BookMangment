using BooksManagementAPI.Data;
using BooksManagementAPI.Models;
using BooksManagementAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagementAPI.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly AppDbContext _db;
        public BooksRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CreateBook(Book item)
        {
            await _db.Book.AddAsync(item);
            return Save();
        }

        public async Task<bool> DeleteBook(int Id)
        {
            Book item = await _db.Book.FindAsync(Id);
            _db.Book.Remove(item);
            return Save();

        }

        public async Task<Book> GetBook(int id)
        {
            return await _db.Book.FindAsync(id);        
        }

        public async Task<ICollection<Book>> GetBooks()
        {
            return await _db.Book.OrderBy(b => b.Name).ToListAsync();
        }

        public async Task<bool> UpdateBook(Book item)
        {
            _db.Book.Update(item);
            return Save();
        }

        public async Task<bool> ExistBook(string name)
        {
            return await _db.Book.AnyAsync(b => b.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> ExistBook(int Id)
        {
            return await _db.Book.AnyAsync(b => b.Id == Id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

    
    }
}
