//Clase estatica que posee metodos extensores para varios de los tipos definidos
public static class Extensors
{
    //Metodo extensor de la clase Person que setea el campo FirstName al valor de entrada y devuelve la instancia de Person modificada
    public static Person FirstName(this Person person, string value)
    {
        person.FirstName = value;
        return person;
    }
    //Metodo extensor de la clase Person que setea el campo LastName al valor de entrada y devuelve la instancia de Person modificada
    public static Person LastName(this Person person, string value)
    {
        person.LastName = value;
        return person;
    }
    //Metodo extensor de la clase Builder que devuelve una nueva instancia de Person con los valores de entrada correspondientes
    public static Person Person(this Builder builder, string FirstName, string LastName)
    {
        builder.Person.FirstName = FirstName;
        builder.Person.LastName = LastName;
        return builder.Person;
    }
    //Metodo extensor de la clase Builder que devuelve una nueva instancia de Persona con los valores de entrada correspondientes
    public static Persona Persona(this Builder builder, object properties)
    {
        var props = properties.GetType().GetProperties();
        foreach (var prop in props)
        {
            var value = prop.GetValue(properties, null);
            builder.Persona[prop.Name] = value;
        }
        return builder.Persona;
    }
    //Metodo extensor de la clase Cuenta que imprime un saludo en pantalla
    public static void SaludaAgain(this Cuenta cuenta)
    {
        System.Console.WriteLine("Hola de nuevo");
    }
}
