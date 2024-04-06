using Aroma.BussinesLogic;
using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.DBModel.Seed;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using Lab_TW.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_TW.Controllers
{
    public class ProductController : Controller
    {


        internal IProduct _product;

        public ActionResult ProductsAdminPanel()
        {
            // Вызов метода из бизнес-логики для получения всех продуктов
            ResponseGetProducts response = _product.AdminGetAction();
            var viewModelProducts = response.Products.Select(p => new Lab_TW.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ProductType = p.ProductType,
                Description = p.Description
            }).ToList();
            if (response.Status)
            {
                // Если запрос прошёл успешно, отображаем список продуктов
                return View(viewModelProducts);
            }
            else
            {
                // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                ViewBag.ErrorMessage = response.Message;
                return View("Error");
            }
            return View();
        }

        public ProductController()
        {
            var bl = new BussinesLogic();
            _product = bl.AddProductBL();

        }

        [HttpPost]
        public ActionResult DeleteProduct(int id)
        {
            // Логика для удаления товара из базы данных
            try
            {
                using (var db = new ProductContext())
                {
                    var productToDelete = db.Products.Find(id);
                    if (productToDelete != null)
                    {
                        db.Products.Remove(productToDelete);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("ProductsAdminPanel"); // Переадресация обратно к списку товаров
            }
            catch (Exception ex)
            {
                // Обработка ошибок, например, возврат страницы с ошибкой
                // или установка сообщения об ошибке в ViewBag и отображение того же представления
                ViewBag.Error = "Error deleting product: " + ex.Message;
                return View("Error"); // Предполагается, что у вас есть представление Error.cshtml
            }
        }


        public ActionResult AddProduct()
        {
            // Инициализация списка продуктов. Если нет продуктов для отображения,
            // передается пустой список, а не null.
      

            // Здесь может быть логика для заполнения списка продуктов,
            // если в вашем приложении это предусмотрено.

            return View(); // Передача списка в представление.
        }

        [HttpPost]

        public ActionResult AddProducts(Aroma.Domain.Entities.Product.DBModel.Product product)
        {
                     
                    var AdminAddProduct = new Models.Product
                    {
                        
                        Price = product.Price,
                       
                        Description = product.Description,
                        Name = product.Name,
                        Category = product.Category,
                        ProductType = product.ProductType,
                     


                       
                    };


            ResponseAddProduct response = _product.AdminAddAction(product);
                    {
                        if (response != null && response.Status)
                        {
                            return RedirectToAction("ProductsAdminPanel", "Product");

                        }
                        else
                            return RedirectToAction("AddProduct", "Home");
                      }

           

        }
        [HttpPost]
        public ActionResult UpdateProduct(Aroma.Domain.Entities.Product.DBModel.Product product)
        {
            try
            {
                // Подготовка объекта продукта для обновления

                var updateProduct = new Aroma.Domain.Entities.Product.DBModel.Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Category = product.Category,
                    ProductType = product.ProductType,
                    Description = product.Description
                };

                // Вызов метода бизнес-логики для обновления продукта
                ResponseToEditProduct response = _product.AdminUpdateAction(updateProduct);

                if (response.Status)
                {
                    // Если обновление прошло успешно, перенаправляем обратно на панель управления продуктами
                    return RedirectToAction("ProductsAdminPanel");
                }
                else
                {
                    // Если при обновлении произошла ошибка, отображаем сообщение об ошибке
                    ViewBag.ErrorMessage = response.MessageError;
                    return View("Error"); // Предполагается, что у вас есть представление Error.cshtml для отображения ошибок
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                ViewBag.ErrorMessage = "Ошибка обновления продукта: " + ex.Message;
                return View("Error");
            }
        }
        public ActionResult GetProducts()
        {
            // Вызов метода из бизнес-логики для получения всех продуктов
            ResponseGetProducts response = _product.AdminGetAction();

            if (response.Status)
            {
                // Если запрос прошёл успешно, отображаем список продуктов
                return View(response.Products);
            }
            else
            {
                // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                ViewBag.ErrorMessage = response.Message;
                return View("Error");
            }
        }
    }
   

}

