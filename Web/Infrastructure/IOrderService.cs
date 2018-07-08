using System.Collections.Generic;
using Web.Models;

namespace Web.Infrastructure
{
    public interface IOrderService
    {
        IList<Order> GetOrdersForCompany(int companyId);
    }
}