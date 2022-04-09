namespace AutomationServerClient.Messages
{

    public class EchoMessage : GenericDataMessage {

        public EchoMessage(byte[] data) : base(MessageType.ECHO, Message.originServer, data) {
        }
    }

}