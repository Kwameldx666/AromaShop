# 🌸 AromaShop - Premium E-Commerce Platform

<div align="center">

![AromaShop Logo](Aroma/img/logo.png)

*Aroma - лучший выбор! (Aroma - the best choice!)*

[![ASP.NET](https://img.shields.io/badge/ASP.NET-MVC-blue?style=for-the-badge&logo=.net)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-7.0+-purple?style=for-the-badge&logo=c-sharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity%20Framework-6.4.4-green?style=for-the-badge)](https://docs.microsoft.com/en-us/ef/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-4.0-purple?style=for-the-badge&logo=bootstrap)](https://getbootstrap.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

</div>

## 📋 About The Project

**AromaShop** is a modern, full-featured e-commerce platform built with ASP.NET MVC. It provides a comprehensive online shopping experience with advanced features for both customers and administrators. The platform combines elegant design with powerful functionality to create the perfect shopping environment.

### ✨ Key Features

🛍️ **E-Commerce Core**
- Complete product catalog with categories
- Advanced shopping cart functionality
- Secure checkout process
- Order tracking system
- Payment integration ready

👥 **User Management** 
- User registration and authentication
- User profiles and account management
- Password recovery system
- Role-based access control

🎯 **Product Management**
- Product CRUD operations
- Category management
- Product reviews and ratings
- Image gallery support
- Inventory tracking

📊 **Admin Dashboard**
- Comprehensive admin panel
- User management system
- Product analytics
- Order management
- Sales reporting

📝 **Content Management**
- Blog system
- Content pages
- Support ticket system
- Contact forms

🤖 **Advanced Features**
- Machine Learning integration
- Search functionality
- Responsive design
- SEO optimization
- Multi-language support

## 🚀 Tech Stack

### Backend
- **Framework:** ASP.NET MVC 5
- **Language:** C# (.NET Framework 4.7.2)
- **ORM:** Entity Framework 6.4.4
- **Database:** SQL Server
- **ML:** Microsoft.ML 3.0.1
- **Mapping:** AutoMapper 6.0.2
- **Logging:** Serilog 3.1.1

### Frontend
- **UI Framework:** Bootstrap 4
- **JavaScript:** jQuery 3.7.1
- **Styling:** SCSS/CSS3
- **Icons:** Font Awesome, Themify Icons
- **Components:** Owl Carousel, Nice Select

### Architecture
- **Pattern:** Model-View-Controller (MVC)
- **Structure:** Layered Architecture
- **Business Logic:** Separate BLL project
- **Domain Models:** Domain-driven design

## 📁 Project Structure

```
AromaShop/
├── 📂 Aroma/                    # Main web application
│   ├── 📂 Controllers/          # MVC Controllers
│   ├── 📂 Models/              # View Models
│   ├── 📂 Views/               # Razor Views
│   ├── 📂 Content/             # CSS and Stylesheets
│   ├── 📂 Scripts/             # JavaScript files
│   ├── 📂 img/                 # Images and assets
│   └── 📂 vendors/             # Third-party libraries
├── 📂 Aroma.BussinesLogic/     # Business Logic Layer
│   ├── 📂 DBModel/             # Database Models
│   └── 📂 Services/            # Business Services
├── 📂 Aroma.Domain/            # Domain Entities
│   ├── 📂 Entities/            # Domain Models
│   └── 📂 Enums/               # Enumerations
└── 📂 Aroma.Helpers/           # Utility Classes
```

## ⚡ Quick Start

### Prerequisites

Before running the project, ensure you have:

- **Visual Studio 2017+** or **Visual Studio Code**
- **.NET Framework 4.7.2** or higher
- **SQL Server** (LocalDB or full version)
- **IIS Express** (included with Visual Studio)
- **Git** for version control

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Kwameldx666/AromaShop.git
   cd AromaShop
   ```

2. **Open the solution**
   ```bash
   # Open in Visual Studio
   start "Lab_TW .sln"
   
   # Or use Visual Studio Code
   code .
   ```

3. **Restore NuGet packages**
   ```bash
   nuget restore
   ```

4. **Update database connection**
   
   Edit `Web.config` in the Aroma project:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Data Source=(LocalDB)\MSSQLLocalDB;..."
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

5. **Update database**
   ```bash
   # Run in Package Manager Console
   Update-Database
   ```

6. **Build and run**
   ```bash
   # Press F5 in Visual Studio or
   dotnet build
   ```

### 🔧 Configuration

#### Application Settings

Configure the application in `Web.config`:

```xml
<appSettings>
  <add key="webpages:Version" value="3.0.0.0" />
  <add key="webpages:Enabled" value="false" />
  <add key="ClientValidationEnabled" value="true" />
  <add key="UnobtrusiveJavaScriptEnabled" value="true" />
</appSettings>
```

#### Database Configuration

The project uses Entity Framework Code First. Run these commands to set up the database:

```bash
# Enable Migrations
Enable-Migrations

# Add Initial Migration
Add-Migration InitialCreate

# Update Database
Update-Database
```

## 📖 Usage Examples

### Adding a New Product

```csharp
public ActionResult AddProduct(Product model)
{
    if (ModelState.IsValid)
    {
        var product = new Product
        {
            Name = model.Name,
            Price = model.Price,
            Description = model.Description,
            CategoryId = model.CategoryId
        };
        
        _productService.AddProduct(product);
        return RedirectToAction("Index");
    }
    
    return View(model);
}
```

### User Authentication

```csharp
[HttpPost]
public ActionResult Login(LoginData model)
{
    if (ModelState.IsValid)
    {
        var user = _userService.ValidateUser(model.Email, model.Password);
        if (user != null)
        {
            FormsAuthentication.SetAuthCookie(user.Email, false);
            return RedirectToAction("Index", "Home");
        }
    }
    
    return View(model);
}
```

## 🎨 Screenshots

### Homepage
*Modern and elegant storefront design*
![Homepage](Aroma/img/home/hero-banner.png)

### Product Catalog
*Clean and intuitive product browsing experience with advanced filtering*

### Admin Dashboard
*Powerful administration tools for comprehensive store management*

### Shopping Cart
*Streamlined checkout process with secure payment integration*

> **Note:** This project includes a complete set of professional UI components and responsive design elements for optimal user experience across all devices.

## 🌐 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/` | Homepage |
| GET | `/Product/Category/{id}` | Products by category |
| POST | `/Product/AddToCart` | Add item to cart |
| GET | `/Account/Login` | User login page |
| POST | `/Account/Register` | User registration |
| GET | `/Admin/Products` | Admin product management |

## 🔒 Security Features

- **Authentication:** Forms-based authentication
- **Authorization:** Role-based access control
- **Data Protection:** SQL injection prevention
- **Input Validation:** Client and server-side validation
- **CSRF Protection:** Anti-forgery tokens

## 🧪 Testing

To run the application:

1. **Build the solution** in Visual Studio
2. **Set Aroma as startup project**
3. **Press F5** to run with debugging
4. **Navigate to** `https://localhost:44353/`

### Test Accounts

Create test accounts through the registration system or seed data:

- **Admin User:** Configure in UserContext.cs
- **Regular User:** Register through the web interface

## 🤝 Contributing

We welcome contributions! Please follow these steps:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/AmazingFeature`)
3. **Commit** your changes (`git commit -m 'Add some AmazingFeature'`)
4. **Push** to the branch (`git push origin feature/AmazingFeature`)
5. **Open** a Pull Request

### Coding Standards

- Follow **C# coding conventions**
- Use **meaningful variable names**
- Add **XML documentation** for public methods
- Include **unit tests** for new features
- Ensure **responsive design** for UI changes

## 📚 Documentation

- [ASP.NET MVC Documentation](https://docs.microsoft.com/en-us/aspnet/mvc/)
- [Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)

## 🐛 Known Issues

- [ ] Image upload size limitation
- [ ] Email service configuration needed
- [ ] Payment gateway integration pending

## 📝 Changelog

### Version 1.0.0
- Initial release
- Core e-commerce functionality
- User authentication system
- Admin panel
- Blog system

## 🆘 Support

If you encounter any issues or have questions:

1. **Check** the [Issues](https://github.com/Kwameldx666/AromaShop/issues) page
2. **Create** a new issue with detailed description
3. **Contact** the development team

## 👨‍💻 Authors

- **Kwameldx666** - *Initial work* - [GitHub](https://github.com/Kwameldx666)

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Bootstrap Team** for the excellent CSS framework
- **Font Awesome** for the beautiful icons
- **Microsoft** for ASP.NET MVC and Entity Framework
- **Open Source Community** for the amazing libraries

---

<div align="center">

**Made with ❤️ for the e-commerce community**

[⬆ Back to top](#-aromashop---premium-e-commerce-platform)

</div>

---

## 🌍 Русская версия

### О проекте

**AromaShop** - современная полнофункциональная платформа электронной коммерции, построенная на ASP.NET MVC. Предоставляет комплексный опыт онлайн-покупок с продвинутыми функциями как для клиентов, так и для администраторов.

### Основные возможности

- 🛒 Полный каталог товаров с категориями
- 👤 Система управления пользователями
- 📊 Административная панель
- 💳 Безопасная система оплаты
- 📱 Адаптивный дизайн
- 🔍 Система поиска
- ⭐ Отзывы и рейтинги

### Быстрый старт

1. **Клонируйте репозиторий**
   ```bash
   git clone https://github.com/Kwameldx666/AromaShop.git
   ```

2. **Откройте в Visual Studio**
   ```bash
   start "Lab_TW .sln"
   ```

3. **Восстановите пакеты NuGet**
4. **Обновите базу данных**
5. **Запустите проект (F5)**

### Поддержка

Если у вас есть вопросы или предложения, создайте [Issue](https://github.com/Kwameldx666/AromaShop/issues) в репозитории.
