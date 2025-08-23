using Nexus.DomainStudio.Domain.Common;
using Nexus.DomainStudio.Domain.Common.Events;

namespace Nexus.DomainStudio.Tests.Domain.Common;

/// <summary>
/// Represents a test domain event for unit testing purposes.
/// </summary>
public sealed class TestDomainEvent : IDomainEvent
{
    public string Name { get; init; }

    public TestDomainEvent(string name)
    {
        Name = name;
    }

    public DateTime OccuredOn { get; init; }

    public string? CorrelationId { get; init;}

    public Guid EventId { get; init;}
}

/// <summary>
/// Represents a test entity for unit testing purposes.
/// </summary>
public sealed class TestEntity : Entity<Guid>
{
    public string Name { get; set; }

    public TestEntity(Guid id, string name = "TestEntity") : base(id)
    {
        Name = name;
    }
}

/// <summary>
/// Unit tests for the Entity class.
/// </summary>
public class EntityTests
{
    /// <summary>
    /// Tests the constructor of the TestEntity class to ensure it initializes the Id property correctly.
    /// </summary>
    [Fact]
    public void Constructor_ShouldInitializeId_WhenCalledWithValidId()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity(id, "Test Entity");

        // Act
        var isEqual = entity.Id.Equals(id);

        // Assert
        Assert.True(isEqual);
    }

    /// <summary>
    /// Tests the ToString method of the TestEntity class to ensure it returns the correct format.
    /// </summary>
    [Fact]
    public void ToString_ShouldReturnCorrectFormat_WhenCalled()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity(id, "Test Entity");

        // Act
        var result = entity.ToString();

        // Assert
        Assert.Equal($"TestEntity [Id: {id}]", result);
    }

    /// <summary>
    /// Tests the Equals method of the TestEntity class to ensure it returns true when comparing entities with the same Id.
    /// </summary>
    [Fact]
    public void Equals_Should_Return_True_For_Same_Id()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id, "Entity 1");
        var entity2 = new TestEntity(id, "Entity 2");

        // Act
        var areEqual = entity1.Equals(entity2);

        // Assert
        Assert.True(areEqual);
    }

    /// <summary>
    /// Tests the Equals method of the TestEntity class to ensure it returns false when comparing entities with different Ids.
    /// </summary>
    [Fact]
    public void Equals_Should_Return_False_Different_Ids()
    {
        // Arrange
        var entity1 = new TestEntity(Guid.NewGuid(), "Entity 1");
        var entity2 = new TestEntity(Guid.NewGuid(), "Entity 2");

        // Act
        var areEqual = entity1.Equals(entity2);

        // Assert
        Assert.False(areEqual);
    }

    /// <summary>
    /// Tests the GetHashCode method of the TestEntity class to ensure it returns the same hash code for entities with the same Id.
    /// </summary>
    [Fact]
    public void GetHashCode_Should_Be_Based_On_Id()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity1 = new TestEntity(id);
        var entity2 = new TestEntity(id);

        // Act
        var areEqual = entity1.GetHashCode() == entity2.GetHashCode();

        // Assert
        Assert.True(areEqual);
    }

    /// <summary>
    /// Tests the AddDomainEvent method of the TestEntity class to ensure it correctly adds a domain event to the entity's domain events collection.
    /// </summary>
    [Fact]
    public void AddDomainEvent_Should_Store_Event()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        var domainEvent = new TestDomainEvent("test");

        // Act
        entity.RaiseDomainEvent(domainEvent);

        // Assert
        Assert.Single(entity.DomainEvents);
        Assert.Contains(domainEvent, entity.DomainEvents);
    }

    /// <summary>
    /// Tests the ClearDomainEvents method of the TestEntity class to ensure it removes all domain events.
    /// </summary>
    [Fact]
    public void ClearDomainEvents_Should_Remove_All_Events()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        entity.RaiseDomainEvent(new TestDomainEvent("test1"));
        entity.RaiseDomainEvent(new TestDomainEvent("test2"));

        // Act
        entity.ClearDomainEvents();

        // Assert
        Assert.Empty(entity.DomainEvents);
    }

    /// <summary>
    /// Tests the HasEvents method of the TestEntity class to ensure it returns true when events are present.
    /// </summary>
    [Fact]
    public void HasEvents_Should_Return_True_If_Events_Exist()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());
        var @event = new TestDomainEvent("TestEvent");

        // Act
        entity.RaiseDomainEvent(@event);

        // Assert
        Assert.True(entity.HasEvents());
    }

    /// <summary>
    /// Tests the HasEvents method of the TestEntity class to ensure it returns false when no events are present.
    /// </summary>
    [Fact]
    public void HasEvents_Should_Return_False_If_No_Events()
    {
        // Arrange
        var entity = new TestEntity(Guid.NewGuid());

        // Act
        bool hasEvents = entity.HasEvents();

        // Assert
        Assert.False(hasEvents);
    }

    /// <summary>
    /// Tests the ToString method of the TestEntity class to ensure it returns the correct format.
    /// </summary>
    [Fact]
    public void ToString_Should_Return_Correct_Format()
    {
        // Arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity(id);

        // Act
        var result = entity.ToString();

        // Assert
        Assert.Contains("TestEntity", result);
        Assert.Contains(id.ToString(), result);
    }
}