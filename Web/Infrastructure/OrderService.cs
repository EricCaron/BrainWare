using System.Collections.Generic;
using System.Data;
using System.Linq;
using Web.Infrastructure.Repositories;
using Web.Models;

namespace Web.Infrastructure
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderService()  //if IoC tool installed, object will be initialized  via argument to constructor
        {
            //For IoC. Use something like structureMap to initialize
            // Will also allow unit tests to mock 
            _ordersRepository = new OrdersRepository();
        }

        public IList<Order> GetOrdersForCompany(int companyId)
        {
            IList<Order> orderList;
            IList<OrderProduct> productList;

            try
            {
                orderList = _ordersRepository.GetOrders(companyId);
                productList = _ordersRepository.GetOrderProducts();
            }
            catch (System.Exception e)
            {
                //log error using tool like log4net
                string errMsg = "Exception while getting orders. companyId [{0}]";
                //Log.Error(string.Format(errMsg, companyId), e);

                throw new DataException("Internal service problem with fetch order data", e);
            }

            if (orderList == null)
            {
                //nothing found, return empty list instead of null
                return new List<Order>();
            }

            if (productList == null)
            {
                //in case null, set to empty list, so it will work without worry below
                productList = new List<OrderProduct>();
            }

            foreach (var order in orderList)
            {
                order.OrderProducts = productList.Where(p => p.OrderId == order.OrderId).ToList();
                if (order.OrderProducts == null)
                {   //better to work with empty lists than trickster nulls
                    order.OrderProducts = new List<OrderProduct>();
                }

                order.OrderTotal = order.OrderProducts.Sum(p => p.Price * p.Quantity);
            }

            return orderList;
        }
    }
}