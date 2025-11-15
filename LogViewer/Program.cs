using System;
using LogViewer;

namespace LogViewer;

class Program
{
    static void Main(string[] args)
    {
        var subscriber = new Subscriber();
        subscriber.StartConsuming();
    }
}
