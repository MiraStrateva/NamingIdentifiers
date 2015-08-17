namespace Person
{
    using System;

    public class Program
    {
        public static void Main()
        {
            Person newEmployee = new Person("Dragan Cankov", 43, Gender.MAN);
            Console.WriteLine(newEmployee.Name);
        }
    }
}
