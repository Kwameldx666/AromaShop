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
using System.Web.Helpers;
using Tensorflow;
using Aroma.BussinesLogic.mainBL;

namespace Lab_TW.Controllers
{
    public class ProductController : BaseController
    {


        internal IProduct _product;

        [AdminAndModerator]
        public ActionResult ProductsAdminPanel()
        {
            StatusSessionCheck();

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
                Quantity = p.Quantity,
                Discount = p.Discount,
                PriceWithDiscount = p.PriceWithDiscount,
                
                
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
        public  JsonResult  FilterProducts(string category, string brand, decimal lowerPrice, decimal upperPrice, string sorting)
        {
            // Получаем все продукты из базы данных или другого источника данных
            ResponseFilterProducts response = _product.GetFilteredProducts(category, brand,lowerPrice,upperPrice,sorting);

            var viewModelProducts = response.FilteredProducts.Select(p => new Lab_TW.Models.Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                ProductType = p.ProductType,
                Description = p.Description,
                ImageUrl = p.ImageUrl,

            }).ToList();
            if (!response.Success)
            {
                // Если произошла ошибка при получении продуктов
                ViewBag.ErrorMessage = response.ErrorMessage;
                return Json(new { success = false, errorMessage = response.ErrorMessage });
            }



            // Возвращаем отфильтрованные продукты в виде JSON
            return Json(new { success = true, filteredProducts = response.FilteredProducts });
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
            StatusSessionCheck();

            return View(); // Передача списка в представление.
        }
        [AdminAndModerator]
        [HttpPost]

        public ActionResult AddProducts(Models.Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Проверка на наличие и тип файла
                if (ImageFile != null && ImageFile.ContentLength > 0 && ImageFile.ContentType.StartsWith("image"))
                {
                    var uploadPath = Server.MapPath("~/img/product");
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(uploadPath, fileName);
                    ImageFile.SaveAs(filePath);

                    product.ImageUrl = Url.Content(Path.Combine("~/img/product", fileName));
                }
                else
                {
                    ModelState.AddModelError("", "Please upload a valid image file.");
                    return View("~/Views/Home/AddProduct.cshtml", product);
                }

                Mapper.Initialize(cfg => cfg.CreateMap<Models.Product, Aroma.Domain.Entities.Product.DBModel.Product>());
                var AdminAddProduct = Mapper.Map<Aroma.Domain.Entities.Product.DBModel.Product>(product);

                ResponseAddProduct response = _product.AdminAddAction(AdminAddProduct);
                if (response != null && response.Status)
                {
                    return RedirectToAction("ProductsAdminPanel", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to add the product. Please try again.");
                    return View("~/Views/Home/AddProduct.cshtml", product);
                }
            }

            return View("~/Views/Home/AddProduct.cshtml", product);
        }

        private IOrderService _orderService;

        public ActionResult AddProductToCart()
        {StatusSessionCheck();
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

        [HttpPost]
        public ActionResult SingleProduct(int? productId, int rating, string textarea)
        {

            Console.WriteLine($"Received POST request for productId: {productId}, rating: {rating}, review: {textarea}");
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
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
            {
                // Вызов метода из бизнес-логики для получения всех продуктов
                ResponseGetRating response = _orderService.ViewOrdersAction(userId, productId.Value, rating, textarea);

               
                    Session["RATING"] = response.Good;
                
                var viewModelOrders = response.View.Select(p => new Lab_TW.Models.RatingData
                {
                   Product = p.Product,
                   Rating = p.Rating,
                   User = p.User,

                }).ToList();
                var productik = new Models.SingleProduct
                {
                    View = viewModelOrders
                };
                if (response.Status)
                {
                    // Если запрос прошёл успешно, отображаем список продуктов
                    return RedirectToAction("SingleProduct");
                }
                else
                {
                    // Если при запросе возникла ошибка, отображаем сообщение об ошибке
                    ViewBag.ErrorMessage = response.Message;
                    return View("Error");
                }
            }
        }
        [HttpGet]
        public ActionResult SingleProduct(int? productId)
        {
            StatusSessionCheck();
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
                ResponseGetRating response1 = null; // Объявляем здесь, чтобы использовать вне блока условия
                if (response.IsPurchased)
                {
                    response1 = _orderService.ViewOrdersAction(userId, productId.Value, 0, null);
                  
                }
                Session["RATING"] = response.IsPurchased;
                var product = new Models.Product
                {
                    Price = response.PriceWidthDiscount,
                    Category = response.Category,
                    Description = response.Description,
                    Name = response.Name,
                    ImageUrl = response.ImageUrl,
                    Quantity = response.Quantity,
                    Id = response.ProductId,
                    View = response.View,
                    AverageReting = response.AverageRating
                };

                // Инициализация viewModelOrders вне условия для доступа к ней позже
                List<Lab_TW.Models.RatingData> viewModelOrders = new List<Lab_TW.Models.RatingData>();

                if (response1 != null && response1.Status)
                {
                    viewModelOrders = response1.View.Select(p => new Lab_TW.Models.RatingData
                    {
                        Product = p.Product,
                        Rating = p.Rating,
                        User = p.User,
                        Feedback = p.Feedback
                    }).ToList();
                }

                var productik = new Models.SingleProduct
                {
                    Products = product,
                    View = viewModelOrders
                };

                if (response.Status)
                {
                    // Возвращаем представление с данными о продукте
                    return View(productik);
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
            StatusSessionCheck();
            try
            {

                int UserId = (int)Convert.ToUInt32(Session["UserId"]);
                if (UserId == 0)
                {
                    GetUserId();
                  /*  if (UserId == -1)
                    {
                        return RedirectToAction("Login", "Account");
                    }*/
                    UserId = (int)Convert.ToUInt32(Session["UserId"]);
                }

                // Логика добавления товара в корзину
                ResponseAddOrder resp = _orderService.PurchaseProduct(UserId, productId, quantity);
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
        public async Task<ActionResult> ConfirmPurchase()
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
                return RedirectToAction("Cart", "Product");
            }
            else
            {
                return Json(new { status = false });
            }



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
            StatusSessionCheck();
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
                ProductType = p.ProductType,
                ImageUrl = p.ImageUrl

            }).ToList();

            return Json(new { status = response.Status, message = response.Message });
        }
        [HttpPost]
        public ActionResult Cart(int productId,int rating,string review)
        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
            {
                // Вызов метода из бизнес-логики для получения всех продуктов
                ResponseGetOrders response = _orderService.ViewOrdersAction(userId);
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
                    ProductType = p.ProductType,
                    Feedback = p.Feedback,
                    Rating = p.Reting,
                    AverageRating = p.AverageRating

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
        public ActionResult Cart()
        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
            {
                // Вызов метода из бизнес-логики для получения всех продуктов
                ResponseGetOrders response = _orderService.ViewOrdersAction(userId);
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
                    ProductType = p.ProductType,
                    Feedback = p.Feedback,
                    Rating = p.Reting,
                    AverageRating = p.AverageRating,
                    Price = p.Product.Price
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
        public ActionResult AddReview()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddReview(int productId)
        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }

            ResponseGetOrders response = _orderService.AddFeedback(productId, userId);

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
            return View(product);
        }
        [AdminAndModerator]
        [HttpPost]
        public ActionResult UpdateProduct(Models.Product product)
        {


            Mapper.Initialize(cfg => cfg.CreateMap<Models.Product, Aroma.Domain.Entities.Product.DBModel.ProductDbTable>());

            var updateProduct = Mapper.Map<Aroma.Domain.Entities.Product.DBModel.ProductDbTable>(product); // Исправлено здесь

         
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

