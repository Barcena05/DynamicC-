using System.Dynamic;
using System.Reflection;
public class Persona : DynamicObject
{
    private Dictionary<string, object> atributos;

    public Persona()
    {
        atributos = new Dictionary<string, object>();
    }
    public object this[string key]
    {
        get
        {
            return atributos[key];
        }
        set
        {
            atributos[key] = value;
        }
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        string nombreAtributo = binder.Name;
        atributos[nombreAtributo] = value;
        return true;
    }
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if(atributos.ContainsKey(binder.Name))result = atributos[binder.Name];
        else{
            System.Console.WriteLine("El atributo {0} no existe",binder.Name);
            result = null;
        }
        return true;
    }
}

public static class DFactory
{
    // public static T New<T>()
    // {
    //     return Activator.CreateInstance<T>();
    // }

    // public static T New<T>(params object[] properties)
    // {
    //     return (T)Activator.CreateInstance(typeof(T), properties);
    // }
    public static dynamic New => new GBuilder();
}

public class GBuilder: DynamicObject
{
    public GBuilder()
    {
        
    }
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var type = Type.GetType(binder.Name);
        result = Activator.CreateInstance(type, args);
        return true;
    }
}
