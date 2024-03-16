public static class Factory
{
    public static Builder New => new Builder();
    
}
public class Builder
    {
        public Person Person = new Person();
        public Persona Persona = new Persona();

    }
public class Person
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string this[string key]
    {
        get
        {
            if (key == "FirstName")
            {
                return FirstName;
            }
            if (key == "LastName")
            {
                return LastName;
            }
            return null;
        }
        set
        {
            if (key == "FirstName")
            {
                FirstName = value;
            }
            if (key == "LastName")
            {
                LastName = value;
            }
        }
    }
}
public class Cuenta{
    public string propietario { get; set; }
    public int saldo { get; set; }
    public Cuenta(string propietario, int saldo){
        this.propietario = propietario;
        this.saldo = saldo;
    }
    public void Saluda()
    {
        System.Console.WriteLine("Hola");
    }
}