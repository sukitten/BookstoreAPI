## Bookstore API

### Description
The Bookstore API is a backend application designed to manage an online bookstore. It provides endpoints for managing books, authors, and orders. The project is built using C# and ASP.NET Core for the backend, with SQLite for data storage.

### Features
- **Books Management**: Create, read, update, and delete book records.
- **Authors Management**: Create, read, update, and delete author records.
- **Orders Management**: Create, read, update, and delete orders.
- **Database**: Uses SQLite for data storage.

### Installation

#### Prerequisites
- .NET 5.0 SDK or later
- SQLite

#### Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/sukitten/BookstoreAPI.git
   cd BookstoreAPI
   ```

2. Restore the dependencies:
   ```sh
   dotnet restore
   ```

3. Update the database:
   ```sh
   dotnet ef database update
   ```

4. Run the application:
   ```sh
   dotnet run
   ```

The API will be available at `http://localhost:5000`.

### API Endpoints

#### Books
- `GET /books`: Retrieve a list of all books.
- `POST /books`: Create a new book.
- `PUT /books/{id}`: Update an existing book.
- `DELETE /books/{id}`: Delete a book.

#### Authors
- `GET /authors`: Retrieve a list of all authors.
- `POST /authors`: Create a new author.
- `PUT /authors/{id}`: Update an existing author.
- `DELETE /authors/{id}`: Delete an author.

#### Orders
- `GET /orders`: Retrieve a list of all orders.
- `POST /orders`: Create a new order.
- `PUT /orders/{id}`: Update an existing order.
- `DELETE /orders/{id}`: Delete an order.

### Configuration

The application settings are stored in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=online_bookstore.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Contributing
Contributions are welcome! Please fork the repository and submit a pull request for any improvements.

### Acknowledgments
- This project was developed using ASP.NET Core and SQLite.
- Thanks to the open-source community for their invaluable contributions.
