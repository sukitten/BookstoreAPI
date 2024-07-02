public class AuthorService
{
    private readonly HttpClient _httpClient;

    public AuthorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthorWithBooks> GetAuthorDetailsWithBooksAsync(Guid authorId)
    {
        var response = await _httpClient.GetAsync($"api/authors/{authorId}/withBooks");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var authorDetails = JsonConvert.DeserializeObject<AuthorWithBooks>(content);
            return authorDetails;
        }

        throw new Exception("Failed to fetch author details with books.");
    }
}
```

```csharp
using System;
using System.Collections.Generic;

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
        // Other book-related properties...
    }
}