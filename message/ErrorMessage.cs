
using System;

namespace AutomationServerClient.Messages {

    public class ErrorMessage : Message {

        private string errorCode;
        private string message;

        internal ErrorMessage(string errorCode, string message, string client) : base(MessageType.ERROR, client) {
            this.errorCode = errorCode;
            this.message = message;
            init();
        }

        internal ErrorMessage(byte[] buffer) : base(buffer) {
            char[] c = new char[4];
            Array.Copy(buffer, 6, c, 0, 4);
            this.errorCode = new string(c);
            c = new char[buffer.Length - 10];
            Array.Copy(buffer, 10, c, 0, buffer.Length - 10);
            this.message = new string(c);
        }

        protected override string resoleOrigin()
        {
            char[] c = new char[5];
            Array.Copy(buffer, 1, c, 0, 5);
            return new string(c);
        }

        protected override void init() {
            int length = 10 + message.Length;
            buffer = new byte[length];
            buffer[0] = (byte)getMessageType();
            for (int i = 0; i < 5; i++)
                buffer[i + 1] = (byte) getOrigin()[i];
            for (int i = 0; i < 4; i++)
                buffer[i + 6] = (byte) errorCode[i];
            for (int i = 0; i < message.Length; i++)
                buffer[i + 10] = (byte) message[i];
        }

        public String getErrorCode() {
            return errorCode;
        }

        public String getMessage() {
            return message;
        }
    }

}