using System;
using LogViewer;

namespace LogViewer;

class Program
{
    static void Main(string[] args)
    {
        string routingKey;

        if (args.Length == 0)
        {
            Console.Write("Introduce el topic (ej: error.*, info.* o #): ");
            routingKey = Console.ReadLine()!;
        }
        else
        {
            routingKey = args[0];
        }

        var subscriber = new Subscriber();
        subscriber.StartConsuming(routingKey);

    }
}
