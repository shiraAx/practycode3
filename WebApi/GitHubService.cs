﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Octokit;


namespace WebApi
{
    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;

        public GitHubService(string accessToken)
        {
            _client = new GitHubClient(new ProductHeaderValue("MyApp"));
            //_client.Credentials = new Credentials(accessToken);
            _client.Credentials = new Credentials("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");
        }

        public async Task<IReadOnlyList<Repository>> GetRepositories()
        {
            return await _client.Repository.GetAllForCurrent();
        }
        public async Task<List<RepositoryInfo>> GetPortfolio()
        {
            var repositories = await GetRepositories();

            var repositoryInfos = new List<RepositoryInfo>();

            foreach (var repository in repositories)
            {
                var language = repository.Language;
                var commits = await _client.Repository.Commit.GetAll(repository.Owner.Login, repository.Name);
                var lastCommit = commits.First();
                Console.WriteLine(lastCommit.ToString());
                var stars = repository.StargazersCount;
                Console.WriteLine(stars.ToString());
                var pullRequests = await _client.PullRequest.GetAllForRepository(repository.Owner.Login, repository.Name);
                Console.WriteLine(pullRequests.ToString());
                var website = repository.GitUrl;

                var repositoryInfo = new RepositoryInfo
                {
                    Name = repository.Name,
                    Language = language,
                    LastCommit = lastCommit.Commit.Message,
                    Stars = stars,
                    PullRequests = pullRequests.Count,
                    Website = website
                };

                repositoryInfos.Add(repositoryInfo);
            }

            return repositoryInfos;
        }
        public async Task<List<string>> SearchRepositories([BindNever] string name, [BindNever] string language, [BindNever] string userName)
        {
            List<string> repositoryList = new List<string>();
            var repositories = await GetRepositories();
            foreach (var repository in repositories)
            {
                if (name != null && repository.Name != name)
                    Console.WriteLine(repository.Name);
                else
                {
                    if (language != null && repository.Language != language)
                        Console.WriteLine(repository.Language);
                    else
                    {
                        //if (userName != "" && _client.User.ToString() != userName)
                        //    Console.WriteLine(_client.User);
                        //else
                        {
                            Console.WriteLine(repository.Name);
                            repositoryList.Add(repository.Name);
                        }
                    }
                }
            }
            return repositoryList;
        }
    }
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public string LastCommit { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public string Website { get; set; }
    }
}