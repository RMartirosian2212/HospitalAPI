using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnoseController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public DiagnoseController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Diagnose>> Get()
        {
            var diagnoses = _db.Diagnoses.ToList();
            return Ok(diagnoses);
        }
        [HttpGet("{id}")]
        public ActionResult<Diagnose> Get(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_db.Diagnoses.Any(x => x.Id == id))
            {
                return NotFound();
            }
            var diagnose = _db.Diagnoses.FirstOrDefault(x => x.Id == id);
            return Ok(diagnose);
        }
        [HttpPost]
        public ActionResult<Diagnose> Post([FromBody] Diagnose diagnose)
        {
            if (diagnose == null)
            {
                return BadRequest();
            }
            _db.Diagnoses.Add(diagnose);
            _db.SaveChanges();
            return Ok(diagnose);
        }
        [HttpPut]
        public ActionResult<Diagnose> Put([FromBody] Diagnose diagnose)
        {
            if (diagnose == null)
            {
                return BadRequest();
            }
            if (!_db.Diagnoses.Any(x => x.Id == diagnose.Id))
            {
                return NotFound();
            }
            _db.Diagnoses.Update(diagnose);
            _db.SaveChanges();
            return Ok(diagnose);
        }
        [HttpDelete("{id}")]
        public ActionResult<Diagnose> Delete(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_db.Diagnoses.Any(x => x.Id == id))
            {
                return NotFound();
            }
            var diagnose = _db.Diagnoses.FirstOrDefault(x => x.Id == id);
            _db.Remove(diagnose);
            _db.SaveChanges();
            return NoContent();

        }   
    }
}
