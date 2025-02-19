using AutoMapper;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos;
using JwtUser.Repos.Models;
using JwtUser.Service;
using Microsoft.EntityFrameworkCore;

namespace JwtUser.Container
{
    public class CustomerService : ICustomerService
    {
        private readonly LearnDataContext _context;

        private readonly IMapper _mapper;

        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger, IMapper mapper, LearnDataContext context)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<APIResponse<CustomerModal>> CreateCustomerAsync(CustomerModal data)
        {
            APIResponse<CustomerModal> response = new APIResponse<CustomerModal>();

            try
            {
                var customer = _mapper.Map<CustomerModal, TblCustomer>(data);
                //this._logger.LogInformation("Creation Started");
                var customerCheck = await GetByCodeAsync(customer.Code);

                if (customerCheck.Result != null)
                {
                    return new APIResponse<CustomerModal>()
                    {
                        Message = "Customer already available with the given code!",
                        ResponseCode = 402,
                        Result = new CustomerModal()
                    };
                }
                await _context.TblCustomers.AddAsync(customer);
                await _context.SaveChangesAsync();

                return new APIResponse<CustomerModal>()
                {
                    Message = "Success",
                    ResponseCode = 200,
                    Result = data
                };
            }
            catch (Exception ex)
            {
                //this._logger.LogError(ex.Message,ex);
                response.ResponseCode = 400;
                response.Message = ex.Message;
                return response;
            }
        }

        public async Task<List<CustomerModal>> GetAllCustomersAsync()
        {
            List<CustomerModal> response = new List<CustomerModal>();
            var customer = await _context.TblCustomers.ToListAsync();
            if (customer != null)
            {
                response = _mapper.Map<List<TblCustomer>, List<CustomerModal>>(customer);
            }
            return response;
        }

        public async Task<APIResponse<CustomerModal>> GetByCodeAsync(string code)
        {
            var customer = await _context.TblCustomers.SingleOrDefaultAsync(x => x.Code == code);
            if (customer == null)
            {
                return new APIResponse<CustomerModal>
                {
                    ResponseCode = 404,
                    Message = "Customer not found",
                    Result = null
                };
            }

            return new APIResponse<CustomerModal>
            {
                ResponseCode = 200,
                Message = "Success",
                Result = _mapper.Map<CustomerModal>(customer)
            };
        }

        public async Task<APIResponse<CustomerModal>> RemoveCustomerAsync(string code)
        {
            CustomerModal response = new CustomerModal();
            var customer = await _context.TblCustomers.FirstOrDefaultAsync(x => x.Code == code);
            if (customer != null)
            {
                _context.TblCustomers.Remove(customer);
                await _context.SaveChangesAsync();

                response = _mapper.Map<TblCustomer, CustomerModal>(customer);

                return new APIResponse<CustomerModal>()
                {
                    ResponseCode = 400,
                    Message = "Customer removed successfully",
                    Result = response,
                };
            }
            return new APIResponse<CustomerModal>
            {
                ResponseCode = 404,
                Message = "Customer not found",
                Result = null
            };
        }

        public async Task<APIResponse<CustomerModal>> UpdateCustomerAsync(string code, CustomerModal request)
        {
            APIResponse<CustomerModal> response = new APIResponse<CustomerModal>();

            var result = await _context.TblCustomers.FirstOrDefaultAsync(x => x.Code == code);

            if (result == null)
            {
                return new APIResponse<CustomerModal>()
                {
                    Message = "Not Found",
                    ResponseCode = 400,
                    Result = null
                };
            }

            result.Code = request.Code;
            result.Name = request.Name;
            result.Phone = request.Phone;
            result.Email = request.Email;
            result.Creditlimit = request.CreditLimit;
            result.IsActive = request.IsActive;

            _context.TblCustomers.Update(result);
            await _context.SaveChangesAsync();

            return new APIResponse<CustomerModal>
            {
                Result = request,
                Message = "Update Successfully",
                ResponseCode = 200,
            };
        }
    }
}