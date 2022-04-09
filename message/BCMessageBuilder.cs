using System.Text;

namespace AutomationServerClient.Messages
{

    public class BCMessageBuilder : MessageBuilder {

        private byte[] message;

        public BCMessageBuilder setMessage(byte[] message) {
            this.message = message;
            return this;
        }

        public BCMessageBuilder setMessage(string message) {
            this.message = Encoding.ASCII.GetBytes(message);
            return this;
        }

        public override Message build() {
            Message m = new BCMessage(message);
            return m;
        }

    }
}