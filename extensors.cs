public static class Extensors
{
    public static Person FirstName(this Person person, string value)
    {
        person.FirstName = value;
        return person;
    }
    public static Person LastName(this Person person, string value)
    {
        person.LastName = value;
        return person;
    }
    public static Person Person(this Builder builder, string FirstName, string LastName)
    {
        builder.Person.FirstName = FirstName;
        builder.Person.LastName = LastName;
        return builder.Person;
    }
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
    public static void SaludaAgain(this Cuenta cuenta)
    {
        System.Console.WriteLine("Hola de nuevo");
    }
}
