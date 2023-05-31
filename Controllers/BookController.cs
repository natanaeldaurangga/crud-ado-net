using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApi.DTOs;
using BookApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly BookRepository _bookRepo;

        public BookController(BookRepository bookRepo)
        {
            _bookRepo = bookRepo;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _bookRepo.GetAllBooks();
            return Ok(books);
        }

        [HttpPost]
        public IActionResult AddBook(BookDTO dto)
        {
            var result = _bookRepo.InsertBook(dto);
            return Ok(result);
        }

        [HttpPut("/{id}")]
        public IActionResult UpdateBook([FromRoute] string id, BookDTO dto)
        {
            var result = _bookRepo.UpdateBook(id, dto);
            return Ok(result);
        }

        [HttpDelete("/{id}")]
        public IActionResult DeleteBook([FromRoute] string id)
        {
            var result = _bookRepo.DeleteBook(id);
            return NoContent();
        }
    }
}