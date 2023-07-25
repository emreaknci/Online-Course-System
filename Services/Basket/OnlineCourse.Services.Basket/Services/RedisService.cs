using StackExchange.Redis;

namespace OnlineCourse.Services.Basket.Services
{
    public class RedisService
    {
        private readonly string Host;

        private readonly int Port;
        private ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string host, int port)
        {
            Host = host;
            Port = port;
        }

        public void Connect() => _connectionMultiplexer = ConnectionMultiplexer.Connect($"{Host}:{Port}");

        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db);
    }
}
