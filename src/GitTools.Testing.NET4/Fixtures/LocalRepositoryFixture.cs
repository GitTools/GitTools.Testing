using GitTools.Testing.Logging;

namespace GitTools.Testing.Fixtures
{
    using LibGit2Sharp;

    public class LocalRepositoryFixture : RepositoryFixtureBase
    {
        public LocalRepositoryFixture(IRepository repository) : base(repository)
        {
        }
    }
}