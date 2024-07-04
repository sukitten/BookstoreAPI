using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AuthorService
{
    private readonly HttpClient _httpClient;

    public AuthorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthorWithBooks> GetAuthorDetailsWithBooksAsync(Guid authorId)
    {
        var authors = await GetAuthorDetailsWithBooksBatchAsync(new List<Guid> { authorId });
        return authors.FirstOrDefault();
    }

    public async Task<List<AuthorWithBooks>> GetAuthorDetailsWithBooksBatchAsync(List<Guid> authorIds)
    {
        var requestPath = $"api/authors/with. Books?ids={string.Join(",", authorIds)}";

        var response = await _httpClient.GetAsync(requestPath);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var authorsDetailsList = JsonConvert.DeserializeObject<List<AuthorWithBooks>>(content);
            return authorsDetailsList ?? new List<AuthorWithBooks>();
        }

        throw new Exception("Failed to fetch author details with books.");
    }
}

namespace MyApplication.Models
{
    public class AuthorWithBooks
    {
        public Guid Id { get;set; }
        public string Name { get;set; }
        public string Biography { get;set; }
        public List<Book> Books { get;set; }
    }

    public class Book
    {
        public Guid Id { get;set; }
        public string Title { get;set; }
        public string Genre { get;set; }
    }
}