using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.BussinesLogic.mainBL;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.Support;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class AdminAPI
    {
        internal ResponseSupport GetViewPortAction()
        {
            try
            {
                using (var db = new SupportContext())
                {
                    var ViewPorts = db.SupportMesages.ToList();
                    return new ResponseSupport()
                    {
                        Status = true,
                        SupportMesages = ViewPorts.Select(s => new USupportForm
                        {
                            message = s.message,
                            name = s.name,
                            subject = s.subject,
                            SupportUser = s.SupportUser,
                            email = s.email,
                            MessageTime = s.MessageTime
                        }).ToList() // Явное преобразование в список
                    };
                }
            }
            catch (Exception ex)
            {
                // Возвращаем объект ResponseSupport с ошибкой
                return new ResponseSupport()
                {
                    Status = false,
             
                };
            }
        }


        internal ResponseSupport MessageToSupportAction(int userId,USupportForm supportForms)
        {
            var message = supportForms.message;
            try
            {
                using (var db = new SupportContext())
                {
                    var user = db.Users.FirstOrDefault(u => u.Id == userId);
                    
                    if (user == null || message == null)
                    {
                        return new ResponseSupport { Status = false, StatusMessage = "User or product not found" };
                    }

                  
                      
                        var supportForm = new USupportForm
                        {
                            SupportUserId = userId,
                            name = supportForms.name,
                            email = supportForms.email,
                            message = supportForms.message,
                            subject = supportForms.subject,
                            MessageTime = supportForms.MessageTime,
                         

                        };

                        db.SupportMesages.Add(supportForm);
                        db.SaveChanges();

                        return new ResponseSupport { Status = true };
                    }


                
            }
            catch (Exception ex)
            {
                return new ResponseSupport { Status = false ,StatusMessage = ex.Message};
            }
        }


        internal ResponseAddProduct AddAdminActionProduct(Product products)
        {
            if (products == null)
            {
                string Error = "Error to add product";
                return new ResponseAddProduct { Status = false, MessageError = Error };
            }

            // Рассчитываем цену товара с учетом скидки
            decimal priceWithDiscount = products.Price - (products.Price * products.Discount / 100);

            var product = new ProductDbTable()
            {
                Name = products.Name,
                Price = priceWithDiscount, // Используем цену с учетом скидки
                ProductType = products.ProductType,
                Category = products.Category,
                Description = products.Description,
                Id = products.Id,
                ImageUrl = products.ImageUrl,
                QuantityProd = products.Quantity,
                Discount = products.Discount,
            };

            using (var db = new ProductContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }

            return new ResponseAddProduct { Status = true };
        }


        internal ResponseGetProducts GetAllProducts()
        {
            try
            {
                using (var db = new ProductContext())
                {
                    var products = db.Products.ToList(); // Получаем все продукты из БД
                    return new ResponseGetProducts
                    {
                        Status = true,
                        Products = products.Select(p => new Product
                        {
                            // Здесь предполагается преобразование из ProductDbTable в Product
                            Id = p.Id,
                            Name = p.Name,
                            Price = p.Price,
                            ProductType = p.ProductType,
                            Category = p.Category,
                            Description = p.Description,
                            ImageUrl = p.ImageUrl,
                            Quantity = p.QuantityProd,
                            Discount = p.Discount,
                        }).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseGetProducts { Status = false, Message = ex.Message };
            }
           
        }
        public ResponseFilterProducts GetFilteredProductsAction(string category, string productType, decimal lowerPrice, decimal upperPrice, string sorting)
        {
            try
            {
                List<ProductDbTable> filteredProductsList = new List<ProductDbTable>(); // Создаем список для сохранения отфильтрованных продуктов

                using (var db = new ProductContext())
                {
                    var filteredProducts = db.Products.Where(p =>
                        (string.IsNullOrEmpty(category) || p.Category.ToLower() == category.ToLower()) &&
                        (string.IsNullOrEmpty(productType) || p.ProductType.ToLower() == productType.ToLower()) &&
                        (lowerPrice <= p.Price && p.Price <= upperPrice)).ToList(); // Добавляем фильтрацию по цене

                    filteredProductsList.AddRange(filteredProducts); // Добавляем отфильтрованные продукты в список

                    // Применяем сортировку
                    switch (sorting)
                    {
                        case "priceAsc":
                            filteredProductsList = filteredProductsList.OrderBy(p => p.Price).ToList();
                            break;
                        case "priceDesc":
                            filteredProductsList = filteredProductsList.OrderByDescending(p => p.Price).ToList();
                            break;
                        case "nameAsc":
                            filteredProductsList = filteredProductsList.OrderBy(p => p.Name).ToList();
                            break;
                        case "nameDesc":
                            filteredProductsList = filteredProductsList.OrderByDescending(p => p.Name).ToList();
                            break;
                        default:
                            // Сортировка по умолчанию (можете указать свой вариант)
                            filteredProductsList = filteredProductsList.OrderBy(p => p.Name).ToList();
                            break;
                    }

                    if (filteredProductsList.Any())
                    {
                        // Если найдены отфильтрованные продукты, возвращаем успешный результат
                        return new ResponseFilterProducts
                        {
                            Success = true,
                            FilteredProducts = filteredProductsList
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
                // Обработка исключений, если они возникают
                return new ResponseFilterProducts
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }


        internal ResponseToDeleteProduct DeleteProductAction(Product productToDelete)
        {
            if (productToDelete == null || productToDelete.Id == 0)
            {
                return new ResponseToDeleteProduct { Status = false, MessageError = "Product to delete is not specified correctly." };
            }

            try
            {
                using (var db = new ProductContext())
                {
                    var product =  db.Products.Find(productToDelete.Id); // Ищем товар асинхронно
                    if (product == null)
                    {
                        return new ResponseToDeleteProduct { Status = false, MessageError = "Product not found." };
                    }

                    db.Products.Remove(product);
                    db.SaveChanges();// Асинхронно сохраняем изменения в базе данных

                    return new ResponseToDeleteProduct { Status = true, MessageError = "Product successfully deleted." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseToDeleteProduct { Status = false, MessageError = $"Error deleting product: {ex.Message}" };
            }
        }


        internal ResponseToEditProduct EditAdminActionProduct(Product product)
        {
            if (product == null )
            {
                return new ResponseToEditProduct { Status = false, MessageError = "Недопустимый продукт для обновления." };
            }

            try
            {
                using (var db = new ProductContext())
                {
                   
                    // Поиск существующего продукта по ID
                    var existingProduct = db.Products.Find(product.Id);
                    while (existingProduct == null)
                    {
                   
                        existingProduct = db.Products.Find(product.Id);
                       
               
                        return new ResponseToEditProduct { Status = false, MessageError = "Продукт не найден." };
                    }

                    // Обновление полей продукта
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.ProductType = product.ProductType;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Category = product.Category;
                    existingProduct.Description = product.Description;
                    existingProduct.QuantityProd = product.Quantity;

                    // Сохранение изменений
                    db.SaveChanges();

                    return new ResponseToEditProduct { Status = true, MessageError = "Продукт успешно обновлен." };
                }
            }
            catch (Exception ex)
            {
                // В случае ошибки возвращаем статус false и сообщение об ошибке
                return new ResponseToEditProduct { Status = false, MessageError = $"Ошибка обновления продукта: {ex.Message}" };
            }
        }



    }
}
