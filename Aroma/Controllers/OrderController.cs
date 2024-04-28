using Aroma.BussinesLogic.Interface;
using Aroma.BussinesLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aroma.Domain.Entities.GeneralResponse;
using System.Threading.Tasks;

namespace Lab_TW.Controllers
{
    public class OrderController : BaseController
    {

        private IOrderService _orderService;

        public OrderController()
        {
            var logicBL = new BussinesLogic();
            _orderService = logicBL.OrderServBL();

        }
        // GET: Order
        public JsonResult UpdateQuantity(int productId, int quantityOrder)

        {
            int userId = (int)Convert.ToUInt32(Session["UserId"]);
            if (userId == 0)
            {
                GetUserId();
                userId = (int)Convert.ToUInt32(Session["UserId"]);
            }
            ResponseUpdateQuantityOrders response = _orderService.EditQuntity(userId,productId, quantityOrder);
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
        [HttpPost]
       
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
                return Json(new { status = true });
            }
            else
                return Json(new { status = false });



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
    }
}