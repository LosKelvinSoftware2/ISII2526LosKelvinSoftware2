using System;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using System.Globalization;

namespace LogViewer;

class Subscriber
{
    private readonly string _hostname = "localhost"; //cambiar por la dirección que corresponda 
    private readonly string _queueName = "logs"; 
    private readonly string _userName = "guest"; //utilizar las credenciales de un usuario de RabbitMQ 
    private readonly string _password = "guest"; 
    private readonly int _port = 5672; //reemplazar por el puerto AMQP de RabbitMQ 
        
    private readonly IConnection _connection; 
    private readonly IModel _channel; 
    private readonly IBasicProperties _properties;

    
        public Subscriber() {
            var factory = new ConnectionFactory() 
            {  
                HostName = _hostname, 
                UserName = _userName, 
                Password = _password, 
                Port = _port  
        
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel(); 
            _properties = _channel.CreateBasicProperties(); 
            _properties.Persistent = true; // Hace el mensaje persistente 
            _channel.QueueDeclare( 
                queue: _queueName, 
                durable: true, // La cola sobrevive a reinicios del servidor 
                exclusive: false, // La cola puede ser usada por múltiples conexiones 
                autoDelete: false, // La cola no se elimina automáticamente 
                arguments: null  
            );
            _channel.ExchangeDeclare(_exchangeName, ExchangeType.fanout); 
            var queueName = tempQueue.QueueName;
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: ""); 
        }

    public void StartConsuming() {


        var subscriber = new EventingBasicConsumer(_channel);

        subscriber.Received += async (model, ea) =>  
        {  
            var body = ea.Body.ToArray(); //contenido del mensaje (array de bytes) 
            var message = Encoding.UTF8.GetString(body); //se convierte de vuelta a string 
            var log = JsonSerializer.Deserialize(message); 
            Console.WriteLine($"Log recibido: {log}");  
        
        };

        _channel.BasicConsume(            
            queue: _queueName,
            autoAck: true,
            consumer: consumer
        );
    }
    public void DisposeResources()
    {
        _channel?.Close();
        _connection?.Close();
    }
    static void Main(string[] args)
    {
        
    }
}
