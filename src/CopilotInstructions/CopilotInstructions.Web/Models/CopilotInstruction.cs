namespace CopilotInstructions.Web.Models
{
    public class CopilotInstruction
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Author { get; set; }
        public string? Email { get; set; }
        public string? Date { get; set; }
    }
}
