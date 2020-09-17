namespace _Project.Scripts.DomainObjects.Configurations
{
    public struct WebSocketConfiguration
    {
        public string url;

        public WebSocketConfiguration(string url = "ws://localhost:8080/")
        {
            this.url = url;
        }
    }
}