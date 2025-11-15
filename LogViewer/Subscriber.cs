using System;
using System.Text;
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
        _channel.ExchangeDeclare(exchange: _exchangeName, type: _exchangeType);
        
        //crear una cola temporal 
        var queueName = _channel.QueueDeclare().QueueName;

        //enlazar la cola al exchange
        _channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: "");

        //crear el consumidor (instanciar EventingBasicConsumer)
        var consumer = new EventingBasicConsumer(_channel);

        //configurar el callback
        consumer.Received += (model, ea) =>  
        {  
        var body = ea.Body.ToArray(); //contenido del mensaje (array de bytes) 
        var message = Encoding.UTF8.GetString(body); //se convierte de vuelta a string 
        Console.WriteLine($"Nuevo mensaje: {message}");  
        };

        //iniciar el consumo (BasicConsume)
        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

        Console.ReadLine();

        _channel.Close();
        _connection.Close();

    }


}