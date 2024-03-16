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
        public ProductDataModel ActionAddMAnyProducts()

        {
            var products = new List<Product>();
            return new ProductDataModel { AddProducts = products }; ;
        }

        public ProductDataModel ActionAddSingleProduct()
        {
            var productik = new Product();
            return new ProductDataModel { AddSingleProduct = productik };
        }

        public RResponseData AddAdminActionProduct(Product product)
        {
            if (product == null) { return new RResponseData { Status = false }; }
            return new RResponseData { Status = true };
        }
    }
}
