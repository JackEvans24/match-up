using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();

        public void Raise()
        {
            foreach (var listener in listeners)
                listener.OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener) => listeners.Add(listener);

        public void DeregisterListener(GameEventListener listener) => listeners.Remove(listener);
    }

    public class GameEvent<T> : ScriptableObject
    {
        private HashSet<GameEventListener<T>> listeners = new HashSet<GameEventListener<T>>();

        public void Raise(T data)
        {
            foreach (var listener in listeners)
                listener.OnEventRaised(data);
        }

        public void RegisterListener(GameEventListener<T> listener) => listeners.Add(listener);

        public void DeregisterListener(GameEventListener<T> listener) => listeners.Remove(listener);
    }
}
