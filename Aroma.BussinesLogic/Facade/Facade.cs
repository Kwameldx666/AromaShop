using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using Aroma.Domain.Enums.OrdersStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AromaShop.Facade
{
    // Подсистемы магазина
    public class ProductManager
    {
        public List<ProductDbTable> GetProducts()
        {
            // Здесь будет логика получения продуктов из БД
            return new List<ProductDbTable>
            {
                new ProductDbTable { Id = 1, Name = "Lavender Essence", Price = 29.99m },
                new ProductDbTable { Id = 2, Name = "Rose Bliss", Price = 34.99m }
            };
        }

        public ProductDbTable GetProduct(int id)
        {
            return GetProducts().FirstOrDefault(p => p.Id == id);
        }
    }

    public class CartManager
    {
        private List<Order> CartItems { get; set; } = new List<Order>();

        public void AddToCart(ProductDbTable product, int quantity, UDbTable user)
        {
            var cartItem = new Order
            {
                ProductId = product.Id,
                Product = product,
                QuantityOrder = quantity,
                TotalPrice = product.Price * quantity,
                UserId = user.Id,
                UDbTable = user,
                OrderDate = DateTime.Now,
                orderStatus = OrderStatus.Successful
            };
            CartItems.Add(cartItem);
        }

        public decimal GetTotal()
        {
            return CartItems.Sum(item => item.TotalPrice);
        }

        public List<Order> GetItems()
        {
            return CartItems;
        }

        public void ClearCart()
        {
            CartItems.Clear();
        }
    }

    public class OrderManager
    {
        public List<Order> CreateOrder(CartManager cart, UDbTable user)
        {
            var orders = cart.GetItems();
            foreach (var order in orders)
            {
                order.orderStatus = OrderStatus.Pending;
                order.OrderDate = DateTime.Now;
            }
            return orders; // Здесь можно добавить сохранение в БД
        }
    }

    public class UserManager
    {
        public UDbTable Login(string email, string password)
        {
            // Здесь будет реальная логика авторизации
            return new UDbTable
            {
                Id = 1,
                Email = email,
                Password = password,
                Username = "User_" + email.Split('@')[0],
                Balance = 100.00m,
                Level = UserRole.User
            };
        }

        public UDbTable GetUserInfo(int userId)
        {
            // Здесь будет получение данных пользователя из БД
            return new UDbTable
            {
                Id = userId,
                Username = "User_" + userId,
                Email = $"user{userId}@example.com"
            };
        }
    }

    // Фасад магазина
    public class ShopFacade
    {
        private readonly ProductManager _productManager;
        private readonly CartManager _cartManager;
        private readonly OrderManager _orderManager;
        private readonly UserManager _userManager;
        private UDbTable _currentUser;

        public ShopFacade()
        {
            _productManager = new ProductManager();
            _cartManager = new CartManager();
            _orderManager = new OrderManager();
            _userManager = new UserManager();
        }

        public bool Login(string email, string password)
        {
            try
            {
                _currentUser = _userManager.Login(email, password);
                return _currentUser != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ProductDbTable> BrowseProducts()
        {
            return _productManager.GetProducts();
        }

        public bool AddProductToCart(int productId, int quantity = 1)
        {
            if (_currentUser == null) return false;

            var product = _productManager.GetProduct(productId);
            if (product != null)
            {
                _cartManager.AddToCart(product, quantity, _currentUser);
                return true;
            }
            return false;
        }

        public List<Order> Checkout()
        {
            if (_currentUser == null || !_cartManager.GetItems().Any())
                return null;

            var orders = _orderManager.CreateOrder(_cartManager, _currentUser);
            if (orders != null && orders.Any())
            {
                if (_currentUser.Balance >= _cartManager.GetTotal())
                {
                    _currentUser.Balance -= _cartManager.GetTotal();
                    _cartManager.ClearCart();
                    return orders;
                }
            }
            return null;
        }

        public decimal GetCartTotal()
        {
            return _cartManager.GetTotal();
        }

        public List<Order> GetCartItems()
        {
            return _cartManager.GetItems();
        }

        public UDbTable GetCurrentUser()
        {
            return _currentUser;
        }
    }
}