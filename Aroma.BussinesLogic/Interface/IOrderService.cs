using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Interface
{
    public interface IOrderService
    {
        ResponseFilterProducts GetFilteredProducts(string category, string brand, string color);
        Task<ResponseGetOrders> ConfirmPurchaseUserAction(int userId);
        Task<ResponseGetOrders> DeleteOrderAction(int userId, int productId);
        ResponseUpdateQuantityOrders EditQuntity(int userId, int productId, int quantityOrder);
        ResponseAddOrder PurchaseProduct(int userId, int productId, int quantity);
        Task<ResponseGetProducts> SearchProducts(string searchTerm);
        ResponseGetOrders ViewOrdersAction();
        ResponseGetOrders ViewOrdersUserAction(int userId);
        ResponseViewProduct ViewProductInfo(int userId, int productId);

        // Другие методы для работы с заказами


    }
}
