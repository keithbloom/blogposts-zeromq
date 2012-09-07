using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZMQ;

namespace ZmqInProcConsole
{
    public class TimeServer
    {
        public void Server()
        {
        
            using (var context = new Context(1))
            using (var timeServer = context.Socket(SocketType.REP))
            {

                timeServer.Bind("tcp://*:5555");
                while(true)
                {
                    timeServer.Send(DateTime.Now.ToLongDateString(), Encoding.Unicode);
                }
            }   
        }

        public void Client()
        {
            using (var context = new Context(1))
            using (var client = context.Socket(SocketType.REQ))
            {
                client.Connect("tcp://localhost:5555");

                var time = client.Send("", Encoding.Unicode);
                Console.WriteLine(time);
            }
        }
    }
}
