using JwtUser.Helper;
using JwtUser.Modal;

namespace JwtUser.Service
{
    public interface ICustomerService
    {
        Task<List<CustomerModal>> GetAllCustomersAsync();
        Task<APIResponse<CustomerModal>> GetByCodeAsync(string code);
        Task<APIResponse<CustomerModal>> RemoveCustomerAsync(string code);
        Task<APIResponse<CustomerModal>> CreateCustomerAsync(CustomerModal data);
        Task<APIResponse<CustomerModal>> UpdateCustomerAsync(string code, CustomerModal data);
    }
}
