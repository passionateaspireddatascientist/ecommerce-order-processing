# E-Commerce Order Processing API

A basic but professional **ASP.NET Core Web API** project for product management, cart management, order placement, and order status tracking.

## Tech Stack

- C#
- ASP.NET Core Web API
- SQL Server
- Entity Framework Core
- LINQ
- Swagger
- Postman
- Git/GitHub

## Features

- User creation
- Product CRUD operations
- Product search
- Add items to cart
- Update cart quantity
- Remove items from cart
- Place order from cart
- Reduce product stock after order placement
- View order history by user
- Update order status
- Cancel order
- Basic validation and exception handling

## Run Locally

```bash
git clone https://github.com/your-username/ecommerce-order-processing-api.git
cd ecommerce-order-processing-api
dotnet restore
dotnet ef database update
dotnet run
```

Swagger:

```text
https://localhost:5001/swagger
```

## Update Connection String

Edit `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EcommerceOrderDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Main API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/users` | Create user |
| GET | `/api/products` | Get products |
| POST | `/api/products` | Add product |
| PUT | `/api/products/{id}` | Update product |
| POST | `/api/cart` | Add product to cart |
| GET | `/api/cart/{userId}` | View cart |
| POST | `/api/orders` | Place order |
| GET | `/api/orders/user/{userId}` | Get user order history |
| PUT | `/api/orders/{id}/status` | Update order status |
