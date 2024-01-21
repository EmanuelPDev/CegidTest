using APICatalogo.Repository;
using AutoMapper;
using DocumentNumber.ValidatorAbstractions;
using EmanuelCegidTest.DTOs;
using EmanuelCegidTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portugal.Nif.Validator;

namespace EmanuelCegidTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesItemController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _Mapper;

        public SalesItemController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _Mapper = mapper;
        }

        [HttpGet()]
        public async Task< ActionResult<IEnumerable<SalesItemsDTO>>> GetSalesItems()
        {
            List<SalesItems> SalesItems = await _context.SalesItemsRepository.Get().ToListAsync();
            if (SalesItems is null)
                return NotFound("Items not found.");

            List<SalesItemsDTO> SalesItemsDTO = _Mapper.Map<List<SalesItemsDTO>>(SalesItems);

            return Ok(SalesItemsDTO);
        }

        [HttpGet("{id:int}", Name = "GetItem")]
        public async Task<ActionResult<SalesItemsDTO>> GetSalesItem(int id)
        {
            SalesItems SalesItem = await _context.SalesItemsRepository.GetById(S => S.ID == id);

            if (SalesItem is null)
                return NotFound("SlesItem not found.");

            SalesItemsDTO salesItemsDTO = _Mapper.Map<SalesItemsDTO>(SalesItem);

            return Ok(salesItemsDTO);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSalesItem(SalesItemsDTO SalesItemDTO)
        {
            if (SalesItemDTO is null)
                return BadRequest();

            if (!(SalesItemDTO.Price > 0))
                return BadRequest("The price must be greater than 0.");

            SalesItems SalesItem = _Mapper.Map<SalesItems>(SalesItemDTO);

            SalesItem.CreationDate = DateTime.Now;

            _context.SalesItemsRepository.Add(SalesItem);
            await _context.Commit();

            return new CreatedAtRouteResult("GetItem", new { id = SalesItem.ID }, SalesItemDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateSalesItem(int id, SalesItemsDTO SalesItemDTO)
        {
            SalesItems recSalesItem = await _context.SalesItemsRepository.GetById(S => S.ID == id);

            if (id != SalesItemDTO.ID)
                return BadRequest("ID does not match.");

            if (!(SalesItemDTO.Price > 0))
                return BadRequest("The price must be greater than 0.");

            SalesItems SalesItem = _Mapper.Map<SalesItems>(SalesItemDTO);

            SalesItem.LastUpdate = DateTime.Now;
            SalesItem.CreationDate = recSalesItem.CreationDate;
            _context.SalesItemsRepository.Update(SalesItem);
            await _context.Commit();

            return Ok(SalesItemDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteSalesItem(int id)
        {
            SalesItems SalesItem = await _context.SalesItemsRepository.GetById(S => S.ID == id);

            if (SalesItem is null)
                return NotFound("SalesItem not found.");

            _context.SalesItemsRepository.Delete(SalesItem);
            await _context.Commit();
            SalesItemsDTO SalesItemsDTO = _Mapper.Map<SalesItemsDTO>(SalesItem);

            return Ok(SalesItemsDTO);
        }
    }
}
