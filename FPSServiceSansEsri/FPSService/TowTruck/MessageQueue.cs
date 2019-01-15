using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace FPSService.TowTruck
{
    public class MessageQueue
    {
        private Queue<string> queue = new Queue<string>();
        private object lockObject = new object();
        private Semaphore queueCount = new Semaphore(0, 10000);

        public MessageQueue() { }

        public string WaitForMessage()
        {
            string message;
            queueCount.WaitOne();
            lock (lockObject)
            {
                message = queue.Dequeue();
            }
            return message;
        }

        public void Enqueue(string message)
        {
            if (queue.Count >= 10000)
            {
                queue.Clear();
            }
            lock (lockObject)
            {
                queue.Enqueue(message);
            }
            queueCount.Release();
        }
    }
}