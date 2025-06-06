using CopilotInstructions.Web.Models;

namespace CopilotInstructions.Web.Services
{
    public interface ICopilotInstructionsService
    {
        Task<IEnumerable<CopilotInstruction>> GetCopilotInstructionsAsync();
        Task<CopilotInstruction?> GetCopilotInstructionAsync(string title);
        Task<IEnumerable<CommitInstruction>> GetCommitInstructionsAsync();
        Task<CommitInstruction?> GetCommitInstructionAsync(string title);
    }
}
