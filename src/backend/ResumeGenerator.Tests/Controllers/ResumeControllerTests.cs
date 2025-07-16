using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ResumeGenerator.API.Controllers;
using ResumeGenerator.Core.DTOs;
using ResumeGenerator.Core.Interfaces;
using System.Security.Claims;
using Xunit;

namespace ResumeGenerator.Tests.Controllers
{
    public class ResumeControllerTests
    {
        private readonly Mock<IResumeService> _mockResumeService;
        private readonly Mock<IAIService> _mockAIService;
        private readonly ResumeController _controller;

        public ResumeControllerTests()
        {
            _mockResumeService = new Mock<IResumeService>();
            _mockAIService = new Mock<IAIService>();
            _controller = new ResumeController(_mockResumeService.Object, _mockAIService.Object);

            // Setup user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                new Claim(ClaimTypes.Name, "test@example.com"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Fact]
        public async Task GetResumes_ReturnsOkResult_WithListOfResumes()
        {
            // Arrange
            var resumes = new List<ResumeDto>
            {
                new ResumeDto { Id = 1, Title = "Software Engineer Resume", UserId = "test-user-id" },
                new ResumeDto { Id = 2, Title = "Data Scientist Resume", UserId = "test-user-id" }
            };

            _mockResumeService.Setup(x => x.GetUserResumesAsync("test-user-id"))
                .ReturnsAsync(resumes);

            // Act
            var result = await _controller.GetResumes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedResumes = Assert.IsType<List<ResumeDto>>(okResult.Value);
            Assert.Equal(2, returnedResumes.Count);
        }

        [Fact]
        public async Task CreateResume_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createResumeDto = new CreateResumeDto
            {
                Title = "New Resume",
                Content = "Resume content",
                TemplateId = 1
            };

            var createdResume = new ResumeDto
            {
                Id = 1,
                Title = "New Resume",
                Content = "Resume content",
                UserId = "test-user-id"
            };

            _mockResumeService.Setup(x => x.CreateResumeAsync(createResumeDto, "test-user-id"))
                .ReturnsAsync(createdResume);

            // Act
            var result = await _controller.CreateResume(createResumeDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetResume), createdAtActionResult.ActionName);
            Assert.Equal(createdResume.Id, ((ResumeDto)createdAtActionResult.Value).Id);
        }

        [Fact]
        public async Task GenerateContent_ReturnsOkResult_WithGeneratedContent()
        {
            // Arrange
            var resumeId = 1;
            var request = new GenerateContentRequest
            {
                JobDescription = "Software Engineer position",
                Skills = new List<string> { "C#", "React", "SQL" },
                Experience = "5 years of experience"
            };

            var resume = new ResumeDto { Id = resumeId, UserId = "test-user-id" };
            var generatedContent = "Generated resume content based on job description";

            _mockResumeService.Setup(x => x.GetResumeAsync(resumeId, "test-user-id"))
                .ReturnsAsync(resume);

            _mockAIService.Setup(x => x.GenerateResumeContentAsync(
                request.JobDescription, request.Skills, request.Experience))
                .ReturnsAsync(generatedContent);

            // Act
            var result = await _controller.GenerateContent(resumeId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = okResult.Value;
            Assert.NotNull(response);
        }
    }
}
