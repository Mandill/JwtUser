using System.Data;
using ClosedXML.Excel;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtUser.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CustomerController : ControllerBase
    {
        private ICustomerService _customerService;
        private readonly IWebHostEnvironment _webhostenvironment;

        public CustomerController(IWebHostEnvironment webhostenvironment, ICustomerService customerService)
        {
            _webhostenvironment = webhostenvironment;
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

        [HttpDelete("DeleteCustomer/{code}")]
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
            var response = await _customerService.UpdateCustomerAsync(code, data);
            if (response.ResponseCode == 400)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel()
        {
            APIResponse<string> apiResponse = new APIResponse<string>();
            try
            {
                var directorypath = GetPath();
                var excelfilepath = directorypath + "\\Customers.xlsx";
                var customers = await _customerService.GetAllCustomersAsync();
                if (customers.Count > 0 && customers != null)
                {
                    DataTable dt = new DataTable("Customers");
                    dt.Columns.Add("Code", typeof(string));
                    dt.Columns.Add("Name", typeof(string));
                    dt.Columns.Add("Email", typeof(string));
                    dt.Columns.Add("CreditLimit", typeof(string));
                    dt.Columns.Add("IsActive", typeof(string));
                    dt.Columns.Add("StatusName", typeof(string));
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "Customers");
                        customers.ForEach(x =>
                        {
                            dt.Rows.Add(x.Code, x.Name, x.Email, x.CreditLimit, x.IsActive, x.StatusName);
                        });
                        using (MemoryStream stream = new MemoryStream())
                        {
                            if (System.IO.File.Exists(excelfilepath))
                            {
                                System.IO.File.Delete(excelfilepath);
                            }
                            wb.SaveAs(excelfilepath);
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
                        }
                    }
                }
                else
                {
                    apiResponse.ResponseCode = 400;
                    apiResponse.Message = "No data found";
                    return NotFound(apiResponse);
                }
            }
            catch (Exception ex)
            {
                apiResponse.ResponseCode = 400;
                apiResponse.Message = ex.Message;
                return NotFound(apiResponse);
            }
        }

        [NonAction]
        public string GetPath()
        {
            string filePath = _webhostenvironment.WebRootPath + "\\export";
            return filePath;
        }
    }
}