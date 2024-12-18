using System;
using System.Threading;

namespace BasicExtensions
{
    public class ThreadExtensions
    {
        private Thread[] _threads;

        public int ThreadCount { get; private set; }

        public ThreadExtensions(int threadCount)
        {
            this.ThreadCount = threadCount;
            this._threads = new Thread[threadCount];
        }

        public void Run(ThreadStart start)
        {
            int threadIndex = FindAvailableThreadIndex();
            if (threadIndex == -1)
            {
                throw new InvalidOperationException("No available threads to run the task.");
            }

            _threads[threadIndex] = new Thread(start);
            _threads[threadIndex].Start();
        }

        public void WaitDone()
        {
            foreach (var thread in _threads)
            {
                thread?.Join(); // Tüm aktif threadlerin tamamlanmasını bekler
            }
        }

        private int FindAvailableThreadIndex()
        {
            for (int i = 0; i < _threads.Length; i++)
            {
                if (_threads[i] == null || !_threads[i].IsAlive)
                {
                    if (_threads[i] != null && !_threads[i].IsAlive)
                    {
                        _threads[i].Join(); // Sonlanan threadlerin kaynaklarını serbest bırakmak için Join kullanılır
                        _threads[i] = null;
                    }
                    return i;
                }
            }

            return -1; // Uygun thread yok
        }

        private bool AreAllThreadsDone()
        {
            foreach (var thread in _threads)
            {
                if (thread != null && thread.IsAlive)
                {
                    return false;
                }
            }
            return true;
        }
    }
}