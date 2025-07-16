namespace ResumeGenerator.Core.Interfaces
{
    public interface IAIService
    {
        Task<string> GenerateResumeContentAsync(string jobDescription, List<string> skills, string experience);
        Task<string> OptimizeResumeAsync(string resumeContent);
        Task<List<string>> SuggestSkillsAsync(string jobDescription);
        Task<string> GenerateCoverLetterAsync(string resumeContent, string jobDescription, string companyName);
    }
}
