using SeaEco.Services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Customer;
using SeaEco.Server.Infrastructure;

namespace SeaEco.Server.Controllers;


[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class CustomerController: ControllerBase
{
   private readonly ICustomerService _customerService;

   public CustomerController(ICustomerService customerService)
   {
      _customerService = customerService;
   }
   
   [HttpGet("Customer-names")]
   public async Task<IActionResult> GetCustomerNames()
   {
      var customerNames = await _customerService.GetCustomerNames();

      if (!customerNames.Any())
      {
         return NotFound("No customers found");
      }
      
      return Ok(customerNames);
   }
   
   [HttpGet("{id:int}")]
   public async Task<IActionResult> GetCustomerInfoById(int id)
   {
      var customer = await _customerService.GetCustomerInfoById(id);
      
      if (customer == null)
      {
         return NotFound($"Customer with ID {id} not found");
      }
      
      return Ok(customer);
   }

   [HttpGet("Project-details-for-customer/{id:int}")]
   public async Task<IActionResult> GetAllProjectDetailsById(int id)
   {
      var customer = await _customerService.GetAllProjectDetailsById(id);

      if (customer == null)
      {
         return NotFound($"Customer with ID {id} not found");
      }
      
      return Ok(customer);
   }
   
   [RoleAccessor(true)]
   [HttpPost("Add-customer")]
   public async Task<IActionResult> AddCustomer([FromBody] EditCustomerDto dto)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      
      var result = await _customerService.AddCustomer(dto);

      if (result.IsSuccess)
      {
         return Ok(result.Message);
      }
      
      return BadRequest(result.Message);
   }

   [RoleAccessor(true)]
   [HttpPut("Update-customer/{id:int}")]
   public async Task<IActionResult> UpdateCustomer([FromRoute] int id, [FromBody] EditCustomerDto dto)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      
      var result = await _customerService.UpdateCustomer(dto, id);
      if (result.IsSuccess)
      {
         return Ok($"{result.Message}");
      }
      
      return BadRequest(result.Message);
   }

   [RoleAccessor(true)]
   [HttpDelete("Delete-customer/{id:int}")]
   public async Task<IActionResult> DeleteCustomer(int id)
   {
      var result = await _customerService.DeleteCustomer(id);
      if (result.IsSuccess)
      {
         return Ok($"{result.Message}");
      }
      
      return BadRequest(result.Message);
   }
}