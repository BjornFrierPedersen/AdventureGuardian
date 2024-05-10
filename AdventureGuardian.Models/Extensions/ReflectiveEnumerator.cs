using System.Reflection;

namespace AdventureGuardian.Models.Extensions;

public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }
    
    public static IEnumerable<T> GetEnumerableOfType<T>() where T : class
    {
        return (from type in Assembly.GetAssembly(typeof(T))
                ?.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))) ?? throw new InvalidOperationException()
            select (T)Activator.CreateInstance(type)).ToList();
    }
}