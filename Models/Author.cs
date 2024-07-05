using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class AuthorService
{
    private readonly HttpClient httpClient;

    public AuthorService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<AuthorWithBooks> FetchAuthorAndBooksAsync(Guid authorId)
    {
        var authorsWithBooks = await FetchMultipleAuthorsAndBooksAsync(new List<Guid> { authorId });
        return authorsWith Books.FirstOrDefault();
    }

    public async Task<List<AuthorWithBooks>> FetchMultipleAuthorsAndBooksAsync(List<Guid> authorIds)
    {
        var requestUri = $"api/authors/withBooks?ids={string.Join(",", authorIds)}"; // Fixed typo in URI

        var httpResponse = await httpClient.GetAsync(requestUri);
        if (httpResponse.IsSuccessStatusCode)
        {
            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var authorsWithBooksList = JsonConvert.DeserializeObject<List<AuthorWithBooks>>(responseContent);
            return authorsWithBooksList ?? new List<AuthorWithBooks>();
        }

        throw new Exception("Failed to retrieve author and book details.");
    }
}

namespace MyApplication.Models
{
    public class AuthorWithBooks
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public List<Book> Books { get; set; }
    }

    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
    }
}