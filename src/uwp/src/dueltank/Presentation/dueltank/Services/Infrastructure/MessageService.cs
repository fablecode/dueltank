
using System;
using System.Collections.Generic;
using System.Linq;
using dueltank.ViewModels.Infrastructure.Services;

namespace dueltank.Services.Infrastructure
{
    public class MessageService : IMessageService
    {
        private readonly List<Subscriber> _subscribers = new List<Subscriber>();
        private readonly object _sync = new object();

        public void Subscribe<TSender>(object target, Action<TSender, string, object> action) where TSender : class
        {
            Subscribe<TSender, object>(target, action);
        }

        public void Subscribe<TSender, TArgs>(object target, Action<TSender, string, TArgs> action)
            where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (_sync)
            {
                var subscriber = _subscribers.FirstOrDefault(r => r.Target == target);
                if (subscriber == null)
                {
                    subscriber = new Subscriber(target);
                    _subscribers.Add(subscriber);
                }

                subscriber.AddSubscription(action);
            }
        }

        public void Unsubscribe<TSender>(object target) where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.FirstOrDefault(r => r.Target == target);
                if (subscriber != null)
                {
                    subscriber.RemoveSubscription<TSender>();
                    if (subscriber.IsEmpty) _subscribers.Remove(subscriber);
                }
            }
        }

        public void Unsubscribe<TSender, TArgs>(object target) where TSender : class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.FirstOrDefault(r => r.Target == target);
                if (subscriber != null)
                {
                    subscriber.RemoveSubscription<TSender, TArgs>();
                    if (subscriber.IsEmpty) _subscribers.Remove(subscriber);
                }
            }
        }

        public void Unsubscribe(object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            lock (_sync)
            {
                var subscriber = _subscribers.FirstOrDefault(r => r.Target == target);
                if (subscriber != null) _subscribers.Remove(subscriber);
            }
        }

        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            if (sender == null)
                throw new ArgumentNullException(nameof(sender));

            foreach (var subscriber in GetSubscribersSnapshot())
                // Avoid sending message to self
                if (subscriber.Target != sender)
                    subscriber.TryInvoke(sender, message, args);
        }

        private Subscriber[] GetSubscribersSnapshot()
        {
            lock (_sync)
            {
                return _subscribers.ToArray();
            }
        }

        private class Subscriber
        {
            private readonly WeakReference _reference;

            private readonly Dictionary<Type, Subscriptions> _subscriptions;

            public Subscriber(object target)
            {
                _reference = new WeakReference(target);
                _subscriptions = new Dictionary<Type, Subscriptions>();
            }

            public object Target => _reference.Target;

            public bool IsEmpty => _subscriptions.Count == 0;

            public void AddSubscription<TSender, TArgs>(Action<TSender, string, TArgs> action)
            {
                if (!_subscriptions.TryGetValue(typeof(TSender), out var subscriptions))
                {
                    subscriptions = new Subscriptions();
                    _subscriptions.Add(typeof(TSender), subscriptions);
                }

                subscriptions.AddSubscription(action);
            }

            public void RemoveSubscription<TSender>()
            {
                _subscriptions.Remove(typeof(TSender));
            }

            public void RemoveSubscription<TSender, TArgs>()
            {
                if (_subscriptions.TryGetValue(typeof(TSender), out var subscriptions))
                {
                    subscriptions.RemoveSubscription<TArgs>();
                    if (subscriptions.IsEmpty) _subscriptions.Remove(typeof(TSender));
                }
            }

            public void TryInvoke<TArgs>(object sender, string message, TArgs args)
            {
                var target = _reference.Target;
                if (_reference.IsAlive)
                {
                    var senderType = sender.GetType();
                    foreach (var keyValue in _subscriptions.Where(r => r.Key.IsAssignableFrom(senderType)))
                    {
                        var subscriptions = keyValue.Value;
                        subscriptions.TryInvoke(sender, message, args);
                    }
                }
            }
        }

        private class Subscriptions
        {
            private readonly Dictionary<Type, Delegate> _subscriptions;

            public Subscriptions()
            {
                _subscriptions = new Dictionary<Type, Delegate>();
            }

            public bool IsEmpty => _subscriptions.Count == 0;

            public void AddSubscription<TSender, TArgs>(Action<TSender, string, TArgs> action)
            {
                _subscriptions.Add(typeof(TArgs), action);
            }

            public void RemoveSubscription<TArgs>()
            {
                _subscriptions.Remove(typeof(TArgs));
            }

            public void TryInvoke<TArgs>(object sender, string message, TArgs args)
            {
                var argsType = typeof(TArgs);
                foreach (var keyValue in _subscriptions.Where(r => r.Key.IsAssignableFrom(argsType)))
                {
                    var action = keyValue.Value;
                    action?.DynamicInvoke(sender, message, args);
                }
            }
        }
    }
}