using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Aroma.Helpers;
using AutoMapper;

using Aroma.Domain.Entities.Product.DBModel;
using Aroma.BussinesLogic.mainBL;
using Aroma.Domain.Enums.OrdersStatus;
using Microsoft.Ajax.Utilities;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.Rating;
using System.Web.Mvc;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class UserAPI
    {
        public int CountRating { get; private set; }

        internal  ResponseCheckCode CheckEmailAction(URegisterData updateCode)
        {
            using(var db = new UserContext())
            {
                var Code = db.Users.FirstOrDefault(c => c.Code == updateCode.Code);
                if (Code == null)
                {
                    return new ResponseCheckCode { Status = false };
                }
                else 

                        return new ResponseCheckCode { Status = true };
            }
     

        }
        internal ResponseViewProfile ViewProfileAction(int userId)
        {
            try
            {
                using (var db = new UserContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    if (user == null)
                    {
                        return new ResponseViewProfile { Status = false, ErrorMessage = "User not found" };
                    }

                    // Преобразуем данные о пользователе в модель UserProfile
                    var userProfile = new ResponseViewProfile
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        Balance = user.Balance,
                        // Заполняем другие свойства профиля пользователя
                    };

                    return userProfile;

                    
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseViewProfile { Status = false, ErrorMessage = ex.Message };
            }
        }

        internal ResponseEditPassword EditPassword(ULoginData user, string newPassword)
        {
            try
            {
                using (var db = new UserContext())
                {

                    // Поиск существующего продукта по ID
                    var existingProduct = db.Users.Find(user.Id);
                    while (existingProduct == null)
                    {

                        existingProduct = db.Users.Find(user.Id);


                        return new ResponseEditPassword { Status = false, ErrorMessage = "Продукт не найден." };
                    }

                    string password = LoginHelper.HashGen(newPassword);
                    existingProduct.Password = password;

                    


                    // Сохранение изменений
                    db.SaveChanges();

                    return new ResponseEditPassword { Status = true, ErrorMessage = "Продукт успешно обновлен." };
                }
            }

            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseEditPassword { Status = false, ErrorMessage = $"Ошибка обновления продукта: {ex.Message}" };
            }
        }
        internal RResponseData ULASessionCheck(ULoginData data)


        {
            using (var db = new UserContext())
            {
                // Поиск пользователя в базе данных по имени пользователя
                var user = db.Users.FirstOrDefault(u => u.Username == data.credential || u.Email == data.credential);

                string pass = LoginHelper.HashGen(data.password);
                if (user != null && user.Password == pass)
                {
                    switch (user.Level)
                    {
                        case UserRole.Admin:
                            return new RResponseData { Status = true, AdminMod = true };
                        case UserRole.Moderator:
                            return new RResponseData { Status = true, ModeratorMod = true };
                        case UserRole.None:
                            return new RResponseData { Status = false, ResponseMessage = "Your account is banned" };
                        default:
                            return new RResponseData { Status = true }; // Аутентификация успешна
                    }
                }

                // Если пользователь не найден или пароль не совпадает
                return new RResponseData { Status = false, ResponseMessage = "Неверные учетные данные" };
            }
        }

        internal ResponseToEditProfile EditProfileAction(ULoginData updateProfile)
        {
            if (updateProfile == null)
            {
                return new ResponseToEditProfile { Status = false, MessageError = "Недопустимый продукт для обновления." };
            }

            try
            {
                using (var db = new UserContext())
                {

                    // Поиск существующего продукта по ID
                    var existingProduct = db.Users.Find(updateProfile.Id);
                    while (existingProduct == null)
                    {

                        existingProduct = db.Users.Find(updateProfile.Id);


                        return new ResponseToEditProfile { Status = false, MessageError = "Продукт не найден." };
                    }


                    // Обновление полей продукта
                    existingProduct.Username = updateProfile.credential;
                    existingProduct.Email = updateProfile.Email;


                    // Сохранение изменений
                    db.SaveChanges();

                    return new ResponseToEditProfile { Status = true, MessageError = "Продукт успешно обновлен." };
                }
            }

            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseToEditProfile { Status = false, MessageError = $"Ошибка обновления продукта: {ex.Message}" };
            }


        }

        internal URegisterResp RRegisterUpService(URegisterData data)
        {
            // Создаем новую запись пользователя с данными из входного объекта
            var User = new UDbTable
            {
                Username = data.Name,
                Password = data.Password,
                LastIP = data.IP,
                Email = data.Email,
                LastLogin = data.RegDate,
                Level = UserRole.User,
                Balance = data.Balance
            };

            // Проверяем, совпадают ли пароли
            if (User.Password != data.AcceptPassword)
            {
                string errorPassword = "Пароли не совпадают";
                return new URegisterResp { Status = false, ResponseMessage = errorPassword };
            }

            if (User.Email == null || User.Username == null)
            {
                return new URegisterResp { Status = false, ResponseMessage = "Заполните все поля" };
            }

            // Хешируем пароль для безопасного хранения в базе данных
            User.Password = LoginHelper.HashGen(User.Password);

            using (var db = new UserContext())
            {
                // Проверяем, существует ли пользователь с таким же именем
                var UserName = db.Users.FirstOrDefault(u => u.Username == data.Name);
                if (UserName != null)
                {
                    // Если пользователь существует, возвращаем сообщение об ошибке
                    return new URegisterResp { Status = false, ResponseMessage = "Пользователь с таким именем уже существует" };
                }

                // Добавляем нового пользователя в базу данных и сохраняем изменения
                string code = GenerateRandomPasswords.GenerateCode();

                bool emailSent = USendPasswordResetEmail.SendConfirmationEmail(User.Email, code);
                if (emailSent)
                {
                    // Если письмо успешно отправлено, сохраняем код подтверждения в базе данных и создаем пользователя
                    User.Code = code;
                    db.Users.Add(User);
                    db.SaveChanges();
                    return new URegisterResp { Status = true, ResponseMessage = "Письмо с кодом подтверждения отправлено на ваш почтовый адрес" };
                }
                else
                {
                    return new URegisterResp { Status = false, ResponseMessage = "Ошибка при отправке письма на ваш почтовый адрес. Пожалуйста, попробуйте позже." };
                }
            }
        }

        internal HttpCookie Cookie(string loginCredential, bool rememberMe)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginCredential),
                Expires = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60)

            };

            using (var db = new SessionContext())
            {
                Session curent;
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(loginCredential))
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }
                else
                {
                    curent = (from e in db.Sessions where e.Username == loginCredential select e).FirstOrDefault();
                }

                if (curent != null)
                {
                    curent.CookieString = apiCookie.Value;
                    // Установка срока действия куки в зависимости от выбора пользователя
                    curent.ExpireTime = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60);
                    using (var todo = new SessionContext())
                    {
                        todo.Entry(curent).State = EntityState.Modified;
                        todo.SaveChanges();
                    }
                }
                else
                {
                    db.Sessions.Add(new Session
                    {
                        Username = loginCredential,
                        CookieString = apiCookie.Value,
                        // Установка срока действия куки в зависимости от выбора пользователя
                        ExpireTime = rememberMe ? DateTime.Now.AddDays(30) : DateTime.Now.AddMinutes(60)
                    });
                    db.SaveChanges();
                }
            }

            return apiCookie;
        }








        internal UserMinimal UserCookie(string cookie)
        {
            Session session;
            UDbTable curentUser;

            using (var db = new SessionContext())
            {
                session = db.Sessions.FirstOrDefault(s => s.CookieString == cookie && s.ExpireTime > DateTime.Now);
            }

            if (session == null) return null;
            using (var db = new UserContext())
            {
                var validate = new EmailAddressAttribute();
                if (validate.IsValid(session.Username))
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Email == session.Username);
                }
                else
                {
                    curentUser = db.Users.FirstOrDefault(u => u.Username == session.Username);
                }
            }

            if (curentUser == null) return null;
            Mapper.Initialize(cfg => cfg.CreateMap<UDbTable, UserMinimal>());
            var userminimal = Mapper.Map<UserMinimal>(curentUser);

            return userminimal;
        }
        internal ResponseLogout LogoutUser()
        {
            var response = new ResponseLogout { Status = false };

            try
            {
                var httpContext = HttpContext.Current;
                if (httpContext != null && httpContext.Request.Cookies["X-KEY"] != null)
                {
                    var cookie = new HttpCookie("X-KEY")
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    response.Status = true;
                    response.Message = "User successfully logged out.";
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = $"An error occurred: {ex.Message}";
            }
            return response;
        }

     
        
        public ResponseToEditProfile ForgotPasswordAction(ULoginData updatePassword)
        {
            string newPassword = GenerateRandomPasswords.GenerateRandomPassword(); // Генерация нового пароля
            using (var db = new UserContext())
            {
                var Email = db.Users.FirstOrDefault(e => e.Email == updatePassword.Email);
                if (Email == null)
                {
       
                    return new ResponseToEditProfile { Status = false, MessageError = "Ошибка при обновления пароля" };
                }
                else
                {
                    var password = LoginHelper.HashGen(newPassword);
                    Email.Password = password;
                    db.SaveChanges();
                }
            }

            bool emailSent = USendPasswordResetEmail.SendPasswordResetEmail(updatePassword.Email, newPassword);
            if (emailSent)
            {
                return new ResponseToEditProfile { Status = true };
            }
            else
                return new ResponseToEditProfile { Status = false ,MessageError = "Ошибка при обновления пароля"};
        }

        internal ResponseAddOrder purchaseProduct(int userId, int productId, int quantity)
        {
            try
            {
                using (var db = new UserContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    var existingOrder = db.Orders.FirstOrDefault(o => o.UserId == userId && o.ProductId == productId && o.orderStatus != Domain.Enums.OrdersStatus.OrderStatus.Successful);

                    if (user == null || product == null)
                    {
                        return new ResponseAddOrder { Success = false, MessageError = "User or product not found" };
                    }

                    if (existingOrder != null)
                    {
                        // Update existing order quantity
                        existingOrder.Product.Quantity++;
                        existingOrder.QuantityOrder += quantity;
                        existingOrder.TotalPrice = product.PriceWithDiscount * existingOrder.QuantityOrder;
                        db.Entry(existingOrder).State = EntityState.Modified;
                        existingOrder.orderStatus = Domain.Enums.OrdersStatus.OrderStatus.Pending;
                        db.SaveChanges();

                        return new ResponseAddOrder { Success = true, Order = existingOrder };
                    }
                    else
                    {
                        // Create a new order
                        decimal totalPrice = product.PriceWithDiscount * quantity;
                        var order = new Order
                        {
                            UserId = userId,
                            ProductId = productId,
                            QuantityOrder = quantity,
                            TotalPrice = totalPrice,
                            OrderDate = DateTime.UtcNow,
                            ProductType = product.ProductType,
                            TotalAmount = totalPrice,
                            orderStatus = Domain.Enums.OrdersStatus.OrderStatus.Pending,

                        };

                        db.Orders.Add(order);
                        db.SaveChanges();

                        return new ResponseAddOrder { Success = true, Order = order };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseAddOrder { Success = false, MessageError = ex.Message };
            }
        }


        internal async Task<ResponseGetProducts> SearchProductsAction(string searchTerm)
        {
            try
            {
                // Преобразуем поисковой запрос в нижний регистр для поиска по началу имени без учета регистра
                var lowerSearchTerm = searchTerm?.Trim().ToLower();

                // Проверяем, что поисковой запрос не пустой
                if (string.IsNullOrEmpty(lowerSearchTerm))
                {
                    return new ResponseGetProducts() { Status = false };
                }

                Console.WriteLine($"Executing search for products starting with '{lowerSearchTerm}'");

                using (var db = new ProductContext())
                {
                    // Выполняем поиск продуктов, у которых имя начинается с указанного поискового запроса (без учета регистра)
                    var searchResults = await db.Products
                                                .Where(p => p.Name.ToLower().StartsWith(lowerSearchTerm))
                                                .ToListAsync();

                    // Проверяем, что найдены какие-либо продукты
                    if (searchResults != null && searchResults.Any())
                    {
                        return new ResponseGetProducts() { Status = true, ProductsV2 = searchResults };
                    }
                    else
                    {
                        // Если продукты не найдены, возвращаем успешный статус, но с пустым списком продуктов
                        return new ResponseGetProducts() { Status = true };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred during product search: {ex.Message}");
                // В случае ошибки возвращаем статус ошибки
                return new ResponseGetProducts() { Status = false };
            }
        }

        internal ResponseViewProduct ViewProductInfoAction(int userId, int productId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Извлекаем информацию о продукте
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    if (product == null)
                    {
                        return new ResponseViewProduct { Status = false, Message = "Product not found" };
                    }

                    // Увеличиваем счетчик просмотров продукта
                    product.View++;
                    db.SaveChanges(); // Сохраняем изменения в базе данных

                    // Получаем среднюю оценку товара
                    double averageRating = db.Rating
     .Where(r => r.ProductId == productId) // Отфильтровываем оценки по идентификатору продукта
     .Select(r => r.Rating) // Выбираем только оценки
     .DefaultIfEmpty(0) // По умолчанию 0, если оценок нет
     .Average(); // Вычисляем среднее значение
                    product.AverageRating = averageRating;

                    // Сохраняем изменения в базе данных
                    db.SaveChanges();

                    var IsPurchased_ = db.Products.Where(p => p.orderStatus == OrderStatus.Successful);
                    if (IsPurchased_ != null)
                    {
                        var ProdStat = new ResponseViewProduct
                        {
                            IsPurchased = true,
                             Status = true,
                            ProductId = productId,
                            Price = product.Price,
                            Description = product.Description,
                            Name = product.Name,
                            Category = product.ProductType,
                            Quantity = product.QuantityProd,
                            ImageUrl = product.ImageUrl,
                            View = product.View,
                            PriceWidthDiscount = product.PriceWithDiscount,
                            AverageRating = averageRating // Добавляем среднюю оценку в ответ
                        };
                        return ProdStat;

                    }
                    else
                    {
                        // Создаем объект ResponseViewProduct с данными о продукте
                        var productInfo = new ResponseViewProduct
                        {
                            IsPurchased = false,
                            Status = true,
                            ProductId = productId,
                            Price = product.Price,
                            Description = product.Description,
                            Name = product.Name,
                            Category = product.ProductType,
                            Quantity = product.QuantityProd,
                            ImageUrl = product.ImageUrl,
                            View = product.View,
                            PriceWidthDiscount = product.PriceWithDiscount,
                            AverageRating = averageRating // Добавляем среднюю оценку в ответ
                        };
                        return productInfo;
                    }

                  
                }
            }
            catch (Exception ex)
            {
                // Логгирование исключения и возврат сообщения об ошибке
                return new ResponseViewProduct { Status = false, Message = "Error retrieving product information: " + ex.Message };
            }
        }



        internal ResponseUpdateQuantityOrders EditOrderQuantity(int userId, int productId, int quantityOrder)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Находим заказ пользователя с заданным userId и productId
                    var order = db.Orders
                                   .FirstOrDefault(o => o.UserId == userId && o.ProductId == productId && o.orderStatus == OrderStatus.Pending);

                    if (order != null)
                    {
                        // Обновляем количество товара и вычисляем новую суммарную стоимость заказа
                        order.QuantityOrder = quantityOrder;
                        order.TotalPrice = order.Product.PriceWithDiscount * quantityOrder;
                        order.TotalAmount =  order.TotalPrice;
                       
                        // Сохраняем изменения в базе данных
                        db.SaveChanges();

                        return new ResponseUpdateQuantityOrders { Status = true, Message = "Order quantity updated successfully." };
                    }
                    else
                    {
                        return new ResponseUpdateQuantityOrders { Status = false, Message = "Order not found for the specified user and product." };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseUpdateQuantityOrders { Status = false, Message = ex.Message };
            }
        }
        internal async Task<ResponseGetOrders> ConfirmPurchaseAction(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Находим все заказы пользователя с заданным userId
                    var userOrders = await db.Orders
                                              .Where(o => o.UserId == userId && o.orderStatus == OrderStatus.Pending)
                                              .ToListAsync();

                    if (userOrders.Any())
                    {
                        decimal totalAmountOfOrders = userOrders.Sum(o => o.TotalAmount);

                        // Обновляем баланс пользователя
                        var userBalance = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                        if (userBalance != null && userBalance.Balance >= totalAmountOfOrders)
                        {
                            userBalance.Balance -= totalAmountOfOrders;
                        }
                        else
                        {
                            return new ResponseGetOrders { Status = false, Message = "Insufficient balance" };
                        }

                 
                        foreach (var order in userOrders)
                        {
                            // Находим все товары, связанные с текущим заказом
                            var orderItems = await db.Orders.Where(oi => oi.OrderId == order.OrderId).ToListAsync();
                            order.orderStatus = Domain.Enums.OrdersStatus.OrderStatus.Successful;
                            foreach (var orderItem in orderItems)
                            {
                                // Обновляем количество товаров на складе
                                var product = await db.Products.FirstOrDefaultAsync(p => p.Id == orderItem.ProductId);
                                if (product != null)
                                {
                             

                                  
                                    product.QuantityProd -= orderItem.QuantityOrder;
                                }
                            }



                            await db.SaveChangesAsync();
                        }

                        // Удаляем все заказы пользователя из контекста
                       

                        // Сохраняем изменения в базе данных
                        await db.SaveChangesAsync();

                        return new ResponseGetOrders { Status = true };
                    }
                    else
                    {
                        // Заказы пользователя не найдены
                        return new ResponseGetOrders { Status = false, Message = "No orders found for the user" };
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }


        internal async Task<ResponseGetOrders> DeleteOrder(int userId, int productId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Находим заказ пользователя с заданным userId и productId
                    var order = await db.Orders
                                        .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId && o.orderStatus == OrderStatus.Pending);

                    if (order != null)
                    {
                        // Если заказ найден, удаляем его из контекста
                        db.Orders.Remove(order);

                        // Асинхронно сохраняем изменения в базе данных
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        // Заказ не найден
                        return new ResponseGetOrders { Status = false, Message = "Order not found" };
                    }

                    return new ResponseGetOrders { Status = true };
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }

        public ResponseGetOrders AddFeedbackAction(int productId, int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Получаем заказы пользователя для указанного товара
                    var order = db.Orders
                        .Include(o => o.Product) // Включаем связанную информацию о товаре
                        .Include(o => o.UDbTable) // Включаем связанную информацию о пользователе
                        .FirstOrDefault(o => o.UserId == userId && o.ProductId == productId && o.orderStatus == OrderStatus.Successful);

                    if (order != null)
                    {
                        return new ResponseGetOrders
                        {
                            Status = true,
                            Price = order.Product.Price,
                            Category = order.Product.Category,
                            Description = order.Product.Description,
                            ProductId = order.ProductId,
                            Name = order.Product.Name,
                        };
                    }
                    else
                    {
                        return new ResponseGetOrders { Status = false, Message = "Order not found for the specified user and product." };
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }


        internal ResponseGetOrders GetOrders(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orders = db.Orders.Where(o => o.UserId == userId && o.orderStatus == OrderStatus.Successful).ToList();

                    // Проверяем, есть ли заказы
                    if (orders.Any())
                    {
                        return new ResponseGetOrders
                        {
                            Status = true,
                            Orders = orders.Select(p => new Order
                            {
                                OrderId = p.OrderId,
                                Product = p.Product,
                                ProductId = p.ProductId,
                                TotalPrice = p.TotalPrice,
                                OrderDate = p.OrderDate,
                                UDbTable = p.UDbTable,
                                QuantityOrder = p.QuantityOrder,
                                TotalAmount = p.TotalAmount,
                                UserId = p.UserId,
                                ProductType = p.ProductType,
                                Feedback = p.Feedback,
                                AverageRating = p.Product.AverageRating,
                                Reting = p.Reting,
                            }).ToList()
                        };
                    }
                    else
                    {
                        return new ResponseGetOrders { Status = false };
                    }
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }



        internal ResponseGetRating GetRating(int userId, int productId, int rating, string review)
        {
            using (var db = new OrderContext())
            {
                // Проверяем, переданы ли рейтинг и отзыв
                if (rating == 0 && review == null)
                {
                    // Получаем первый отзыв для каждого пользователя для данного товара
                    var firstReviewsForUsers = db.Rating
                        .Where(o => o.ProductId == productId && o.Rating != 0)
                        .ToList();

                    // Проверяем, есть ли отзывы для товара
                    bool good = firstReviewsForUsers.Any();

                    return new ResponseGetRating
                    {
                        Good = good,
                        Status = true,
                        View = firstReviewsForUsers.Select(p => new RatingUdbTable
                        {
                            ProductId = p.ProductId,
                            Product = p.Product,
                            Rating = p.Rating,
                            User = p.User,
                            Feedback = p.Feedback
                        }).ToList()
                    };
                }
                else
                {
                    // Получаем все заказы с соответствующим productId
                    var orders = db.Orders.Where(o => o.ProductId == productId).ToList();
                    foreach (var order in orders)
                    {
                        order.Reting = rating;
                    }

                    // Проверяем, есть ли уже оценка от данного пользователя для данного товара
                    var existingRating = db.Rating.FirstOrDefault(r => r.ProductId == productId && r.UserId == userId);

                    if (existingRating != null)
                    {
                        // Если оценка уже есть, обновляем её значение
                        existingRating.Rating = rating;
                        existingRating.Feedback = review;
                    }
                    else
                    {
                        // Иначе добавляем новую оценку
                        var newRating = new RatingUdbTable
                        {
                            ProductId = productId,
                            Rating = rating,
                            Feedback = review,
                            UserId = userId,
                        };

                        db.Rating.Add(newRating);
                    }

                    db.SaveChanges(); // Сохраняем изменения в базе данных

                    return new ResponseGetRating
                    {
                        Status = true
                    };
                }
            }
        }

        internal ResponseGetOrders GetUserOrders(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Получаем все заказы данного пользователя с указанным productId
                    var orders = db.Orders.Where(o => o.UserId == userId && o.orderStatus == OrderStatus.Pending).ToList();

                    // Создаем список для хранения обработанных заказов
                    var processedOrders = new List<Order>();

                    foreach (var order in orders)
                    {
                       
                            // Добавляем заказ в список обработанных заказов
                            processedOrders.Add(new Order
                            {
                                ProductId = order.ProductId,
                                Product = order.Product,
                                QuantityOrder = order.QuantityOrder,
                                TotalAmount = order.TotalAmount,
                                ProductType = order.ProductType,
                                TotalPrice = order.TotalPrice,
                                // Другие необходимые свойства заказа, которые нужно сохранить
                            })
                                ; ;
                        
                    }

                    // Возвращаем успешный результат с обработанными заказами
                    return new ResponseGetOrders
                    {
                        Status = true,
                        Orders = processedOrders
                    };
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }


    }
}
    







