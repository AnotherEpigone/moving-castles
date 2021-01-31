using MovingCastles.Serialization.Map;
using Newtonsoft.Json;
using Priority_Queue;
using System;
using System.Collections.Generic;

namespace MovingCastles.GameSystems.Time
{
    [JsonConverter(typeof(TimeMasterJsonConverter))]
    public class TimeMaster : ITimeMaster
    {
        private readonly SimplePriorityQueue<ITimeMasterNode, long> _queue;
        private readonly McTimeSpan _journeyTime;

        public TimeMaster()
            : this(0)
        { }

        public TimeMaster(long initialTicks)
        {
            _journeyTime = new McTimeSpan(initialTicks);
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

        public IEnumerable<ITimeMasterNode> Nodes => _queue;
    }
}
