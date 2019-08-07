using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using ShortStoryBOL;

namespace ShortStoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryDb storyDb;

        public StoriesController(IStoryDb storyDb)
        {
            this.storyDb = storyDb;
        }

        // GET: api/Stories
       // [ResponseCache(Duration =30)]
        [HttpGet]
        public async Task<ActionResult> GetStories()
        {
            return Ok(await storyDb.GetAll().ToListAsync());
        }

        // GET: api/Stories/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Story>> GetStory(int id) 
        //{
        //    var story = await _context.Stories.FindAsync(id);

        //    if (story == null)
        //    {
        //        return NotFound();
        //    }

        //    return story;
        //}

        //// PUT: api/Stories/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutStory(int id, Story story)
        //{
        //    if (id != story.SSId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(story).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Stories
        //[HttpPost]
        //public async Task<ActionResult<Story>> PostStory(Story story)
        //{
        //    _context.Stories.Add(story);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetStory", new { id = story.SSId }, story);
        //}

        //// DELETE: api/Stories/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Story>> DeleteStory(int id)
        //{
        //    var story = await _context.Stories.FindAsync(id);
        //    if (story == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Stories.Remove(story);
        //    await _context.SaveChangesAsync();

        //    return story;
        //}

        //private bool StoryExists(int id)
        //{
        //    return _context.Stories.Any(e => e.SSId == id);
        //}
    }
}
