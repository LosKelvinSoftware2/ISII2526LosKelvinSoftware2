using System;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogViewer;

class Subscriber{

    
    //Parametros de configuracion
    private readonly string _hostname = "localhost"; //cambiar por la dirección que corresponda
    private readonly string _exchangeName = "logs"; //nombre del exchange que usa la API -------------------- object
    private readonly string _exchangeType = "fanout"; //tipo de exchange usado
    private readonly string _userName = "guest"; //utilizar las credenciales de un usuario de RabbitMQ
    private readonly string _password = "guest";
    private readonly int _port = 5672; //reemplazar por el puerto AMQP de RabbitMQ

    private readonly IConnection _connection;
    private readonly IModel _channel;
    //private readonly IBasicProperties _properties;
    

    // Definir una clase para deserializar el log
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? LogLevel { get; set; }
        public string? Category { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
    }
    
    public Subscriber()
    {
        //usamos una ConnectionFactory para definir los parametros de conexion 
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            UserName = _userName,
            Password = _password,
            Port = _port
        };

        //creamos la conexion con RabbitMQ
        _connection = factory.CreateConnection();
        //creamos un canal de comunicacion
        _channel = _connection.CreateModel();
        //no hace falta, solo recibe mensajes;ni los crea ni los publica
        //_properties = _channel.CreateBasicProperties();
        //_properties.Persistent = true; // Hace el mensaje persistente

    }


    public void StartConsuming()
    {

        //declarar el exchange de tipo fanout (igual que el usado en la API)
        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, durable: true);
        
        //crear una cola temporal 
        var queueName = _channel.QueueDeclare().QueueName;

        //enlazar la cola al exchange
        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

        //crear el consumidor (instanciar EventingBasicConsumer)
        var consumer = new EventingBasicConsumer(_channel);

        //configurar el callback
        consumer.Received += (model, ea) =>  
        { 
        var body = ea.Body.ToArray();
        var jsonString = Encoding.UTF8.GetString(body);
                
        // Deserializar el JSON a un objeto LogEntry
        var logEntry = JsonSerializer.Deserialize<LogEntry>(jsonString);
                
        if (logEntry != null)
            {
                // Mostrar la información de forma estructurada
                Console.WriteLine("=== NUEVO LOG ===");
                Console.WriteLine($"Timestamp: {logEntry.Timestamp:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"Level: {logEntry.LogLevel}");
                Console.WriteLine($"Category: {logEntry.Category}");
                Console.WriteLine($"EventId: {logEntry.EventId}");
                Console.WriteLine($"EventName: {logEntry.EventName}");
                Console.WriteLine($"Message: {logEntry.Message}");
                    
                if (!string.IsNullOrEmpty(logEntry.Exception))
                {
                    Console.WriteLine($"Exception: {logEntry.Exception}");
                }
                    
                Console.WriteLine("=================");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error: No se pudo deserializar el mensaje");
                Console.WriteLine($"Contenido crudo: {jsonString}");
            } 
        };

        //iniciar el consumo (BasicConsume)
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.ReadLine();

        _channel.Close();
        _connection.Close();

    }


}