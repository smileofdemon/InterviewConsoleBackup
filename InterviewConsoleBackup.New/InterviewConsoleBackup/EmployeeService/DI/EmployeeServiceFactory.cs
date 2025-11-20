using EmployeeService.Repositories;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace EmployeeService
{
    public class EmployeeServiceFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var repo = new EmployeeRepository(ConfigurationManager.ConnectionStrings["TestDatabase"].ConnectionString);
            var service = new EmployeeService(repo);
            return new CustomServiceHost(service, baseAddresses);
        }
    }
}