using EmployeeService.Models;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace EmployeeService
{
    [ServiceContract]
    public interface IEmployeeService
    {

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "employees/{id}",
            ResponseFormat = WebMessageFormat.Json,  BodyStyle = WebMessageBodyStyle.Bare)]
        EmployeeDto GetEmployeeById(string id);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "employees/{id}",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        EmployeeEnableDto EnableEmployee(string id, EmployeeEnableDto dto);
    }
}
