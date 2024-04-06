using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseGetProducts
    {
        public bool Status { get; set; } // Успех операции
        public string Message { get; set; } // Сообщение, например, об ошибке

        public List<Product.DBModel.Product> Products;

        public ResponseGetProducts()
        {
            Products = new List<Product.DBModel.Product>(); // Инициализация списка для избежания NullReferenceException
        }

    
    }

}
