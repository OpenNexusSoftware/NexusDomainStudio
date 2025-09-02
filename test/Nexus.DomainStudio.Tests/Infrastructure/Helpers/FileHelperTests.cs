using Nexus.DomainStudio.Infrastructure.Util;

namespace Nexus.DomainStudio.Tests.Infrastructure.Helpers;

public class FileHelperTests
{
    private static string CreateTempDir()
    {
        var dir = Path.Combine(Path.GetTempPath(), "nds-tests", "filehelper", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        return dir;
    }

    private static void Touch(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "");
    }

    private static string Norm(string p) => Path.GetFullPath(p).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

    [Fact]
    public void MatchGlobFiles_SimplePattern_MatchesFilesInDirectory()
    {
        var root = CreateTempDir();
        try
        {
            // Arrange
            var f1 = Path.Combine(root, "a.entity.json");
            var f2 = Path.Combine(root, "b.entity.json");
            var f3 = Path.Combine(root, "c.other.json"); // should not match
            Touch(f1); Touch(f2); Touch(f3);

            // Act
            var matches = FileHelper.MatchGlobFiles(root, "*.entity.json").ToArray();

            // Assert (order is not guaranteed, so compare sets)
            Assert.Equal(2, matches.Length);
            Assert.Contains(f1, matches);
            Assert.Contains(f2, matches);
            Assert.DoesNotContain(f3, matches);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void MatchGlobFiles_SubfolderPattern_MatchesOnlyInThatSubfolder()
    {
        var root = CreateTempDir();
        try
        {
            var entitiesDir = Path.Combine(root, "entities");
            var otherDir = Path.Combine(root, "aggregates");

            var e1 = Path.Combine(entitiesDir, "one.entity.json");
            var e2 = Path.Combine(entitiesDir, "two.entity.json");
            var a1 = Path.Combine(otherDir, "agg.aggregate.json");

            Touch(e1); Touch(e2); Touch(a1);

            var matches = FileHelper.MatchGlobFiles(root, "entities/*.entity.json").ToArray();
            var normalizedMatches = matches.Select(Norm).ToArray();

            Assert.Equal(2, normalizedMatches.Length);

            Assert.Contains(Norm(e1), normalizedMatches, StringComparer.OrdinalIgnoreCase);
            Assert.Contains(Norm(e2), normalizedMatches, StringComparer.OrdinalIgnoreCase);
            Assert.DoesNotContain(Norm(a1), normalizedMatches, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void MatchGlobFiles_RecursivePattern_UsesGlobStar()
    {
        var root = CreateTempDir();
        try
        {
            var d1 = Path.Combine(root, "entities");
            var d2 = Path.Combine(root, "entities", "nested");
            var f1 = Path.Combine(d1, "top.entity.json");
            var f2 = Path.Combine(d2, "deep.entity.json");
            var f3 = Path.Combine(root, "entities.json"); // should not match

            Touch(f1); Touch(f2); Touch(f3);

            var matches = FileHelper.MatchGlobFiles(root, "entities/**/*.entity.json").ToArray();
            var normalizedMatches = matches.Select(Norm).ToArray();

            Assert.Equal(2, normalizedMatches.Length);
            Assert.Contains(Norm(f1), normalizedMatches, StringComparer.OrdinalIgnoreCase);
            Assert.Contains(Norm(f2), normalizedMatches, StringComparer.OrdinalIgnoreCase);
            Assert.DoesNotContain(Norm(f3), normalizedMatches, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void MatchGlobFiles_NoMatches_ReturnsEmpty()
    {
        var root = CreateTempDir();
        try
        {
            Touch(Path.Combine(root, "a.txt"));
            Touch(Path.Combine(root, "b.json"));

            var matches = FileHelper.MatchGlobFiles(root, "*.entity.json").ToArray();
            var normalizedMatches = matches.Select(Norm).ToArray();

            Assert.Empty(matches);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void MatchGlobFiles_NonExistingBaseDir_ReturnsEmpty()
    {
        var root = CreateTempDir();
        var missing = Path.Combine(root, "does-not-exist");
        try
        {
            var matches = FileHelper.MatchGlobFiles(missing, "*.json").ToArray();
            Assert.Empty(matches);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }

    [Fact]
    public void MatchGlobFiles_MultipleFilesSameNameDifferentFolders_AllReturned()
    {
        var root = CreateTempDir();
        try
        {
            var d1 = Path.Combine(root, "entities");
            var d2 = Path.Combine(root, "more", "entities");
            var f1 = Path.Combine(d1, "dup.entity.json");
            var f2 = Path.Combine(d2, "dup.entity.json");

            Touch(f1); Touch(f2);

            var matches = FileHelper.MatchGlobFiles(root, "**/dup.entity.json").ToArray();
            var normalizedMatches = matches.Select(Norm).ToArray();

            Assert.Equal(2, matches.Length);
            Assert.Contains(Norm(f1), normalizedMatches, StringComparer.OrdinalIgnoreCase);
            Assert.Contains(Norm(f2), normalizedMatches, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            try { Directory.Delete(root, true); } catch { }
        }
    }
}