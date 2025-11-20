using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace EmployeeService
{
    public class InstanceProvider : IInstanceProvider
    {
        private readonly object _instance;

        public InstanceProvider(object instance)
        {
            _instance = instance;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return _instance;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return _instance;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}