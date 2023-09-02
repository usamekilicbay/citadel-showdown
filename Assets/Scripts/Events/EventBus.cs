using System;
using System.Collections.Generic;

namespace CitadelShowdown.Events
{
    public class EventBus
    {
        private Dictionary<Type, List<EventHandler>> _events = new();

        public void Publish(object sender, EventArgs eventArgs)
        {
            if (!_events.TryGetValue(eventArgs.GetType(), out var subscribers))
                return;

            foreach (var subscriber in subscribers)
                subscriber(sender, eventArgs);
        }

        public void Subscribe<T>(EventHandler eventHandler) where T : EventArgs
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var actions))
                actions.Add(eventHandler);
            else
                _events.Add(type, new List<EventHandler>());
        }

        public void Unsubscribe<T>(EventHandler eventHandler) where T : EventArgs
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var subscribers))
                subscribers.Remove(eventHandler);
        }
    }
}
