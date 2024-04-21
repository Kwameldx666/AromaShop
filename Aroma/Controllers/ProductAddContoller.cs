using Aroma.BussinesLogic.Interface;
using Aroma.BussinesLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.GeneralResponse;
using Lab_TW.Models;

namespace Lab_TW.Controllers
{
    public class ProductAddContoller : BaseController
    {
        private IOrderService _orderService;

        public ProductAddContoller()
        {
            var logicBL = new BussinesLogic();
            _orderService = logicBL.OrderServBL();

        }

        [HttpPost]

        public ActionResult AddProductToCart(int productId)
        {
            try
            {
                // Получаем идентификатор пользователя из сессии
                int userId = (int)Session["UserId"];
                int quantity = 1;
                // Логика добавления товара в корзину
                _orderService.PurchaseProduct(userId, productId, quantity);

                // Переадресация обратно к списку товаров или на другую страницу
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                // Обработка ошибки, если не удалось добавить товар в корзину
                ViewBag.ErrorMessage = "Произошла ошибка при добавлении товара в корзину.";
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Error", "Home");
            }
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
    }
}