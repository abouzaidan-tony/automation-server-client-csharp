namespace AutomationServerClient.Stream {

    public interface OnMessageReadyListener
    {
        void OnMessageReady(byte[] message);
    }
}