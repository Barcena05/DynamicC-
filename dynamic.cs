using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
//Clase Persona, hereda de DynamicObject y sobreescribe los metodos tryGetMember y trySetMember.
//Posee un diccionario de atributos para poder insertar nuevos atributos dinamicamente
public class Persona : DynamicObject
{
    private Dictionary<string, object> atributos;

    public Persona()
    {
        atributos = new Dictionary<string, object>();
    }
    //Indexers para los campos dinamicos
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

    //Sobreescritura del metodo TrySetMember
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        string nombreAtributo = binder.Name;
        atributos[nombreAtributo] = value;
        return true;
    }
    //Sobreescritura del metodo TryGetMember
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

//Clase estatica DFactory, posee un campo New que devuelve una instancia de GBuilder
public static class DFactory
{
    public static dynamic New => new GBuilder();
}

//Clase GBuilder, hereda de DynamicObject y sobreescribe el metodo TryInvokeMember. El parametro de salida es un objeto de tipo DType
public class GBuilder : DynamicObject
{
    public GBuilder()
    {

    }
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = new DType(binder.Name, args);
        return true;
    }
}

//Clase DType, actua como un envoltorio para crear dinamicamente instancias de tipos conocidos en ejecucion
//Hereda de DynamicObject y hace uso de reflection para simular el comportamiento del tipo dinamico que se quiere obtener
public class DType : DynamicObject
{
    //Tipo de dato que se quiere simular
    private Type type;
    //Argumentos pasados al constructor de dicho tipo de datos
    private object[] properties;
    //TypeInfo del tipo de dato que se quiere simular
    private TypeInfo typeInfo;
    //Objeto dinamico que representara la instancia del tipo de datos que se quiere simular
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