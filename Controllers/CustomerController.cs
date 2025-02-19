using JwtUser.Modal;
using JwtUser.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtUser.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpGet("GetByCode/{code}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCustomersByCode([FromRoute] string code)
        {
            var customer = await _customerService.GetByCodeAsync(code);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpDelete("Customer/{code}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RemoveCustomer([FromRoute] string code)
        {
            var response = await _customerService.RemoveCustomerAsync(code);
            return Ok(response);
        }

        [HttpPost("CreateCustomer")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerModal data)
        {
            var response = await _customerService.CreateCustomerAsync(data);
            return Ok(response);
        }

        [HttpPut("UpdateCustomer/{code}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerModal data, [FromRoute] string code)
        {
            var response = await _customerService.UpdateCustomerAsync(code,data);
            if(response.ResponseCode == 400)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
