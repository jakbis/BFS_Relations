using Relations;

var people = new[]
{
    new Person { FullName = new("Grace","Hopper"), Address = new("Main","NY") },
    new Person { FullName = new("Grace","Hopper"), Address = new("High","BP") },
    new Person { FullName = new("Alan","Turing"),  Address = new("High","BP") },
    new Person { FullName = new("Alan","Turing"),  Address = new("Broad","OX") }
};

var rf = new RelationFinder();
rf.Init(people);

Console.WriteLine(rf.FindMinRelationLevel(people[0], people[2])); // prints 2
Console.WriteLine(rf.FindMinRelationLevel(people[0], people[3])); //prints 3

