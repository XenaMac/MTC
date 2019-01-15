using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace FPSService.CADIntegration
{
    //This is currently unused
    public class CADQueue
    {
        private Queue<string> queue = new Queue<string>();
        private Semaphore queueCount = new Semaphore(0, 1000);

        public CADQueue() { }

        public string WaitForMessage()
        {
            string message;
            queueCount.WaitOne();
            if (queue.Count > 0)
            {
                message = queue.Dequeue();
                return message;
            }
            else
            {
                return null;
            }
        }

        public void Enqueue(string Message)
        {
            if (queue.Count >= 9999)
            {
                queue.Clear();
                queueCount.Release();
            }
            queue.Enqueue(Message);
            queueCount.Release();
        }

        public void ClearQueue()
        {
            queue.Clear();
        }
    }
}