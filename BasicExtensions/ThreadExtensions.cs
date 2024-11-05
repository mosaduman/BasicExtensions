using System.Threading;

namespace BasicExtensions
{
    public class ThreadExtensions
    {
        private Thread[] _threads;

        public int ThreadCount { get; set; }

        public ThreadExtensions(int threadCount)
        {
            this.ThreadCount = threadCount;
            this.CreateThread();
        }

        public void WaitDone()
        {
            do
                ;
            while (!this.AreAllThreadDone());
        }

        public void Run(ThreadStart start)
        {
            int referanceThreadIndex = this.FindNullReferanceThreadIndex();
            this._threads[referanceThreadIndex] = new Thread(start);
            this._threads[referanceThreadIndex].Start();
        }

        private void CreateThread()
        {
            if (this._threads == null)
            {
                this._threads = new Thread[this.ThreadCount];
                for (int index = 0; index < this.ThreadCount; ++index)
                    this._threads[index] = (Thread)null;
            }
            else if (this._threads.Length > this.ThreadCount)
            {
                Thread[] threadArray = new Thread[this.ThreadCount];
                for (int index = 0; index < this.ThreadCount; ++index)
                    threadArray[index] = this._threads[index];
                this._threads = threadArray;
            }
            else
            {
                if (this._threads.Length >= this.ThreadCount)
                    return;
                Thread[] threadArray = new Thread[this.ThreadCount];
                for (int index = 0; index < this._threads.Length; ++index)
                    threadArray[index] = this._threads[index];
                for (int length = this._threads.Length; length < this.ThreadCount; ++length)
                    threadArray[length] = this._threads[length];
                this._threads = threadArray;
            }
        }

        private int FindNullReferanceThreadIndex()
        {
        label_0:
            for (int referanceThreadIndex = 0; referanceThreadIndex < this.ThreadCount; ++referanceThreadIndex)
            {
                if (this._threads[referanceThreadIndex] == null)
                    return referanceThreadIndex;
                if (!this._threads[referanceThreadIndex].IsAlive)
                {
                    this._threads[referanceThreadIndex].Join();
                    this._threads[referanceThreadIndex] = (Thread)null;
                    return referanceThreadIndex;
                }
            }
            goto label_0;
        }

        private bool AreAllThreadDone()
        {
            bool flag = true;
            for (int index = 0; index < this.ThreadCount; ++index)
            {
                if (this._threads[index] != null)
                {
                    if (this._threads[index].IsAlive)
                        flag = false;
                    else
                        this._threads[index] = (Thread)null;
                }
            }
            return flag;
        }
    }
}
