using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Common.Validators;

namespace Nexus.DomainStudio.Tests.Domain.Validators;

public class EmailValidatorTests
{
    /// <summary>
    /// Tests that AgainstInvalidInput throws ArgumentException for invalid email formats.
    /// </summary> 
    [Fact]
    public void AgainstIsEmail_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        string email = "testhotmail.com";
        //act
        var ex = Assert.Throws<ArgumentException>(() =>
            Guard.AgainstInvalidInput(
                email,
                nameof(email),
                e => EmailValidator.IsValid(e)
            )
        );
        // Assert
        Assert.Contains(nameof(email), ex.Message);
        Assert.Contains("testhotmail.com", ex.Message);
    }
}