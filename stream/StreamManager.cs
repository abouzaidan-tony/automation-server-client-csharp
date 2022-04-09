using System;
using AutomationServerClient;

namespace AutomationServerClient.Stream {

    public abstract class StreamManager : OnDataReceivedListener
    {
        protected OnMessageReadyListener messageReadyListener;

        public void setMessageReadyListener(OnMessageReadyListener listener){
            messageReadyListener = listener;
        }

        public abstract void OnDataReceived(byte[] buffer, int length);
        public abstract byte[] formatStream(byte[] msg);
    }
}