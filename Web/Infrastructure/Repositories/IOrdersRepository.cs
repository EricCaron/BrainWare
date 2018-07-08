using System.Collections.Generic;
using Web.Models;

namespace Web.Infrastructure.Repositories
{
    public interface IOrdersRepository
    {
        IList<Order> GetOrders(int companyId);
        IList<OrderProduct> GetOrderProducts();
    }
}