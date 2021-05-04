namespace WebApplication.API.Realtime
{
    public sealed class ConnectionManager
    {
        public ConnectionMapping<string> Connections { get; }
            = new ConnectionMapping<string>();

        private static ConnectionManager instance = null;
        private static readonly object padlock = new object();

        ConnectionManager()
        {
        }

        public static ConnectionManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ConnectionManager();
                    }
                    return instance;
                }
            }
        }
    }
}