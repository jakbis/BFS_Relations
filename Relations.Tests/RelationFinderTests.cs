using Relations;
using Xunit;

namespace Relations.Tests;

public sealed class RelationFinderTests
{
    private readonly Person[] _data =
    {
        new() { FullName = new("Grace","Hopper"), Address = new("Main","NY") },
        new() { FullName = new("Grace","Hopper"), Address = new("High","BP") },
        new() { FullName = new("Alan","Turing"),  Address = new("High","BP") },
        new() { FullName = new("Alan","Turing"),  Address = new("Broad","OX") }
    };

    [Fact]
    public void Level_Is_Two_For_Grace_To_Alan() 
    {
        var rf = new RelationFinder();
        rf.Init(_data);
        Assert.Equal(2, rf.FindMinRelationLevel(_data[0], _data[2]));
    }

    [Fact]
    public void Level_Is_Zero_For_Same_Record()
    {
        var rf = new RelationFinder();
        rf.Init(_data);
        Assert.Equal(0, rf.FindMinRelationLevel(_data[1], _data[1]));
    }

    [Fact]
    public void Level_Is_MinusOne_When_Disconnected()
    {
        var rf = new RelationFinder();
        rf.Init(_data);
        var solo = new Person { FullName = new("Ada", "Lovelace"), Address = new("Park", "LDN") };
        Assert.Equal(-1, rf.FindMinRelationLevel(_data[0], solo));
    }
}
