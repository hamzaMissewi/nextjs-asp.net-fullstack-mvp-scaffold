using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ResumeGenerator.Core.Interfaces;
using System.Security.Claims;

namespace ResumeGenerator.API.Hubs
{
    [Authorize]
    public class ResumeHub : Hub
    {
        private readonly IResumeService _resumeService;

        public ResumeHub(IResumeService resumeService)
        {
            _resumeService = resumeService;
        }

        public async Task JoinResumeGroup(string resumeId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"resume_{resumeId}");
        }

        public async Task LeaveResumeGroup(string resumeId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"resume_{resumeId}");
        }

        public async Task UpdateResumeContent(string resumeId, string content, string section)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Broadcast the update to all users in the resume group
            await Clients.Group($"resume_{resumeId}").SendAsync("ResumeContentUpdated", new
            {
                ResumeId = resumeId,
                Content = content,
                Section = section,
                UpdatedBy = userId,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task SendTypingIndicator(string resumeId, string section, bool isTyping)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = Context.User?.FindFirst(ClaimTypes.Name)?.Value;

            await Clients.Group($"resume_{resumeId}").SendAsync("TypingIndicator", new
            {
                ResumeId = resumeId,
                Section = section,
                UserId = userId,
                UserName = userName,
                IsTyping = isTyping
            });
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Clients.All.SendAsync("UserConnected", userId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await Clients.All.SendAsync("UserDisconnected", userId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
