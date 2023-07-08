using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Models;
using webapi.Utility;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public PatientController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult GetPatients()
        {
            var patients = _db.Patients
                .Include(p => p.Person)
                .Include(p => p.Diagnoses)
                .ToList();

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public IActionResult GetPatient(int id)
        {
            var patient = _db.Patients
                .Include(p => p.Person)
                .Include(p => p.Diagnoses)
                .FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }
        [HttpPost]
        public IActionResult CreatePatient([FromBody] PostPatientRequest data)
        {
            var person = _db.Persons.FirstOrDefault(p => p.Id == data.PersonId);
            if (person == null)
            {
                return BadRequest("Invalid PersonId");
            }

            // Получение списка объектов Diagnose по их идентификаторам
            var diagnoses = _db.Diagnoses.Where(d => data.DiagnoseIds.Contains(d.Id)).ToList();

            // Создание объекта Patient и привязка полученных объектов
            var patient = new Patient
            {
                PersonId = data.PersonId,
                Person = person,
                Diagnoses = diagnoses
            };

            _db.Patients.Add(patient);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePatient([FromBody]UpdatePatientRequest request)
        {
            var existingPatient = _db.Patients.Include(p => p.Person).Include(p => p.Diagnoses).FirstOrDefault(p => p.Id == request.Id);

            if (existingPatient == null)
            {
                return NotFound();
            }

            // Получение объекта Person по его идентификатору
            var person = _db.Persons.FirstOrDefault(p => p.Id == request.PersonId);
            if (person == null)
            {
                return BadRequest("Invalid PersonId");
            }

            // Получение списка объектов Diagnose по их идентификаторам
            var diagnoses = _db.Diagnoses.Where(d => request.DiagnoseIds.Contains(d.Id)).ToList();

            // Обновление полей существующего пациента
            existingPatient.PersonId = request.PersonId;
            existingPatient.Person = person;
            existingPatient.Diagnoses = diagnoses;

            _db.SaveChanges();

            return Ok(existingPatient);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var patient = _db.Patients.FirstOrDefault(p => p.Id == id);

            if (patient == null)
            {
                return NotFound();
            }

            _db.Patients.Remove(patient);
            _db.SaveChanges();

            return NoContent();
        }
    }

}
