using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Data;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PersonController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            var person = _db.Persons.ToList();
            return Ok(person);
        }
        [HttpGet("{id}")]
        public ActionResult<Person> Get(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_db.Persons.Any(x => x.Id == id))
            {
                return NotFound();
            }
            var person = _db.Persons.FirstOrDefault(x => x.Id == id);
            return Ok(person);
        }
        [HttpPost]
        public ActionResult<Person> Post([FromBody] Person person)
        {
            if (person == null)
            {
                return BadRequest();
            }
            _db.Persons.Add(person);
            _db.SaveChanges();
            return Ok(person);
        }
        [HttpPut("{id}")]
        public ActionResult<Person> UpdatePerson(int id, [FromBody] Person updatedPerson)
        {
            if (updatedPerson == null)
            {
                return BadRequest();
            }

            var existingPerson = _db.Persons.FirstOrDefault(p => p.Id == id);

            if (existingPerson == null)
            {
                return NotFound();
            }

            // Обновляем поля объекта existingPerson на основе переданного updatedPerson
            existingPerson.Name = updatedPerson.Name;
            existingPerson.Surname = updatedPerson.Surname;
            // Обновите другие поля в соответствии с вашей логикой

            _db.Persons.Update(existingPerson);
            _db.SaveChanges();

            return Ok(existingPerson);
        }
        [HttpDelete("{id}")]
        public ActionResult<Person> Delete(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (!_db.Persons.Any(x => x.Id == id))
            {
                return NotFound();
            }
            var person = _db.Persons.FirstOrDefault(x => x.Id == id);
            _db.Remove(person);
            _db.SaveChanges();
            return NoContent();

        }
    }
}
