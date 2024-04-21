using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponce;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Aroma.Helpers;
using AutoMapper;
using Aroma.Domain.Entities.Product.DBModel;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class UserAPI
    {
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
                var user = db.Users.FirstOrDefault(u => u.Username == data.credential);
                if (user == null)
                {
                    user = db.Users.FirstOrDefault(u => u.Email == data.credential);

                }

                string pass = LoginHelper.HashGen(data.password);
                if (user != null && user.Password == pass)
                {
                    if (user.Level == UserRole.Admin)
                    {
                        return new RResponseData { Status = true, AdminMod = true };
                    }

                    if (user.Level == UserRole.Moderator)
                    {
                        return new RResponseData { Status = true, ModeratorMod = true };
                    }

                    if (user.Level == UserRole.None)
                    {
                        return new RResponseData { Status = false, ResponseMessage = "You have banned" };
                    }
                    // Аутентификация успешна
                    return new RResponseData { Status = true };
                }

                // Если пользователь не найден или пароль не совпадает
                return new RResponseData { Status = false };
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




            if (User.Password != data.AcceptPassword)
            {
                string errorPassword = "Пароли не совпадают";
                return new URegisterResp { Status = false, ResponseMessage = errorPassword };
            }


            User.Password = LoginHelper.HashGen(User.Password);


            using (var db = new UserContext())
            {
                db.Users.Add(User);
                db.SaveChanges();
            }

            return new URegisterResp { Status = true };
        }


        internal HttpCookie Cookie(string loginCredential)
        {
            var apiCookie = new HttpCookie("X-KEY")
            {
                Value = CookieGenerator.Create(loginCredential)
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
                    curent.ExpireTime = DateTime.Now.AddMinutes(60);
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
                        ExpireTime = DateTime.Now.AddMinutes(60)
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

        public ResponseFilterProducts GetFilteredProductsAction(string category, string brand, string color)
        {
            try
            {
                using (var db = new ProductContext())
                {
                    // Используем LINQ для фильтрации продуктов по заданным критериям
                    var filteredProducts = db.Products.Where(p =>
                        (string.IsNullOrEmpty(category) || p.Category.ToLower() == category.ToLower()) &&
                        (string.IsNullOrEmpty(brand) || p.ProductType.ToLower() == brand.ToLower())).ToList();

                    if (filteredProducts != null && filteredProducts.Any())
                    {
                        // Если найдены отфильтрованные продукты, возвращаем успешный результат
                        return new ResponseFilterProducts
                        {
                            Success = true,
                            FilteredProducts = filteredProducts
                        };
                    }
                    else
                    {
                        // Если не найдено ни одного продукта по заданным критериям, возвращаем успешный результат с пустым списком
                        return new ResponseFilterProducts
                        {
                            Success = true,
                            FilteredProducts = new List<ProductDbTable>()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки при выполнении запроса фильтрации
                Console.WriteLine($"Error occurred during product filtering: {ex.Message}");
                return new ResponseFilterProducts
                {
                    Success = false,
                    ErrorMessage = "An error occurred while filtering products."
                };
            }
        }

        internal ResponseAddOrder purchaseProduct(int userId, int productId, int quantity)
        {
            try
            {
                using (var db = new UserContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);

                    if (user == null || product == null)
                    {
                        return new ResponseAddOrder { Success = false, MessageError = "User or product not found" };
                    }

                    // Проверяем, есть ли у пользователя уже заказ с этим товаром
                    var existingOrder = db.Orders.FirstOrDefault(o => o.UserId == userId && o.ProductId == productId);

                    if (existingOrder != null)
                    {
                        // Update existing order quantity
                        existingOrder.QuantityOrder += quantity;
                        existingOrder.TotalPrice = product.Price * existingOrder.QuantityOrder;
                        db.Entry(existingOrder).State = EntityState.Modified;
                        db.SaveChanges();

                        return new ResponseAddOrder { Success = true, Order = existingOrder };
                    }
                    else
                    {
                        // Create a new order
                        int totalPrice = product.Price * quantity;
                        var order = new Order
                        {
                            UserId = userId,
                            ProductId = productId,
                            QuantityOrder = quantity,
                            TotalPrice = totalPrice,
                            OrderDate = DateTime.UtcNow,
                            ProductType = product.ProductType,
                            TotalAmount = totalPrice
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
                    // Извлекаем информацию о продукте, а не заказе
                    var product = db.Products.FirstOrDefault(p => p.Id == productId);
                    if (product == null)
                    {
                        return new ResponseViewProduct { Status = false, Message = "Product not found" };
                    }

                    // Создаем объект ResponseViewProduct с данными о продукте
                    var productInfo = new ResponseViewProduct
                    {
                        ProductId = productId,
                        Price = product.Price,
                        Description = product.Description,
                        Name = product.Name,
                        Category = product.ProductType,
                        Quantity = product.QuantityProd,
                        ImageUrl = product.ImageUrl
                    };

                    return productInfo;
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
                                   .FirstOrDefault(o => o.UserId == userId && o.ProductId == productId);

                    if (order != null)
                    {
                        // Обновляем количество товара и вычисляем новую суммарную стоимость заказа
                        order.QuantityOrder = quantityOrder;
                        order.TotalPrice = order.Product.Price * quantityOrder;
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
                                              .Where(o => o.UserId == userId)
                                              .ToListAsync();

                    if (userOrders.Any())
                    {
                        decimal totalAmountOfOrders = userOrders.Sum(o => o.TotalAmount);

                        // Обновляем баланс пользователя
                        var userBalance = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
                        if (userBalance != null)
                        {
                            userBalance.Balance -= totalAmountOfOrders;
                        }

                        // Удаляем товары (заказанные товары)
                        foreach (var order in userOrders)
                        {
                            // Находим все товары, связанные с текущим заказом
                            var orderItems = await db.Orders.Where(oi => oi.OrderId == order.OrderId).ToListAsync();

                            // Удаляем все товары из базы данных
                            db.Orders.RemoveRange(orderItems);
                        }

                        // Удаляем все заказы пользователя из контекста
                        db.Orders.RemoveRange(userOrders);

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
                                        .FirstOrDefaultAsync(o => o.UserId == userId && o.ProductId == productId);

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


        internal ResponseGetOrders GetOrders()
        {
            try
            {
                using (var db = new OrderContext())
                {
                    var orders = db.Orders.ToList(); // Получаем все продукты из БД
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
                            ProductType = p.ProductType




                        }).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseGetOrders { Status = false, Message = ex.Message };
            }
        }

        internal ResponseGetOrders GetUserOrders(int userId)
        {
            try
            {
                using (var db = new OrderContext())
                {
                    // Получаем все заказы данного пользователя с указанным productId
                    var orders = db.Orders
                                    .Where(o => o.UserId == userId )
                                    .ToList();

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
                        }); ;
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
    







