using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace EmployeeService
{
    public class InstanceProviderBehavior : IContractBehavior
    {
        private readonly object _instance;

        public InstanceProviderBehavior(object instance)
        {
            _instance = instance;
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime) { }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = new InstanceProvider(_instance);
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint) { }
    }
}