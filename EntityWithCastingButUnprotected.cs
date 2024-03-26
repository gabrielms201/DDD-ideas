namespace Playground;


public record ForumChanged(string NewName) : IEvent
{
    public string Id { get; } = Guid.NewGuid().ToString();
}

public record ForumCreated(string Name) : IEvent
{
    public string Id { get; } = Guid.NewGuid().ToString();
}

public abstract class Entity

{
    public Entity()
    {

    }

    protected void Apply<TEvent>(TEvent e) where TEvent : IEvent
        => When<TEvent>(e);

    private void When<TEvent>(TEvent e) where TEvent : IEvent
        => (this as dynamic).When(e);
}
class Forum : Entity
{
    public string? Name;
    // Problema: Como mudar esse encapsulamento,
    //  e ainda assim chamar o metodo a partir de Entity 
    internal void When(ForumCreated e)
    {
        Console.WriteLine("Forum created");
        Name = e.Name;
    }
    internal void When(ForumChanged e)
    {
        Console.WriteLine("Forum changed");
        Name = e.NewName;
    }


    public void ChangeForum()
    {
        Apply(new ForumChanged("New Forum Name"));
    }

    public Forum()
        : base() =>
        Apply(new ForumCreated("Welcome to my forum"));

}
class Program
{

    static void Main()
    {
        Forum e = new();
        Console.WriteLine("Forum Name: " + e.Name);

        e.ChangeForum();
        Console.WriteLine("Forum Name: " + e.Name);

    }

}
