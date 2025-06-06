using CopilotInstructions.Web.Models;
using Microsoft.Extensions.Options;
using Octokit;

namespace CopilotInstructions.Web.Services
{
    public class CopilotInstructionsService : ICopilotInstructionsService
    {
        private readonly GitHubConfig _config;
        private readonly GitHubClient _gitHubClient;
        private readonly ILogger<CopilotInstructionsService> _logger;

        public CopilotInstructionsService(IOptions<GitHubConfig> config, ILogger<CopilotInstructionsService> logger)
        {
            _config = config.Value;
            _logger = logger;
            
            _gitHubClient = new GitHubClient(new ProductHeaderValue("CopilotInstructions-Web"));
            if (!string.IsNullOrEmpty(_config.Token))
            {
                _gitHubClient.Credentials = new Credentials(_config.Token);
            }
        }

        public async Task<CommitInstruction?> GetCommitInstructionAsync(string title)
        {
            try
            {
                var issues = await _gitHubClient.Issue.GetAllForRepository(
                    _config.Owner,
                    _config.Repository,
                    new RepositoryIssueRequest
                    {
                        Labels = { "custominstruction" },
                        State = ItemStateFilter.Open
                    });

                var issue = issues.FirstOrDefault(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                
                if (issue != null)
                {
                    return new CommitInstruction
                    {
                        Title = issue.Title,
                        Description = issue.Body ?? string.Empty,
                        Author = issue.User.Login,
                        Date = issue.CreatedAt.ToString("yyyy-MM-dd")
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commit instruction: {Title}", title);
                return null;
            }
        }

        public async Task<IEnumerable<CommitInstruction>> GetCommitInstructionsAsync()
        {
            try
            {
                var issues = await _gitHubClient.Issue.GetAllForRepository(
                    _config.Owner,
                    _config.Repository,
                    new RepositoryIssueRequest
                    {
                        Labels = { "custominstruction" },
                        State = ItemStateFilter.Open
                    });

                return issues.Select(issue => new CommitInstruction
                {
                    Title = issue.Title,
                    Description = issue.Body ?? string.Empty,
                    Author = issue.User.Login,
                    Date = issue.CreatedAt.ToString("yyyy-MM-dd")
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting commit instructions");
                return Enumerable.Empty<CommitInstruction>();
            }
        }

        public async Task<CopilotInstruction?> GetCopilotInstructionAsync(string title)
        {
            try
            {
                var issues = await _gitHubClient.Issue.GetAllForRepository(
                    _config.Owner,
                    _config.Repository,
                    new RepositoryIssueRequest
                    {
                        Labels = { "custominstruction" },
                        State = ItemStateFilter.Closed
                    });

                var issue = issues.FirstOrDefault(i => i.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
                
                if (issue != null)
                {
                    return new CopilotInstruction
                    {
                        Title = issue.Title,
                        Description = issue.Body ?? string.Empty,
                        Author = issue.User.Login,
                        Date = issue.CreatedAt.ToString("yyyy-MM-dd")
                    };
                }
                
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting copilot instruction: {Title}", title);
                return null;
            }
        }

        public async Task<IEnumerable<CopilotInstruction>> GetCopilotInstructionsAsync()
        {
            try
            {
                var issues = await _gitHubClient.Issue.GetAllForRepository(
                    _config.Owner,
                    _config.Repository,
                    new RepositoryIssueRequest
                    {
                        Labels = { "custominstruction" },
                        State = ItemStateFilter.Closed
                    });

                return issues.Select(issue => new CopilotInstruction
                {
                    Title = issue.Title,
                    Description = issue.Body ?? string.Empty,
                    Author = issue.User.Login,
                    Date = issue.CreatedAt.ToString("yyyy-MM-dd")
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting copilot instructions");
                return Enumerable.Empty<CopilotInstruction>();
            }
        }
    }
}
