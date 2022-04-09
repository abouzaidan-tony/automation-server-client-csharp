using System;

namespace AutomationServerClient.Messages
{
    public class BufferMessageBuilder : MessageBuilder
    {

        private byte[] buffer;

        public BufferMessageBuilder()
        {

        }


        public override Message build()
        {
            Message.MessageType mType = (Message.MessageType)Enum.ToObject(typeof(Message.MessageType), buffer[0]);
            switch(mType){
                case Message.MessageType.DATA:
                case Message.MessageType.ECHO:
                case Message.MessageType.BROADCAST:
                return new GenericDataMessage(buffer);
                case Message.MessageType.ERROR:
                return new ErrorMessage(buffer);
            }
            return null;
        }

        public byte[] getBuffer()
        {
            return buffer;
        }

        public BufferMessageBuilder setBuffer(byte[] buffer)
        {
            this.buffer = buffer;
            return this;
        }
    }
}