
using AutomationServerClient.Stream;

using AutomationServerClient.Messages;

namespace AutomationServerClient {

    public class ClientSession : Session
    {
        public delegate void MessageReceivedDelegate(Message message);
        public event MessageReceivedDelegate onMessageReady;

        public ClientSession(string API_KEY, string APP_KEY, string deviceId) : base(API_KEY, APP_KEY, deviceId)
        {}
        
        public override void OnMessageReady(byte[] message)
        {
            BufferMessageBuilder builder = new BufferMessageBuilder();
            builder.setBuffer(message);
            Message m = builder.build();
            onMessageReady?.Invoke(m);
        }
        
        public override void Authenticate()
        {
            byte[] key = BytesStreamManager.ChartoByte(("U"+API_KEY+APP_KEY+deviceId).ToCharArray());
            sendMessage(key);
        }
    }
}