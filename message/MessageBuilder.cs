
namespace AutomationServerClient.Messages
{
    public abstract class MessageBuilder {

        private string origin;

        public MessageBuilder() {

        }

        public abstract Message build();

        public string getOrigin() {
            return origin;
        }

        public MessageBuilder setOrigin(string origin) {
            this.origin = origin;
            return this;
        }
    }

}