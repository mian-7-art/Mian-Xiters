using System;
using AuthlyX;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new Auth(
                ownerId: "ff49ba675be9",
                appName: "External_Cheat",
                version: "1.0.0",
                secret:  "gYwM0dXIZbpWkTx8MIgLBM4t8mEb70Swk05GiYsD",
                debug:   true
            );

            Console.WriteLine("Initializing AuthlyX...");
            app.Init();
            Console.WriteLine("Success: " + app.response.success);
            Console.WriteLine("Message: " + app.response.message);
        }
    }
}
