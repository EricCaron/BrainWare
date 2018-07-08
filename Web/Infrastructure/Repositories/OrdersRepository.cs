using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Web.Models;

namespace Web.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly Database _database;

        public OrdersRepository()
        {
            //For IoC. Use something like structureMap to initialize
            // Will also allow unit tests to mock 
            _database = new Database();
        }

        public IList<Order> GetOrders(int companyId)
        {
            //I dont like these queries but I cant see the table structure. 
            //CompanyId is never used. This should be used in the query as a filter if > 0

            var query =
                "SELECT c.name, o.description, o.order_id " +
                "FROM company c " +
                "INNER JOIN [order] o on c.company_id = o.company_id";

            IList<Order> results = new List<Order>();

            //this should really be performed by a framework such as EF or nHibernate
            using (DbDataReader resultReader = _database.ExecuteReader(query))
            {
                while (resultReader.Read())
                {
                    var dataRecord = (IDataRecord)resultReader;

                    results.Add(new Order
                    {
                        CompanyName = dataRecord.GetString(0),
                        Description = dataRecord.GetString(1),
                        OrderId = dataRecord.GetInt32(2)
                    });

                }
            }
            return results;
        }

        public IList<OrderProduct> GetOrderProducts()
        {
            //I feel this query can be combined with the query above, but alias I cannot view table structure
            var query =
                "SELECT op.price, op.order_id, op.product_id, op.quantity, p.name, p.price " +
                "FROM orderproduct op " +
                "INNER JOIN product p on op.product_id=p.product_id";

            IList<OrderProduct> results = new List<OrderProduct>();

            //this should really be performed by a framework such as EF or nHibernate
            using (DbDataReader resultReader = _database.ExecuteReader(query))
            {
                while (resultReader.Read())
                {
                    var dataRecord = (IDataRecord)resultReader;

                    results.Add(new OrderProduct
                    {
                        OrderId = dataRecord.GetInt32(1),
                        ProductId = dataRecord.GetInt32(2),
                        Price = dataRecord.GetDecimal(0),
                        Quantity = dataRecord.GetInt32(3),
                        Product = new Product
                        {
                            Name = dataRecord.GetString(4),
                            Price = dataRecord.GetDecimal(5)
                        }
                    });
                }
            }

            return results;
        }
    }
}