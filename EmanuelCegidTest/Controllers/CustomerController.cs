using APICatalogo.Repository;
using AutoMapper;
using EmanuelCegidTest.DTOs;
using EmanuelCegidTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portugal.Nif.Validator;

namespace EmanuelCegidTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly INifValidator _NifValidator;
        private readonly IMapper _Mapper;
        public CustomerController(IUnitOfWork context, INifValidator nifValidator, IMapper mapper)
        {
            _context = context;
            _NifValidator = nifValidator;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersDTO>>> GetCustomers()
        {
            List<Customers> Customers = await _context.CostomerRepository.Get().ToListAsync();
            if (Customers is null)
                return NotFound("Customer not found.");

            List<CustomersDTO> CustomersDTO = _Mapper.Map<List<CustomersDTO>>(Customers);
            return Ok(CustomersDTO);
        }

        [HttpGet("{id:int}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomersDTO>> GetCustomer(int id)
        {
            Customers Customer = await _context.CostomerRepository.GetById(C => C.ID == id);
            if (Customer is null)
                return NotFound("Customer not found.");

            CustomersDTO CustomerDTO = _Mapper.Map<CustomersDTO>(Customer);

            return Ok(CustomerDTO);
        }

        [HttpPost]
        public async Task<ActionResult> SaveCustomer(CustomersDTO CustomerDTO)
        {
            if (CustomerDTO is null)
                return BadRequest();

            if (!_NifValidator.Validate(CustomerDTO.TaxID) && CustomerDTO.Country.ToUpper() == "PORTUGAL")
                return BadRequest("Invalid Nif.");

            Customers Customer = _Mapper.Map<Customers>(CustomerDTO);

            Customer.CreationDate = DateTime.Now;
            _context.CostomerRepository.Add(Customer);
            await _context.Commit();

            return new CreatedAtRouteResult("GetCustomer", new { id = Customer.ID }, CustomerDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCustomer(int id, CustomersDTO CustomerDTO)
        {
            Customers recCustomer = await _context.CostomerRepository.GetById(C => C.ID == id);

            if (id != CustomerDTO.ID || recCustomer is null)
                return BadRequest("Customer not found.");

            if (!_NifValidator.Validate(CustomerDTO.TaxID) && CustomerDTO.Country.ToUpper() == "PORTUGAL")
                return BadRequest("Invalid Nif.");

            Customers Customer = _Mapper.Map<Customers>(CustomerDTO);
            Customer.LastUpdate = DateTime.Now;
            Customer.CreationDate = recCustomer.CreationDate;
            _context.CostomerRepository.Update(Customer);
            await _context.Commit();

            return Ok(CustomerDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            Customers Customer = await _context.CostomerRepository.GetById(C => C.ID == id);

            if (Customer is null)
                return NotFound("Customer not found.");

            _context.CostomerRepository.Delete(Customer);
            await _context.Commit();

            CustomersDTO CustomerDTO = _Mapper.Map<CustomersDTO>(Customer);

            return Ok(CustomerDTO);
        }
    }
}
