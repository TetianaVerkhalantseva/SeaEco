using SeaEco.EntityFramework.Entities;
using SeaEco.Services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeaEco.Abstractions.Models.Customer;

namespace SeaEco.Server.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class CustomerController: ControllerBase
{
   private readonly ICustomerService _customerService;

   public CustomerController(ICustomerService customerService)
   {
      _customerService = customerService;
   }

   [HttpGet]
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
   public async Task<IActionResult> GetCustomerById(int id)
   {
      var customer = await _customerService.GetCustomerById(id);
      
      if (customer == null)
      {
         return NotFound($"Customer with ID {id} not found");
      }
      
      return Ok(customer);
   }

   [HttpPost]
   public async Task<IActionResult> AddCustomer([FromBody] AddCustomerDto dto)
   {
      if (!ModelState.IsValid)
      {
         return BadRequest(ModelState);
      }
      
      var result = await _customerService.AddCustomer(dto);

      if (result.IsSuccess)
      {
         return Ok();
      }
      
      return BadRequest(result.Message);
   }
}