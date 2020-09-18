namespace _Project.Scripts.DomainObjects.Configurations
{
    public struct InTrainingConfiguration
    {
        public WebSocketConfiguration webSocket;
        public int maxNumberOfPeople;

        public InTrainingConfiguration(WebSocketConfiguration webSocket = new WebSocketConfiguration(),
            int maxNumberOfPeople = 2)
        {
            this.webSocket = webSocket;
            this.maxNumberOfPeople = maxNumberOfPeople;
        }
    }
}