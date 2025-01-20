using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppQuiz.Data;

namespace WebAppQuiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HighScoresController(QuizDbContext _context) : ControllerBase
    {

        // GET: api/HighScores
        [HttpGet]
        public async Task<ActionResult> GetHighScores()
        {
            var highScores = await _context.HighScores.OrderByDescending(h => h.Score) // Sort by score in descending order
                 .Take(10).ToListAsync();
            return Ok(highScores);
        }

        // GET: api/HighScores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HighScore>> GetHighScores(int id)
        {
            var highScores = await _context.HighScores.FindAsync(id);

            if (highScores == null)
            {
                return NotFound();
            }

            return Ok(highScores);
        }


        // POST: api/HighScores
        [HttpPost]
        public async Task<ActionResult<HighScore>> PostHighScores([FromBody] HighScoreDto dto)
        {
            if (string.IsNullOrEmpty(dto.email) || !new EmailAddressAttribute().IsValid(dto.email))
            {
                return BadRequest("Invalid email address.");
            }
            List<Question> questions = await _context.Questions.ToListAsync();

            int totalScore = 0;
            var pointCalculatorVisitor = new PointCalculationVisitor();
            // Iterate through the answers in the DTO
            foreach (var answerPair in dto.answers)
            {
                var questionId = answerPair.Key;
                var userAnswer = answerPair.Value;

                // Find the question by its ID
                var question = questions.FirstOrDefault(q => q.Id == questionId);

                if (question != null)
                {
                    if (question is MultiAnswerQuestion)
                    {
                        // Parse the JSON array string into a List<string>
                        var multiAnswer = JsonSerializer.Deserialize<List<string>>(userAnswer.ToString()!);
                        if (multiAnswer != null)
                        {
                            totalScore += question.Accept(pointCalculatorVisitor, multiAnswer);
                        }
                    }
                    else
                    {
                        // Handle single answer or text question
                        var singleAnswer = userAnswer?.ToString();
                        totalScore += question.Accept(pointCalculatorVisitor, singleAnswer);
                    }
                }
            }

            // Create a HighScore object to save in the database
            var highScore = new HighScore(dto.email, totalScore, DateTime.Now);

            // Save the high score in the database
            _context.HighScores.Add(highScore);
            await _context.SaveChangesAsync();

            // Return the created high score with a location header
            return CreatedAtAction("GetHighScores", new { id = highScore.Id }, highScore);
        }

        // DELETE: api/HighScores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHighScores(int id)
        {
            var highScores = await _context.HighScores.FindAsync(id);
            if (highScores == null)
            {
                return NotFound();
            }

            _context.HighScores.Remove(highScores);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HighScoresExists(int id)
        {
            return _context.HighScores.Any(e => e.Id == id);
        }
    }
}
