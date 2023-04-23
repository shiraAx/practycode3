using Microsoft.AspNetCore.Mvc.ModelBinding;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
{
    public interface IGitHubService
    {
        Task<IReadOnlyList<Repository>> GetRepositories();
        Task<List<RepositoryInfo>> GetPortfolio();
        Task<List<string>> SearchRepositories([BindNever] string name, [BindNever] string language, [BindNever] string userName);
    }
}
