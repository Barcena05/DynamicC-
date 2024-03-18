using System.Dynamic;
using System.Reflection;
// Accediendo directamente a los atributos
var p1 = Factory.New.Person;
p1.FirstName = "Louis";
p1.LastName = "Dejardin";
System.Console.WriteLine(p1.FirstName);
System.Console.WriteLine(p1.LastName);

// Accediendo a los atributos en forma de diccionario
var p2 = Factory.New.Person;
p2["FirstName"] = "Louis";
p2["LastName"] = "Dejardin";
System.Console.WriteLine(p2["FirstName"]);
System.Console.WriteLine(p2["LastName"]);

// Inicializando mediante una "fluent interface"
var p3 = Factory.New.Person.FirstName("Louis").LastName("Dejardin");
System.Console.WriteLine(p3.FirstName);
System.Console.WriteLine(p3.LastName);

// Con notación similar a JSON
var p4 = Factory.New.Person(FirstName : "Louis", LastName : "Dejardin" );
System.Console.WriteLine(p4.FirstName);
System.Console.WriteLine(p4.LastName);

//Agregando parametros de forma dinamica
dynamic person = Factory.New.Persona(new{
  FirstName = "Louis",
  LastName = "Dejardin",
  Manager = Factory.New.Persona(
    new{FirstName= "Bertrand",
    LastName= "Le Roy"}
  )
}
);
System.Console.WriteLine(person.Manager.FirstName);
System.Console.WriteLine(person.Manager.LastName);

//Creando instancias de distintos tipos conocidos en ejecucion:

//Creacion y casteo implicito a un tipo Person
Person p = DFactory.New.Person();
//Acceso a propiedades
p.FirstName = "Pepe";
//Acceso a metodos extensores
p.LastName("Antonio");
System.Console.WriteLine(p.FirstName);
System.Console.WriteLine(p.LastName);

//Creacion de un objeto de tipo person en tiempo de ejecucion con notacion de fluent interfaces(Mediante metodos extensores)
dynamic g = DFactory.New.Person().FirstName("Pepe").LastName("Antonio");
System.Console.WriteLine(g.FirstName);
System.Console.WriteLine(g.LastName);

Cuenta c = DFactory.New.Cuenta("Ernesto", 5);
System.Console.WriteLine(c.propietario);
System.Console.WriteLine(c.saldo);
c.Saluda();
System.Console.WriteLine(c.GetType()); // imprime el tipo de c
c.SaludaAgain();



