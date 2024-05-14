using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aroma.BussinesLogic.mainBL
{
    public class ProductBL : AdminAPI, IProduct
    {
        public ResponseGetProducts AdminGetAction()
        {
            return GetAllProductsAdmin();
        }

        public ResponseGetProducts GetAction()
        {
            return GetAllProducts();
        }

        public ResponseAddProduct AdminAddAction(Product products )
        {
            return AddAdminActionProduct(products);
        }
        public ResponseToEditProduct AdminUpdateAction(ProductDbTable updateProduct)
        {
            return EditAdminActionProduct(updateProduct);
        }

        public ResponseToDeleteProduct AdminDeleteAction(Product productDelete)
        {
            return  DeleteProductAction(productDelete);
        }


        public async Task<ResponseFilterProducts> GetFilteredProducts(string category, string brand, decimal lowerPrice, decimal upperPrice, string sorting)
        {
            return await GetFilteredProductsAction(category, brand , lowerPrice,upperPrice, sorting);
        }
    }
}
