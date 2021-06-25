﻿using DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiNursePlanning.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NursesController : ControllerBase
    {

        private readonly INurseRepository repository;

        public NursesController(INurseRepository nurseRepository)
        {
            repository = nurseRepository;
        }
        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nurse>>> GetNurses()
        {
            return Ok(await repository.ListNurses());
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nurse>> GetAppointment(string id)
        {
            var nurse = await repository.Details(id);

            if (nurse == null)
            {
                return NotFound();
            }

            return Ok(nurse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNurse(string id, Nurse nurse)
        {
            if (id != nurse.Id)
            {
                return BadRequest();
            }

            try
            {
                await repository.Edit(nurse);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NurseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool NurseExists(string id)
        {
            if (repository.Details(id) is null)
                return false;
            return true;
        }

    }
}
