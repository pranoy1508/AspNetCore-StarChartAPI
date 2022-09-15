using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}",Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var celestialBody = _context.CelestialObjects.Find(id);
            if(celestialBody==null)
            {
                return NotFound();
            }
            celestialBody.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialBody.Id).ToList();
            return Ok(celestialBody);
        }


        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialBody = _context.CelestialObjects.Where(x => x.Name==name).ToList();
            if (!celestialBody.Any())
            {
                return NotFound();
            }
            foreach(var celeObj in celestialBody)
            {
                celeObj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celeObj.Id).ToList();
            }
            return Ok(celestialBody);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialBody = _context.CelestialObjects.ToList();
            foreach (var celeObj in celestialBody)
            {
                celeObj.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celeObj.Id).ToList();
            }
            return Ok(_context.CelestialObjects);
        }
    }
}
