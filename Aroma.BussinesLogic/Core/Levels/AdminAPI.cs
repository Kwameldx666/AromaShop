using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Core.Levels
{
    public class AdminAPI
    {


  

        public ResponseAddProduct AddAdminActionProduct(Product products)
        {
            if (products == null) 
            {
                string Error = "Error to add product";
                return new ResponseAddProduct { Status = false ,MessageError = Error};
            }

            var product = new ProductDbTable()
            {
                Name = products.Name,
                Price = products.Price,
                ProductType = products.ProductType,
                Category = products.Category,
                Description = products.Description,
                Id = products.Id
            };
            using(var db = new ProductContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
            return new ResponseAddProduct { Status = true };
        }

        public ResponseGetProducts GetAllProducts()
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
                            Description = p.Description
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

        public ResponseToEditProduct EditAdminActionProduct(Product product)
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
                        product.Id++;
                        existingProduct = db.Products.Find(product.Id);
                       
                        if (product.Id > 100)
                        return new ResponseToEditProduct { Status = false, MessageError = "Продукт не найден." };
                    }

                    // Обновление полей продукта
                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price;
                    existingProduct.ProductType = product.ProductType;
                    existingProduct.Category = product.Category;
                    existingProduct.Description = product.Description;

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
