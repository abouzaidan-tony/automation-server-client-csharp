using System.Text;

namespace AutomationServerClient.Messages
{

    public class DataMessageBuilder : MessageBuilder {

        private byte[] message;

        public DataMessageBuilder setMessage(byte[] message) {
            this.message = message;
            return this;
        }

        public DataMessageBuilder setMessage(string message) {
            this.message = Encoding.ASCII.GetBytes(message);
            return this;
        }

        public override Message build() {
            Message m = new DataMessage(message, getOrigin());
            return m;
        }

    }
}