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
        ResponseAddProduct AdminAddAction(Product product);
        ResponseToEditProduct AdminUpdateAction(Product updateProduct);

        ResponseToDeleteProduct  AdminDeleteAction(Product productDelete);
        ResponseFilterProducts GetFilteredProducts(string category, string brand, decimal lowerPrice, decimal upperPrice, string sorting);
    }
}
