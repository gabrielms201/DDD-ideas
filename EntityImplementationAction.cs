using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace Playground;



public class ForumCreated : IEvent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ForumCreated(string name) => Name = name;
}
public abstract class Entity

{
    private Dictionary<Type, Action<IEvent>> _events = new();
    public Entity()
    {
        SetEvents();
    }

    protected void Apply(IEvent e)
    {
        if (_events.TryGetValue(e.GetType(), out var action))
            action(e);
    }
    protected void SetEvent<TEvent>(Action<TEvent> handler) where TEvent : IEvent
        => _events.TryAdd(typeof(TEvent), e => handler((TEvent)e));

    protected abstract void SetEvents();
}
class Forum : Entity
{
    public int? Id;
    public string? Name;
    private void WhenForumCreated(ForumCreated e)
    {
        Console.WriteLine("Forum created");
        Id = e.Id;
        Name = e.Name;
    }

    protected override void SetEvents()
    {
        SetEvent<ForumCreated>(WhenForumCreated);
    }

    public Forum() : base() => Apply(new ForumCreated("Welcome to my forum"));
}
class Program
{

    static void Main()
    {
        Forum e = new();
        Console.WriteLine("Forum Name: " + e.Name);
    }

}
