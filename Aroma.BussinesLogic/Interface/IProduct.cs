using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aroma.BussinesLogic.Interface
{
    public interface IProduct
    {

        ResponseGetProducts AdminGetAction();
        ResponseGetProducts GetAction();
        ResponseAddProduct AdminAddAction(Product product);
        ResponseToEditProduct AdminUpdateAction(ProductDbTable updateProduct);

        ResponseToDeleteProduct  AdminDeleteAction(Product productDelete);
        Task<ResponseFilterProducts> GetFilteredProducts(string category, string brand, decimal lowerPrice, decimal upperPrice, string sorting);
    }
}
