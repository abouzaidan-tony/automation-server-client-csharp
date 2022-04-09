using System.Text;

namespace AutomationServerClient.Messages
{

    public class EchoMessageBuilder : MessageBuilder {

        private byte[] message;

        public EchoMessageBuilder setMessage(byte[] message) {
            this.message = message;
            return this;
        }

        public EchoMessageBuilder setMessage(string message) {
            this.message = Encoding.ASCII.GetBytes(message);
            return this;
        }

        public override Message build() {
            Message m = new EchoMessage(message);
            return m;
        }

    }
}