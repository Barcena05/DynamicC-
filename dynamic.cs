using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        if (atributos.ContainsKey(binder.Name)) result = atributos[binder.Name];
        else
        {
            System.Console.WriteLine("El atributo {0} no existe", binder.Name);
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

public class GBuilder : DynamicObject
{
    public GBuilder()
    {

    }
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        var type = Type.GetType(binder.Name);
        System.Console.WriteLine(type);
        System.Console.WriteLine(binder.ReturnType);
        result = new DType(binder.Name, args);
        return true;
    }
}

public class DType : DynamicObject
{
    private Type type;
    private object[] properties;
    private TypeInfo typeInfo;
    private dynamic obj;
    public DType(string type, object[]? properties)
    {
        this.type = Type.GetType(type);
        this.properties = properties;
        typeInfo = this.type.GetTypeInfo();
        obj = Activator.CreateInstance(Type.GetType(type), properties);
    }
    public override bool TryConvert(ConvertBinder binder, out object? result)
    {
        result = Convert.ChangeType(obj, binder.Type);
        return true;
    }
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        if (typeInfo.GetDeclaredProperty(binder.Name) != null)
        {
            result = typeInfo.GetDeclaredProperty(binder.Name).GetValue(obj);
            return true;
        }
        result = null;
        return false;
    }
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (typeInfo.GetDeclaredProperty(binder.Name) != null)
        {
            typeInfo.GetDeclaredProperty(binder.Name).SetValue(obj, value);
            return true;
        }
        return false;
    }
    public dynamic this[string name]
    {
        get
        {
            return typeInfo.GetDeclaredProperty(name).GetValue(obj);
        }
        set
        {
            typeInfo.GetDeclaredProperty(name).SetValue(obj, value);
        }
    }
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        if (typeInfo.GetDeclaredMethod(binder.Name) != null)
        {
            result = typeInfo.GetDeclaredMethod(binder.Name).Invoke(obj, args);
            return true;
        }
        else
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
            var staticMethods = allTypes.SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(m => m.GetParameters().FirstOrDefault()?.ParameterType == this.type)
                .Where(m => m.IsDefined(typeof(ExtensionAttribute), false));
            var extencionMethod = staticMethods.FirstOrDefault(m => m.Name == binder.Name);
            if (extencionMethod != null)
            {
                dynamic answ = extencionMethod.Invoke(null, new[] { obj }.Concat(args).ToArray());
                if (answ.GetType() == this.type)
                {
                    this.obj = answ;
                    result = this;
                }
                else result = answ;
                return true;
            }
            result = null;
            return false;
        }
    }
}