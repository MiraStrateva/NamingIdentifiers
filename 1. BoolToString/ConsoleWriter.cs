namespace ConsoleWriter
{
    using System;

    public class ConsoleWriter
    {
        public void Write(bool valueToWrite)
        {
            string stringValueToWrite = valueToWrite.ToString();
            Console.WriteLine(stringValueToWrite);
        }
    }
}
