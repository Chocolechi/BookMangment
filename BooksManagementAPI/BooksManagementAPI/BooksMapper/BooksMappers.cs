using AutoMapper;
using BooksManagementAPI.Models;
using BooksManagementAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagementAPI.BooksMapper
{
    public class BooksMappers : Profile
    {
        public BooksMappers()
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
