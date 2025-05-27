using Microsoft.AspNetCore.Http;
using Library.Models;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class CountryController : ControllerBase
        {
            private readonly ICountryService _CountryService;
            public CountryController(ICountryService CountryService)
            {
                _CountryService = CountryService;
            }


            // GET: api/<UserTypeController>
            [HttpGet]
            public async Task<ActionResult<List<Country>>> GetAllCountry()
            {
                return Ok(await _CountryService.GetAll());
            }

            // GET api/<UserTypeController>/5
            [HttpGet("{idCountry}")]
            public async Task<ActionResult<Country>> GetCountry(int idCountry)
            {
                var country = await _CountryService.GetCountry(idCountry);
                if (country == null)
                {
                    return BadRequest("Country not found :(");
                }
                return Ok(country);
            }

            // UserType: api/UserType
            [HttpPost]
            public async Task<ActionResult<Country>> PostCountry(string countryName)
            {
                var CountryToPut = await _CountryService.CreateCountry(countryName);

                if (CountryToPut != null)
                {
                    return Ok(CountryToPut);
                }
                else
                {
                    return BadRequest("Error when inserting into the database :(");
                }


            }

            // PUT: api/UserType/5
            [HttpPut("Update/{idCountry}")]
            public async Task<ActionResult<Country>> PutCountry(int idCountry, string countryName)
            {
                var CountryToPut = await _CountryService.UpdateCountry(idCountry, countryName);

                if (CountryToPut != null)
                {
                    return Ok(CountryToPut);
                }
                else
                {
                    return BadRequest("Error updating the database :(");
                }

            }

            // Delete: api/UserType/5
            [HttpDelete("Delete/{idCountry}")]
            public async Task<ActionResult<Country>> DeleteCountry(int idCountry)
            {

                var CountryToDelete = await _CountryService.DeleteCountry(idCountry);

                if (CountryToDelete != null)
                {
                    return Ok(CountryToDelete);
                }
                else
                {
                    return BadRequest("Error updating the database :(");
                }
            }
        }

    }

