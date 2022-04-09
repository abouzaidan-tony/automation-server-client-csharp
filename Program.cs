using System;
using System.Threading;
using System.Collections.Generic;
using AutomationServerClient.Messages;

namespace AutomationServerClient
{
    class Program
    {
        static ClientSession s;

        static bool connected;
        static void Main(string[] args)
        {
            connected = false;
            Console.WriteLine("Hello World!");
            //s = new ClientSession("O9372DPXM78ONG0", "HP61BKEPKNVGAMG", "RITAA");
            //s = new ClientSession("K57EOE0PXJNDSH1", "652C4G36KQLIY3A", "RITAA");
            //s = new ClientSession("O9372DPXM78ONG0", "NDD7QR9LPKBGLVY", "ESP32");
            s = new ClientSession("T4I9EALB62QH10W","FMQFBNG3BGVE4KV","AAAAA");
            s.Connect();
            s.OnConnected += OnConnected;
            s.onMessageReady += readMessage;

            while(!connected) {
                Thread.Sleep(100);
            }

            while(s.IsRunning()){
                string y = Console.ReadLine().Trim();
                if(y == "e")
                    break;
                Console.WriteLine("xxx...");
                Message m = new DataMessageBuilder()
                               .setMessage(y)
                               .setOrigin("ARDNO")
                               .build();

                s.sendMessage(m);
            }
            s.close();
        }

        static Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
        static void readMessage(Message msg) {
            Console.WriteLine("Received from " + msg.getOrigin());
            
            if(typeof(GenericDataMessage).IsInstanceOfType(msg)){
                GenericDataMessage dataMsg = (GenericDataMessage)msg;
                
                byte[] data = dataMsg.getData();
                Console.WriteLine(System.Text.Encoding.ASCII.GetString(data));

                Message m = new DataMessageBuilder()
                                               .setMessage("Hello back from Unity")
                                               .setOrigin(msg.getOrigin())
                                               .build();
                // Message m = new DataMessageBuilder().setMessage(new byte[] { data[0], (byte)(status ? 1 : 0) }).setOrigin(msg.getOrigin()).build();
                s.sendMessage(m);
            }else if(typeof(ErrorMessage).IsInstanceOfType(msg)){
                ErrorMessage error = (ErrorMessage)msg;
                Console.WriteLine(error.getErrorCode());
                Console.WriteLine(error.getMessage());
            }
        }

        static void OnConnected(){
            connected = true;
        }
    }
}
