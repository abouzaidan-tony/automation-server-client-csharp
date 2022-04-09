

namespace AutomationServerClient.Messages {

    public class BCMessage : GenericDataMessage
    {
        internal BCMessage(byte[] data) : base(MessageType.BROADCAST, Message.originServer, data)
        {
        }
    }
}

