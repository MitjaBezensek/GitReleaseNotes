using System;
using ApprovalTests;
using GitReleaseNotes.Git;
using GitReleaseNotes.IssueTrackers;
using LibGit2Sharp;
using Xunit;

namespace GitReleaseNotes.Tests
{
    public class ReleaseNotesGeneratorTests
    {
        [Fact]
        public void AllTagsWithNoCommitsOrIssuesAfterLastRelease()
        {
            IRepository repo;
            IIssueTracker issueTracker;
            new TestDataCreator(new DateTimeOffset(2012, 1, 1, 0, 0, 0, new TimeSpan()))
                .CreateRelease("0.1.0", "Issue1", "Issue2")
                .CreateRelease("0.2.0", "Issue3")
                .Build(out repo, out issueTracker);

            var tagToStartFrom = GitRepositoryInfoFinder.GetFirstCommit(repo);
            var currentReleaseInfo = GitRepositoryInfoFinder.GetCurrentReleaseInfo(repo); 

            var releaseNotes = ReleaseNotesGenerator.GenerateReleaseNotes(
                repo, issueTracker, new SemanticReleaseNotes(), new string[0],
                tagToStartFrom, currentReleaseInfo);

            Approvals.Verify(releaseNotes.ToString());
        }

        [Fact]
        public void AllTags()
        {
            IRepository repo;
            IIssueTracker issueTracker;
            new TestDataCreator(new DateTimeOffset(2012, 1, 1, 0, 0, 0, new TimeSpan()))
                .CreateRelease("0.1.0", "Issue1", "Issue2")
                .CreateRelease("0.2.0", "Issue3")
                .AddIssues("Issue4")
                .Build(out repo, out issueTracker);

            var tagToStartFrom = GitRepositoryInfoFinder.GetFirstCommit(repo);
            var currentReleaseInfo = GitRepositoryInfoFinder.GetCurrentReleaseInfo(repo); 

            var releaseNotes = ReleaseNotesGenerator.GenerateReleaseNotes(
                repo, issueTracker, new SemanticReleaseNotes(), new string[0],
                tagToStartFrom, currentReleaseInfo);

            Approvals.Verify(releaseNotes.ToString());
        }

    }
}