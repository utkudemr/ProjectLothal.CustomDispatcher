
# Lothal.Mediator

**Lothal.Mediator** is a lightweight, MediatR-inspired in-process messaging library for .NET.  

---

## 📦 Installation

Install via NuGet Package Manager:

```bash
dotnet add package Lothal.Mediator
```

---

## 🚀 Getting Started

### 1. Define a request

```csharp
public class GetUserByIdQuery : IRequest<User>
{
    public int Id { get; set; }
}
```

### 2. Implement a handler

```csharp
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
{
    public Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        // Your logic here
        return Task.FromResult(new User { Id = request.Id, Name = "John Doe" });
    }
}
```

### 3. Register handlers

```csharp
builder.Services.AddLothalMediator(typeof(Program)); // or any marker type from your handler assembly
```

### 4. Send a request

```csharp
var user = await _mediator.Send(new GetUserByIdQuery { Id = 1 });
```

---

## ✅ Features

- Simple and familiar `IRequest` / `IRequestHandler` pattern
- Reflection-based handler discovery
- Lightweight with no dependencies
- Suitable for modular applications and microservices

---

## 📄 License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).
