using ByteartRetail.Application;
using ByteartRetail.Domain.Repositories.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace ByteartRetail.Services.SelfHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseAddress = ConfigurationManager.AppSettings["baseAddress"];
            ApplicationService.Initialize();
            MongoDBBootstrapper.Bootstrap();

            using (ServiceHost orderService = new ServiceHost(typeof(OrderService), new Uri(string.Format("{0}/OrderService.svc", baseAddress))))
            using (ServiceHost productService = new ServiceHost(typeof(ProductService), new Uri(string.Format("{0}/ProductService.svc", baseAddress))))
            using (ServiceHost userService = new ServiceHost(typeof(UserService), new Uri(string.Format("{0}/UserService.svc", baseAddress))))
            {
                orderService.Open();
                productService.Open();
                userService.Open();

                Console.WriteLine("Byteart Retail Services started at: {0}.", DateTime.Now);
                Console.ReadLine();
                Console.WriteLine("Byteart Retail Services stopped at: {0}.", DateTime.Now);

                orderService.Close();
                productService.Close();
                userService.Close();
            }
        }
    }
}
