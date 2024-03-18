//Tipos no dinamicos definidos


//Clase estatica Factory, posee un campo estatico New que devuelve un objeto de tipo Builder
public static class Factory
{
    public static Builder New => new Builder();
    
}
//Clase Builder, posee dos campos para devolver un objeto de tipo Person y otro de tipo Persona
public class Builder
    {
        public Person Person = new Person();
        public Persona Persona = new Persona();

    }
//Clase Person, tipo no dinamico que posee dos campos, FirstName y LastName con sus respectivas propiedades get y set
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
//Clase cuenta, posee dos campos (propietario y saldo) y un metodo void (Saluda)
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