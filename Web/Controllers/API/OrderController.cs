using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers.API
{
    [RoutePrefix("api/Orders")]
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrderController() //if IoC tool installed, object will be initialized  via argument to constructor
        {
            //For IoC. Use something like structureMap to initialize
            // Will also allow unit tests to mock 
            _orderService = new OrderService();
        }

        [HttpGet]
        [Route("{companyId}")]
        public IHttpActionResult GetOrders(int companyId = 1)
        {
            IList<Order> orders;
            try
            {
                orders = _orderService.GetOrdersForCompany(companyId);
            }
            catch (DataException)
            {
                //handled exception already logged
                return InternalServerError();
            }
            catch (Exception e)
            {
                //something really bad happened

                //log it using tool like log4net
                string errMsg = "Unhandled exception in request [/api/Orders/{0}] ";
                //Log.Error(string.Format(errMsg, companyId), e);

                return InternalServerError();
            }

            return Ok(orders);
        }
    }
}
