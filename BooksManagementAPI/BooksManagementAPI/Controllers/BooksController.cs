using AutoMapper;
using BooksManagementAPI.Models;
using BooksManagementAPI.Models.Dtos;
using BooksManagementAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagementAPI.Controllers
{
    [Route("api/Books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _repo;
        private readonly IMapper _mapper;
        public BooksController(IBooksRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// To get all Books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<BookDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetBooks()
        {
            var listBooks = await _repo.GetBooks();
            var listBooksDto = new List<BookDto>();

            foreach (var item in listBooks)
            {
                listBooksDto.Add(_mapper.Map<BookDto>(item));
            }
            return Ok(listBooksDto);
        }
        /// <summary>
        /// to get a book by its category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetBook")]
        [ProducesResponseType(200, Type = typeof(List<BookDto>))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetBook(int id)
        {
            var itemBook = await _repo.GetBook(id);

            if (itemBook == null)
            {
                return NotFound();
            }
            var itemBookDto = _mapper.Map<BookDto>(itemBook);
            return Ok(itemBookDto);
        }

        /// <summary>
        /// to create a new book / add a new book to the list of books
        /// </summary>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(List<BookDto>))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBook([FromBody] BookDto bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest(ModelState);
            }
            if (await _repo.ExistBook(bookDto.Name))
            {
                ModelState.AddModelError("", "the book alredy exist");
                return StatusCode(404, ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);

            var bookCreate = await _repo.CreateBook(book);
            if (!bookCreate)
            {
                ModelState.AddModelError("", $"an error occurred saving the book: {book.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new { Id = book.Id }, book);
        }

        /// <summary>
        /// to update a book
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="bookDto"></param>
        /// <returns></returns>
        [HttpPatch("{Id:int}", Name = "UpdateBook")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBook(int Id, [FromBody] BookDto bookDto)
        {
            if (bookDto == null || Id != bookDto.Id)
            {
                return BadRequest(ModelState);
            }

            var book = _mapper.Map<Book>(bookDto);

            var bookUpdate = await _repo.UpdateBook(book);
            if (!bookUpdate)
            {
                ModelState.AddModelError("", $"an error occurred updating the book: {book.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// to delete a book / remove a book from the book's list
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id:int}", Name = "DeleteBook")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int Id)
        {
            if (! await _repo.ExistBook(Id))
            {
                return NotFound();
            }

            var book = await _repo.GetBook(Id);

            if (! await _repo.DeleteBook(book.Id))
            {
                ModelState.AddModelError("", $"an error occurred deleting the book: {book.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
