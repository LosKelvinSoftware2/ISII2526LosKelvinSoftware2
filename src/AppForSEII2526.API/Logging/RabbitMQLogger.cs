using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.Logging;

public class RabbitMQLogger : ILogger, IDisposable
{
    private readonly string _name;
    private readonly RabbitMQLoggerConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel; // RabbitMQ channel 7.0.2
    private readonly IBasicProperties _properties;

    public RabbitMQLogger(string name, RabbitMQLoggerConfiguration config)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _config = config ?? throw new ArgumentNullException(nameof(config));


        //Factoria AÑADIDO
        var factory = new ConnectionFactory
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
             exchange: _config.Exchange,
             type: _config.ExchangeType,
             durable: _config.Durable);

        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = true;
        _properties.ContentType = "application/json";

        ValidateConfiguration(_config); 
    }


    

    private static void ValidateConfiguration(RabbitMQLoggerConfiguration config)
    {
        if (string.IsNullOrEmpty(config.HostName))
            throw new ArgumentException("RabbitMQ HostName is required", nameof(config));
        if (config.Port <= 0)
            throw new ArgumentException("RabbitMQ Port must be greater than 0", nameof(config));
        if (string.IsNullOrEmpty(config.UserName))
            throw new ArgumentException("RabbitMQ UserName is required", nameof(config));
        if (string.IsNullOrEmpty(config.Password))
            throw new ArgumentException("RabbitMQ Password is required", nameof(config));
        if (string.IsNullOrEmpty(config.Exchange))
            throw new ArgumentException("RabbitMQ Exchange is required", nameof(config));
        if (string.IsNullOrEmpty(config.ExchangeType))
            throw new ArgumentException("RabbitMQ ExchangeType is required", nameof(config));
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        try
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow,
                LogLevel = logLevel.ToString(),
                Category = _name,
                EventId = eventId.Id,
                EventName = eventId.Name,
                Message = formatter(state, exception),
                Exception = exception?.ToString()
            };
            
            // Serializamos a JSON y convertimos a bytes
            string json = JsonSerializer.Serialize(logEntry);
            byte[] body = Encoding.UTF8.GetBytes(json);


            // Generar routing key según nivel de log
            string routingKey = logLevel switch
            {
                LogLevel.Information => "log.info",
                LogLevel.Warning => "log.warn",
                LogLevel.Error => "log.error",
                LogLevel.Critical => "log.critical",
                LogLevel.Debug => "log.debug",
                _ => "log.other"
            };


            _channel.BasicPublish(
                 exchange: _config.Exchange,
                 routingKey: routingKey,
                 basicProperties: _properties,
                 body: body);

        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error publishing log message to RabbitMQ: {ex.Message}");
        }
    }

    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error disposing RabbitMQ logger: {ex.Message}");
        }
        GC.SuppressFinalize(this);
    }
}