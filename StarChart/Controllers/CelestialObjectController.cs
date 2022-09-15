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

        [HttpGet(Name="GetById")]
        [Route("{id:int}")]
        public IActionResult GetById(int id)
        {
            var celestialBody = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);
            if(celestialBody==null)
            {
                return NotFound();
            }
            celestialBody.Satellites = _context.CelestialObjects.FirstOrDefault(x => x.OrbitedObjectId == celestialBody.Id).Satellites;
            return Ok(celestialBody);
        }


        [HttpGet]
        [Route("{name:string}")]
        public IActionResult GetByName(string name)
        {
            var celestialBody = _context.CelestialObjects.FirstOrDefault(x => x.Name.Equals(name,StringComparison.OrdinalIgnoreCase));
            if (celestialBody == null)
            {
                return NotFound();
            }
            _context.CelestialObjects.ToList().ForEach(_ =>
            {
                if (_.OrbitedObjectId == celestialBody.Id)
                    _.Satellites = _.Satellites;
            });
            return Ok(celestialBody);
        }
        [HttpGet]
        public IActionResult GetAll()
        {

            return Ok(_context.CelestialObjects);
        }
    }
}
