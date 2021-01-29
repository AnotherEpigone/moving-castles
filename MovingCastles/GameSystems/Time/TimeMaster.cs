using Priority_Queue;
using System;

namespace MovingCastles.GameSystems.Time
{
    public class TimeMaster : ITimeMaster
    {
        private readonly SimplePriorityQueue<ITimeMasterNode, long> _queue;
        private readonly McTimeSpan _journeyTime;

        public TimeMaster()
        {
            _journeyTime = new McTimeSpan(0);
            _queue = new SimplePriorityQueue<ITimeMasterNode, long>();
        }

        public event EventHandler<McTimeSpan> TimeUpdated;

        public McTimeSpan JourneyTime => new McTimeSpan(_journeyTime.Ticks);

        public void Enqueue(ITimeMasterNode node) => _queue.Enqueue(node, node.Time);

        public ITimeMasterNode Next()
        {
            var node = _queue.Dequeue();

            _journeyTime.SetTicks(node.Time);
            TimeUpdated?.Invoke(this, JourneyTime);

            return node;
        }
    }
}
