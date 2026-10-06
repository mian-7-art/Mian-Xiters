using System;
using System.Reflection;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            var asm = Assembly.LoadFile(@"c:\Users\nafee\Desktop\WpfApp1\packages\AuthlyX.2.2.0\lib\net48\AuthlyX.dll");
            foreach (var t in asm.GetTypes())
            {
                Console.WriteLine("TYPE: " + t.FullName);
                foreach (var m in t.GetMethods())
                {
                    if (m.IsPublic) Console.WriteLine("  METHOD: " + m.Name);
                }
            }
        }
    }
}
