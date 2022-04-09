using System.IO;
using System;

namespace AutomationServerClient.Stream {

    public class BytesStreamManager : StreamManager
    {
        public enum STATE
        {
            LENGTH1, LENGTH2, LENGTH3, LENGTH4, DATA
        };

        private STATE state;
        private int length;
        private int remaining;
        private MemoryStream ms;
        private BinaryWriter writer;


        public BytesStreamManager()
        {
            state = STATE.LENGTH1;
            length = 0;
            remaining = 0;
        }

        public override void OnDataReceived(byte[] buffer, int len)
        {
            int offset = 0;
            while (offset < len)
            {
                switch (state)
                {
                    case STATE.LENGTH1:
                        length = (int)((buffer[offset] << 24) & 0xFF000000);
                        offset++;
                        state = STATE.LENGTH2;
                        break;
                    case STATE.LENGTH2:
                        length |= (buffer[offset] << 16) & 0x00FF0000;
                        state = STATE.LENGTH3;
                        offset++;
                        break;
                    case STATE.LENGTH3:
                        length |= (buffer[offset] << 8) & 0x0000FF00;
                        state = STATE.LENGTH4;
                        offset++;
                        break;
                    case STATE.LENGTH4:
                        length |= buffer[offset] & 0x000000FF;
                        state = STATE.DATA;
                        remaining = length;
                        ms = new MemoryStream();
                        writer = new BinaryWriter(ms);
                        offset++;
                        if (remaining == 0)
                        {
                            state = STATE.LENGTH1;
                            ms.Close();
                        }
                        break;
                    case STATE.DATA:
                        int min = remaining < len - offset ? remaining : len - offset;
                        writer.Write(buffer, offset, min);
                        remaining -= min;
                        offset += min;

                        if (remaining <= 0)
                        {
                            state = STATE.LENGTH1;
                            messageReadyListener?.OnMessageReady(ms.ToArray());
                            ms.Close();
                        }
                        break;
                }
            }
        }

        public override byte[] formatStream(byte[] msg){
            byte[] r = new byte[msg.Length + 4];
            int length = msg.Length;
            r[0] = (byte)((length & 0xFF000000) >> 24);
            r[1] = (byte)((length & 0x00FF0000) >> 16);
            r[2] = (byte)((length & 0x0000FF00) >> 8);
            r[3] = (byte)((length & 0x000000FF));
            Array.Copy(msg, 0, r, 4, msg.Length);
            return r;
        }

        public static byte[] ChartoByte(char[] charArray)
        {
            byte[] byteArray = new byte[charArray.Length];
            for (int i = 0; i < charArray.Length; i++)
                byteArray[i] = (byte)charArray[i];
            return byteArray;
        }

    }

}