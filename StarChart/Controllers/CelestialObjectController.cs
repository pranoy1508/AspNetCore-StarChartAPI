using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialBody = _context.CelestialObjects.Find(id);
            if (celestialBody == null)
            {
                return NotFound();
            }
            celestialBody.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialBody.Id).ToList();
            return Ok(celestialBody);
        }


        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialBody = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (!celestialBody.Any())
            {
                return NotFound();
            }
            foreach (var celeObj in celestialBody)
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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject CelestialObject)
        {
            _context.CelestialObjects.Add(CelestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { CelestialObject.Id }, CelestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject CelestialObject)
        {
            var celestialObj = _context.CelestialObjects.Find(id);
            if (celestialObj == null)
                return NotFound();
            celestialObj.Name = CelestialObject.Name;
            celestialObj.OrbitalPeriod = CelestialObject.OrbitalPeriod;
            celestialObj.OrbitedObjectId = CelestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(celestialObj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id,string name)
        {
            var celestialObj = _context.CelestialObjects.Find(id);
            if (celestialObj == null)
                return NotFound();
            celestialObj.Name = name;
            _context.CelestialObjects.Update(celestialObj);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObj = _context.CelestialObjects.Where(x=>x.Id==id || x.OrbitedObjectId==id);
            if (!celestialObj.Any())
                return NotFound();
            _context.CelestialObjects.RemoveRange(celestialObj);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
