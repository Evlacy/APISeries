using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIRestV1.Models.EntityFramework;

namespace APIRestV1.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly SeriesDbContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SeriesController(SeriesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all series
        /// </summary>
        /// <returns>Http response</returns>
        /// <response code="200">When the series are found</response>
        /// <response code="404">When the series are not found</response>
        // GET: api/Series
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Serie>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<Serie>>> GetSeries()
        {
            return await _context.Series.ToListAsync();
        }

        /// <summary>
        /// Get a single serie
        /// </summary>
        /// <param name="id">Serie's id</param>
        /// <response code="200">When the serie is found</response>
        /// <response code="404">When the serie is not found</response>
        // GET: api/Series/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Serie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Serie>> GetSerie(int id)
        {
            var serie = await _context.Series.FindAsync(id);

            if (serie == null)
            {
                return NotFound();
            }

            return serie;
        }

        /// <summary>
        /// Put a single serie
        /// </summary>
        /// <param name="id">Serie's id</param>
        /// <param name="serie">The serie</param>
        /// <returns>Http response</returns>
        /// <response code="400">Client error </response>
        /// <response code="204">When the serie id is found</response>
        /// <response code="404">When the serie id is not found</response>
        // PUT: api/Series/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutSerie(int id, Serie serie)
        {
            if (id != serie.Serieid)
            {
                return BadRequest();
            }

            _context.Entry(serie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SerieExists(id))
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

        /// <summary>
        /// Post a single serie
        /// </summary>
        /// <param name="serie">The serie</param>
        /// <returns>Http response</returns>
        /// <response code="200">Posted</response>
        /// <response code="400">Client error</response>
        // POST: api/Series
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Serie>> PostSerie(Serie serie)
        {
            _context.Series.Add(serie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSerie", new { id = serie.Serieid }, serie);
        }

        /// <summary>
        /// Delete a single serie
        /// </summary>
        /// <param name="id">The serie's id</param>
        /// <returns>Http response</returns>
        /// <response code="204">When the serie id is found</response>
        /// <response code="404">When the serie id is not found</response>
        // DELETE: api/Series/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSerie(int id)
        {
            var serie = await _context.Series.FindAsync(id);
            if (serie == null)
            {
                return NotFound();
            }

            _context.Series.Remove(serie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Existing series
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Found</response>
        /// <response code="404">When the serie id is not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private bool SerieExists(int id)
        {
            return _context.Series.Any(e => e.Serieid == id);
        }
    }
}
