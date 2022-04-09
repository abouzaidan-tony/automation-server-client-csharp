using System;

namespace AutomationServerClient.Messages {
    public abstract class Message {

        public enum MessageType : byte {
            DATA = 1, 
            ERROR = 2,
            ECHO = 3, BROADCAST = 4, EMPTY = 5
        }
           
        public static string originServer = "00000";

        private MessageType mType;
        protected byte[] buffer;
        private string origin;

        protected Message(MessageType mType, string destination){
            this.mType = mType;
            this.origin = destination;
        }

        protected Message(byte[] buffer) {
            this.mType = (MessageType)Enum.ToObject(typeof(MessageType), buffer[0]);
            this.buffer = buffer;
            this.origin = resoleOrigin();
        }

        protected abstract void init();

        protected abstract string resoleOrigin();

        public byte[] toByteArray(){
            if(buffer == null)
                init();
            return buffer;
        }

        public MessageType getMessageType() {
            return mType;
        }

        public string getOrigin() {
            return origin;
        }
        public void setOrigin(string origin) {
            this.origin = origin;
            init();
        }
    }

}