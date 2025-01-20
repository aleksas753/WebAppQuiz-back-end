using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppQuiz.Data;

namespace WebAppQuiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionsController(QuizDbContext _context) : ControllerBase
    {
        // GET: api/Questions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            
            List<Question> questions = await _context.Questions.ToListAsync();

            return Ok(questions);
        }
    }
}
