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
using Lab_TW.Atributes;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI.WebControls;
using System.IO;
using Aroma.Domain.Entities.User;
using AutoMapper;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Threading.Tasks;
using Lab_TW.Extension;

namespace Lab_TW.Controllers
{
    public class ProductController : BaseController
    {


        internal IProduct _product;

        [AdminAndModerator]
        public ActionResult ProductsAdminPanel()
        {


            // Вызов метода из бизнес-логики для получения всех продуктов;
            ResponseGetProducts response = _product.AdminGetAction();
            var viewModelProducts = response.Products.Select(p => new Lab_TW.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ProductType = p.ProductType,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity
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

        }
        [HttpPost]
        public ActionResult FilterProducts(string category, string brand, string color)
        {
            // Получаем все продукты из базы данных или другого источника данных
            ResponseGetProducts response = _product.AdminGetAction();

            if (!response.Status)
            {
                // Если произошла ошибка при получении продуктов
                ViewBag.ErrorMessage = response.Message;
                return View("Error");
            }

            // Фильтруем продукты по выбранным критериям
            var filteredProducts = response.Products.Where(p =>
                (category == null || p.Category == category) &&
                (brand == null || p.ProductType == brand) 

            ).ToList();

            // Возвращаем отфильтрованные продукты в виде JSON
            return Json(new { success = true, products = filteredProducts });
        }
    
    public ProductController()
        {
            var bl = new BussinesLogic();
            _product = bl.AddProductBL();
            _orderService = bl.OrderServBL();

        }
        [AdminMode]
        [HttpPost]
        public ActionResult DeleteProduct(Aroma.Domain.Entities.Product.DBModel.Product product)
        {
            var AdminIdProductToEdit = new Models.Product
            {
                Id = product.Id
            };

            ResponseToDeleteProduct responseToDelete =  _product.AdminDeleteAction(product);


            if (responseToDelete != null && responseToDelete.Status == true)
            {

                TempData["SuccessMessage"] = "Product successfully deleted";
                return RedirectToAction("ProductsAdminPanel");
            }

            else
            {
                // Обработка ошибок, например, возврат страницы с ошибкой
                // или установка сообщения об ошибке в ViewBag и отображение того же представления
                ViewBag.Error = "Error deleting product ";
                return View("ProductsAdminPanel");
            }
        }

        [AdminAndModerator]
        public ActionResult AddProduct()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Account");
            }

            return View(); // Передача списка в представление.
        }
        [AdminAndModerator]
        [HttpPost]

        public ActionResult AddProducts(Models.Product product, HttpPostedFileBase ImageFile)
        {
            // Здесь логика для сохранения загруженного файла и генерации URL
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                var uploadPath = Server.MapPath("~/img/product"); // Путь к директории для сохранения файла
                var fileName = Path.GetFileName(ImageFile.FileName); // Получение имени файла
                var filePath = Path.Combine(uploadPath, fileName); // Создание полного пути к файлу

                // Сохранение файла на сервер
                ImageFile.SaveAs(filePath);

                // Присвоение URL к свойству ImageUrl
                product.ImageUrl = Url.Content(Path.Combine("~/img/product", fileName));
            }


            Mapper.Initialize(cfg => cfg.CreateMap<Models.Product, Aroma.Domain.Entities.Product.DBModel.Product>());
            var AdminAddProduct = Mapper.Map<Aroma.Domain.Entities.Product.DBModel.Product>(product); // Исправлено здесь




