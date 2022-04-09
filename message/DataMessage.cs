namespace AutomationServerClient.Messages
{
    public class DataMessage : GenericDataMessage {

        internal DataMessage(byte[] data, string destination) : base(MessageType.DATA, destination, data)
        {
        }
    }
}