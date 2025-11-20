using System;
using System.ServiceModel;

namespace EmployeeService
{
    public class CustomServiceHost : ServiceHost
    {
        public CustomServiceHost(object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance.GetType(), baseAddresses)
        {
            foreach (var cd in this.ImplementedContracts.Values)
            {
                cd.Behaviors.Add(new InstanceProviderBehavior(singletonInstance));
            }
        }
    }
}