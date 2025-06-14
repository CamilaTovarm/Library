﻿using Library.Context;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repositories
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> CreateBook(string title, string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId, string imgUrl, int authorId, bool loanState);
        Task<Book> UpdateBook(Book book);
        Task<Book> DeleteBook(Book book);
    }
    public class BooksRepository: IBooksRepository
    {
        private readonly LibraryDbContext _db;
        public BooksRepository(LibraryDbContext db)
        {
            _db = db;
        }
        public async Task<Book> CreateBook(string title, string isbn, DateOnly publicationDate, int pageCount, int editorialId, int countryId, string imgUrl, int authorId, bool loanState)
        {
            Editorial? editorial = _db.Editorial.FirstOrDefault(ut => ut.EditorialId == editorialId);
            Country? country = _db.Country.FirstOrDefault(ut => ut.CountryId == countryId);
            Authors? authors = _db.Authors.FirstOrDefault(ut => ut.AuthorId == authorId);

            Book newBook = new Book
            {
                BookTitle = title,
                ISBN = isbn,
                PublicationDate = publicationDate,
                PageCount = pageCount,
                EditorialId = editorialId,
                CountryId = countryId,
                ImgUrl = imgUrl,
                AuthorId = authorId,
                LoanState = loanState,
                CreateDate = null,
                State = false
            };
            await _db.Books.AddAsync(newBook);
            _db.SaveChanges();
            return newBook;
        }

        public async Task<Book> DeleteBook(Book book)
        {
            _db.Books.Attach(book); //Llamamos la actualizacion
            _db.Entry(book).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(u => u.BookId == id);
        }

        public async Task<Book> UpdateBook(Book book)
        {
            Book bookUpdate = await _db.Books.FindAsync(book.BookId);

            if (bookUpdate != null)
            {
                bookUpdate.BookTitle = book.BookTitle;
                bookUpdate.ISBN = book.ISBN;
                bookUpdate.PublicationDate = book.PublicationDate;
                bookUpdate.PageCount = book.PageCount;
                bookUpdate.EditorialId = book.EditorialId;
                bookUpdate.CountryId = book.CountryId;
                bookUpdate.ImgUrl = book.ImgUrl;
                bookUpdate.AuthorId = book.AuthorId;
                bookUpdate.LoanState = book.LoanState;

                await _db.SaveChangesAsync();
            }

            return bookUpdate;
        }
    }
}
