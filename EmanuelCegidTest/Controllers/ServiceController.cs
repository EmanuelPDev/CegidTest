using APICatalogo.Repository;
using DocumentNumber.Portugal.Nif.Generator;
using EmanuelCegidTest.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmanuelCegidTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly INifGenerator _NifGenerator;
        public ServiceController(INifGenerator NifGenerator)
        {
            _NifGenerator = NifGenerator;
        }

        [HttpGet("GetValidNif")]
        public ActionResult<string> GetGeneratedNif()
        {
            return Ok(_NifGenerator.GenerateDocumentNumber());
        }
    }
}
