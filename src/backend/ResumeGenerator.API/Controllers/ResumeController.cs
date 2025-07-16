using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeGenerator.Core.DTOs;
using ResumeGenerator.Core.Interfaces;
using System.Security.Claims;

namespace ResumeGenerator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeService _resumeService;
        private readonly IAIService _aiService;

        public ResumeController(IResumeService resumeService, IAIService aiService)
        {
            _resumeService = resumeService;
            _aiService = aiService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResumeDto>>> GetResumes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resumes = await _resumeService.GetUserResumesAsync(userId);
            return Ok(resumes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeDto>> GetResume(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resume = await _resumeService.GetResumeAsync(id, userId);
            
            if (resume == null)
                return NotFound();

            return Ok(resume);
        }

        [HttpPost]
        public async Task<ActionResult<ResumeDto>> CreateResume(CreateResumeDto createResumeDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resume = await _resumeService.CreateResumeAsync(createResumeDto, userId);
            return CreatedAtAction(nameof(GetResume), new { id = resume.Id }, resume);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResume(int id, UpdateResumeDto updateResumeDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _resumeService.UpdateResumeAsync(id, updateResumeDto, userId);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResume(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var success = await _resumeService.DeleteResumeAsync(id, userId);
            
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/generate-content")]
        public async Task<ActionResult<string>> GenerateContent(int id, [FromBody] GenerateContentRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resume = await _resumeService.GetResumeAsync(id, userId);
            
            if (resume == null)
                return NotFound();

            var generatedContent = await _aiService.GenerateResumeContentAsync(request.JobDescription, request.Skills, request.Experience);
            return Ok(new { content = generatedContent });
        }

        [HttpPost("{id}/optimize")]
        public async Task<ActionResult<string>> OptimizeResume(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var resume = await _resumeService.GetResumeAsync(id, userId);
            
            if (resume == null)
                return NotFound();

            var optimizedContent = await _aiService.OptimizeResumeAsync(resume.Content);
            return Ok(new { optimizedContent });
        }
    }

    public class GenerateContentRequest
    {
        public string JobDescription { get; set; }
        public List<string> Skills { get; set; }
        public string Experience { get; set; }
    }
}
