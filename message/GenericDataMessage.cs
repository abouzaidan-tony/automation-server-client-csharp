using System;

namespace AutomationServerClient.Messages {

public class GenericDataMessage : Message {

        private byte[] data;

        internal GenericDataMessage(MessageType mType, string client, byte[] data) : base(mType, client)
        {
            this.data = data;
            setOrigin(client);
            init();
        }

        internal GenericDataMessage(byte[] buffer) : base(buffer) {
            data = new byte[buffer.Length - 6];
            Array.Copy(buffer, 6, data, 0, data.Length);
        }

        protected override string resoleOrigin() {
            char [] c = new char[5];
            Array.Copy(buffer, 1, c, 0, 5);
            return new string(c);
        }

        protected override void init() {
            int totalLength = 6 + data.Length;
            buffer = new byte[totalLength];
            buffer[0] = (byte) getMessageType();
            char[] idBuffer = getOrigin().ToCharArray();
            for (int i = 0; i < 5; i++)
                buffer[i + 1] = i < idBuffer.Length ? (byte)idBuffer[i] : (byte) 0;
            for (int i = 0; i < data.Length; i++)
                buffer[i + 6] = data[i];
        }

        public byte[] getData() {
            return data;
        }

        public void setData(byte[] data) {
            this.data = data;
        }
    }

}