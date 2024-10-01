using backend_Master.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_Master.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesCommentsCoursesBeforeController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ResponsesCommentsCoursesBeforeController(MyDbContext context)
        {
            _context = context;
        }

        // Retrieve replies for a specific comment
        [HttpGet("comment/{commentId}")]
        public async Task<ActionResult<IEnumerable<ResponsesCommentsCoursesBefore>>> GetResponsesForComment(int commentId)
        {
            try
            {
                var responses = await _context.ResponsesCommentsCoursesBefores
                                              .Where(r => r.CommentId == commentId)
                                              .ToListAsync();

                if (responses == null || !responses.Any())
                {
                    return NotFound($"No responses found for comment ID {commentId}");
                }

                return Ok(responses);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        // Add a new reply to a comment
        [HttpPost]
        public async Task<ActionResult<ResponsesCommentsCoursesBefore>> PostResponse(ResponsesCommentsCoursesBefore response)
        {
            if (response == null)
            {
                return BadRequest("Invalid response data.");
            }

            // Check if the comment exists
            var commentExists = await _context.CommentsCoursesBefores.AnyAsync(c => c.CommentId == response.CommentId);
            if (!commentExists)
            {
                return NotFound($"Comment with ID {response.CommentId} not found.");
            }

            _context.ResponsesCommentsCoursesBefores.Add(response);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetResponsesForComment), new { commentId = response.CommentId }, response);
        }

        // Retrieve all replies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsesCommentsCoursesBefore>>> GetResponses()
        {
            return await _context.ResponsesCommentsCoursesBefores.ToListAsync();
        }
    }
}
