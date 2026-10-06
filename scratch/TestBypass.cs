using System;
using System.Reflection;
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

            // Bypass strict signature verification field if Authly backend is currently un-signed
            var field = typeof(Auth).GetField("requireSignedResponses", BindingFlags.Instance | BindingFlags.NonPublic);
            if (field != null)
            {
                field.SetValue(app, false);
                Console.WriteLine("requireSignedResponses set to FALSE");
            }

            Console.WriteLine("Initializing AuthlyX...");
            app.Init();
            Console.WriteLine("Init Success: " + app.response.success);
            Console.WriteLine("Init Message: " + app.response.message);

            if (app.response.success)
            {
                app.Login("1nOnlyMian"); // test login
                Console.WriteLine("Login Success: " + app.response.success);
                Console.WriteLine("Login Message: " + app.response.message);
            }
        }
    }
}