            // Вызов метода бизнес-логики для добавления продукта
            ResponseAddProduct response = _product.AdminAddAction(AdminAddProduct);
            if (response != null && response.Status)
            {
                // Если продукт успешно добавлен, перенаправляем на страницу управления продуктами
                return RedirectToAction("ProductsAdminPanel", "Product");
            }
            else
            {
                // В случае ошибки возвращаемся на страницу добавления продукта
                return RedirectToAction("AddProduct", "Home");
            }



        }
        private IOrderService _orderService;

        public ActionResult AddProductToCart()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<ActionResult> SearchProducts(string searchTerm)
        {
            try
            {
                var result = await _orderService.SearchProducts(searchTerm);

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult SingleProduct(int? productId)
        {
            // Если productId не был передан как параметр, попробуйте получить его из маршрута
            if (productId == null)
            {
                // Получаем productId из маршрута
                string idFromRoute = RouteData.Values["id"] as string;

                if (string.IsNullOrEmpty(idFromRoute) || !int.TryParse(idFromRoute, out int parsedId))
                {
                    // Если idFromRoute пустой или не удается преобразовать его в int, возвращаем ошибку 404
                    return HttpNotFound();
                }

                productId = parsedId; // Присваиваем значение из маршрута productId
            }

            try
            {
                // Логика обработки запроса для конкретного productId

                // Получаем идентификатор пользователя из сессии
                int userId = (int)Convert.ToUInt32(Session["UserId"]);
                if (userId == 0)
                {
                    GetUserId();
                    userId = (int)Convert.ToUInt32(Session["UserId"]);
                }

                // Логика добавления товара в корзину
                ResponseViewProduct response = _orderService.ViewProductInfo(userId, productId.Value);

                Models.Product product = new Models.Product
                {
                    Price = response.Price,
                    Category = response.Category,
                    Description = response.Description,
                    Name = response.Name,
                    ImageUrl = response.ImageUrl,
                    Quantity = response.Quantity,
                    Id = response.ProductId,
                };

                if (response.Status != true)
                {
                    // Возвращаем представление с данными о продукте
                    return View(product);
                }
                else
                {
                    // Перенаправляем на страницу ошибки, если что-то пошло не так
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибки, если не удалось добавить товар в корзину
                ViewBag.ErrorMessage = "Произошла ошибка при добавлении товара в корзину.";
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


 
                [HttpPost]
        public JsonResult AddProductToCart(int productId ,int quantity)
        {
            try
            {
                // Получаем идентификатор пользователя из сессии
                int userId = (int)Convert.ToUInt32(Session["UserId"]);
                if (userId == 0)
                {
                    GetUserId();
                    userId = (int)Convert.ToUInt32(Session["UserId"]);
                }

                // Логика добавления товара в корзину
                ResponseAddOrder resp = _orderService.PurchaseProduct(userId, productId, quantity);
                if (resp.Success)
                    // Переадресация обратно к списку товаров или на другую страницу
                    return Json(new { success = true });
                else
                    return Json(new { success = false });

            }
            catch (Exception ex)
            {
                // Обработка ошибки, если не удалось добавить товар в корзину
                ViewBag.ErrorMessage = "Произошла ошибка при добавлении товара в корзину.";
                ViewBag.ErrorMessage = ex.Message;
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<JsonResult> DeleteOrder(int productId)
        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
            ResponseGetOrders response = await _orderService.DeleteOrderAction(userId, productId);
            if (response.Status)
            {
                return Json(new { success = true });
            }
            else
                return Json(new { status = false }); // Ошибка при удалении
        }
        public async Task<JsonResult> ConfirmPurchase()
        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }

            // Вызов метода из бизнес-логики для получения всех продуктов
            ResponseGetOrders response = await _orderService.ConfirmPurchaseUserAction(userId);
            var viewModelOrders = response.Orders.Select(p => new Lab_TW.Models.OrderPr
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

            }).ToList();
                if (response.Status)
                {
                    return Json(new { success = true });
                }
                else
                    return Json(new { status = false });

        

        }
        public ActionResult OrderConfirm()
        {

            
                int userId = (int)Convert.ToUInt32(Session["UserId"]);
                if (userId == 0)
                {
                    GetUserId();
                    userId = (int)Convert.ToUInt32(Session["UserId"]);
                }
                
                // Вызов метода из бизнес-логики для получения всех продуктов
                ResponseGetOrders response = _orderService.ViewOrdersUserAction(userId);
                var viewModelOrders = response.Orders.Select(p => new Lab_TW.Models.OrderPr
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

                }).ToList();
                if (response.Status)
                {
                    // Если запрос прошёл успешно, отображаем список продуктов
                    return View(viewModelOrders);
                }
                else
                {
                    // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                    ViewBag.ErrorMessage = response.Message;
                    return View("Error");
                }



        }
    
        public ActionResult Category()
        {
            SessionStatus();
            if ((string)System.Web.HttpContext.Current.Session["LoginStatus"] != "login")
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.IsUserLoggedIn = true;
            var user = System.Web.HttpContext.Current.GetMySessionObject();

            ResponseGetProducts response = _product.AdminGetAction();

            var viewModelProducts = response.Products.Select(p => new Lab_TW.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ProductType = p.ProductType,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                Quantity = p.Quantity,
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

        }

        public JsonResult UpdateQuantity(int productId, int quantityOrder)

        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
            ResponseUpdateQuantityOrders response = _orderService.EditQuntity(userId, productId, quantityOrder);
            var viewModelOrders = response.Orders.Select(p => new Lab_TW.Models.OrderPr
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

            }).ToList();

            return Json(new { status = response.Status, message = response.Message });
        }
        public ActionResult Cart()
        {

            {
                // Вызов метода из бизнес-логики для получения всех продуктов
                ResponseGetOrders response = _orderService.ViewOrdersAction();
                var viewModelOrders = response.Orders.Select(p => new Lab_TW.Models.OrderPr
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
                }).ToList();
                if (response.Status)
                {
                    // Если запрос прошёл успешно, отображаем список продуктов
                    return View(viewModelOrders);
                }
                else
                {
                    // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                    ViewBag.ErrorMessage = response.Message;
                    return View("Error");
                }

            }


        }
        [AdminAndModerator]
        [HttpPost]
        public ActionResult UpdateProduct(Models.Product product)
        {


            Mapper.Initialize(cfg => cfg.CreateMap<Models.Product, Aroma.Domain.Entities.Product.DBModel.Product>());

            var updateProduct = Mapper.Map<Aroma.Domain.Entities.Product.DBModel.Product>(product); // Исправлено здесь

         
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
        [AdminAndModerator]
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

