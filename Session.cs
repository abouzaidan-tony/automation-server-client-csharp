using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using AutomationServerClient.Stream;
using AutomationServerClient.Messages;

namespace AutomationServerClient
{
    public abstract class Session : OnMessageReadyListener
    {

        public delegate void SessionConnectedDelegate();
        public event SessionConnectedDelegate OnConnected;
        public event SessionConnectedDelegate OnConnectionFailed;
        public event SessionConnectedDelegate OnAuthenticationFailed;
        public event SessionConnectedDelegate OnDisconnected;

        private static string server = "SOME IP ADDRESS";
        private static int port = 9909;
        private Thread sessionThread;
        protected Socket socket;
        protected string API_KEY;
        protected string APP_KEY;
        protected string deviceId;
        private volatile bool running;
        private StreamManager manager;
        private OnDataReceivedListener dataReceivedListener;

        public abstract void OnMessageReady(byte[] message);

        public Session(string API_KEY, string APP_KEY, string deviceId)
        {
            this.API_KEY = API_KEY;
            this.APP_KEY = APP_KEY;
            this.deviceId = deviceId;
            sessionThread = new Thread(run);
            manager = new BytesStreamManager();
            manager.setMessageReadyListener(this);
            dataReceivedListener = manager;
            running = false;
        }

        public void Connect()
        {
            sessionThread.Start();
        }

        public abstract void Authenticate();

        private void run()
        {
            int length = 0;
            byte[] authB = new byte[1];

            byte[] buffer = new byte[256];

            try
            {

                IPAddress address = Dns.GetHostAddresses(server)[0];
                IPEndPoint ipe = new IPEndPoint(address, port);
                socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipe);

                Console.WriteLine("Connected");

                running = true;

                Authenticate();

        
                socket.Receive(authB);

                if (authB[0] != 1){
                    OnAuthenticationFailed?.Invoke();
                    Console.WriteLine("Failed");
                    return;
                }
                    

                OnConnected?.Invoke();

            }
            catch (Exception ex)
            {
                running = false;
                Console.WriteLine(ex.Message);
                OnConnectionFailed?.Invoke();
                return;
            }

            

            try
            {
                while (running)
                {
                    length = socket.Receive(buffer);

                    if (length < 0)
                        break;

                    dataReceivedListener?.OnDataReceived(buffer, length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            close();
        }

        public void close()
        {
            try
            {
                socket.Close();
            }
            catch (Exception) { }
            finally
            {
                running = false;
                OnDisconnected?.Invoke();
            }

        }

        protected virtual void sendMessage(byte[] msg)
        {
            try
            {
                socket.Send(manager.formatStream(msg));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                running = false;
            }
        }

        public void sendMessage(Message message)
        {
            Console.WriteLine("sending...");
            sendMessage(message.toByteArray());
        }

        public bool IsRunning()
        {
            return running;
        }
    }
}