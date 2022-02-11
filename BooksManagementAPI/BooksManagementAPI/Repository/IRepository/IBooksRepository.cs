using BooksManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagementAPI.Repository.IRepository
{
    public interface IBooksRepository 
    {
        Task<ICollection<Book>> GetBooks();
        Task<Book> GetBook(int id);
        Task<bool> CreateBook(Book item);
        Task<bool> UpdateBook(Book item);
        Task<bool> DeleteBook(int Id);
        Task<bool> ExistBook(string name);
        Task<bool> ExistBook(int Id);

        // This method will commit the information to the database
        public bool Save();
        
    }
}
