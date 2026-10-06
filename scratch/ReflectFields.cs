using System;
using System.Reflection;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            var asm = Assembly.LoadFile(@"c:\Users\nafee\Desktop\WpfApp1\packages\AuthlyX.2.2.0\lib\net48\AuthlyX.dll");
            var authType = asm.GetType("AuthlyX.Auth");
            foreach (var f in authType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                Console.WriteLine("FIELD: " + f.Name + " (" + f.FieldType + ")");
            }
            foreach (var p in authType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                Console.WriteLine("PROP: " + p.Name + " (" + p.PropertyType + ")");
            }
        }
    }
}
