namespace Demo.AzureFunctions.Models.Factories
{
    public class MessageFactory
    {
        public static Message CreateInvalidCommentMessage()
        {
            return new Message { Error = "Request is not a valid Comment." };
        }
    }
}
