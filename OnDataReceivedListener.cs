namespace AutomationServerClient {

    public interface OnDataReceivedListener
    {
        void OnDataReceived(byte[] buffer, int length);
    }
}