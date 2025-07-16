using Microsoft.Extensions.Configuration;
using ResumeGenerator.Core.Interfaces;
using System.Text;
using System.Text.Json;

namespace ResumeGenerator.Core.Services
{
    public class OpenAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _model;

        public OpenAIService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiKey = _configuration["OpenAI:ApiKey"];
            _model = _configuration["OpenAI:Model"];
            
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<string> GenerateResumeContentAsync(string jobDescription, List<string> skills, string experience)
        {
            var prompt = $@"
                Generate professional resume content based on the following information:
                
                Job Description: {jobDescription}
                Skills: {string.Join(", ", skills)}
                Experience: {experience}
                
                Please create compelling bullet points that highlight relevant achievements and skills.
                Focus on quantifiable results and action verbs.
                Format the response as clean, professional resume content.
            ";

            return await CallOpenAIAsync(prompt);
        }

        public async Task<string> OptimizeResumeAsync(string resumeContent)
        {
            var prompt = $@"
                Please optimize the following resume content for better impact and readability:
                
                {resumeContent}
                
                Improvements should include:
                - Stronger action verbs
                - Quantified achievements where possible
                - Better formatting and structure
                - ATS-friendly keywords
                - Concise and impactful language
            ";

            return await CallOpenAIAsync(prompt);
        }

        public async Task<List<string>> SuggestSkillsAsync(string jobDescription)
        {
            var prompt = $@"
                Based on this job description, suggest 10-15 relevant technical and soft skills:
                
                {jobDescription}
                
                Return only a comma-separated list of skills, no additional text.
            ";

            var response = await CallOpenAIAsync(prompt);
            return response.Split(',').Select(s => s.Trim()).ToList();
        }

        public async Task<string> GenerateCoverLetterAsync(string resumeContent, string jobDescription, string companyName)
        {
            var prompt = $@"
                Generate a professional cover letter based on:
                
                Resume Content: {resumeContent}
                Job Description: {jobDescription}
                Company Name: {companyName}
                
                The cover letter should be personalized, professional, and highlight relevant experience from the resume.
            ";

            return await CallOpenAIAsync(prompt);
        }

        private async Task<string> CallOpenAIAsync(string prompt)
        {
            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a professional resume writer and career coach." },
                    new { role = "user", content = prompt }
                },
                max_tokens = int.Parse(_configuration["OpenAI:MaxTokens"]),
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseContent);
            
            return responseObject
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
        }
    }
}
