using System;
using System.Collections.Generic;
using GameCore;
using Platform;

namespace HotLogic
{
    public enum MessengerMode : byte
    {
        DONT_REQUIRE_LISTENER,
        REQUIRE_LISTENER,
    }
    static internal class MessengerInternal
    {
        static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
        static public readonly MessengerMode DEFAULT_MODE = MessengerMode.DONT_REQUIRE_LISTENER;

        static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
        {
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
                }
            }
        }
        static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
        {
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
                    }
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                    }
                }
            }
            else
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
                }
            }
        }

        static public void OnListenerRemoved(string eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        static public void OnBroadcasting(string eventType, MessengerMode mode)
        {
            if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventType))
            {
                if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                {
                    GameDebug.LogError(string.Format("Broadcasting message {0} but no listener found.", eventType));
                }
            }
        }
    }
    // No parameters
    public class Messenger
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType)
        {
            Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action Action = d as Action;
                if (Action != null)
                {
                    Action();
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // One parameter
    static public class Messenger<T>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;
        static public void AddListener(string eventType, Action<T> handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1)
        {
            Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T> Action = d as Action<T>;
                if (Action != null)
                {
                    Action(arg1);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // Two parameters
    static public class Messenger<T, U>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action<T, U> handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T, U> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1, U arg2)
        {
            Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U> Action = d as Action<T, U>;
                if (Action != null)
                {
                    Action(arg1, arg2);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // Three parameters
    static public class Messenger<T, U, V>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action<T, U, V> handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T, U, V> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3)
        {
            Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V> Action = d as Action<T, U, V>;
                if (Action != null)
                {
                    Action(arg1, arg2, arg3);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // Four parameters
    static public class Messenger<T, U, V, W>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action<T, U, V, W> handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T, U, V, W> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4)
        {
            Broadcast(eventType, arg1, arg2, arg3, arg4, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V, W> Action = d as Action<T, U, V, W>;
                if (Action != null)
                {
                    Action(arg1, arg2, arg3, arg4);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // Five parameters
    static public class Messenger<T, U, V, W, X>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action<T, U, V, W, X> handler)
        {
            eventTable[eventType] = (Action<T, U, V, W, X>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T, U, V, W, X> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W, X>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5)
        {
            Broadcast(eventType, arg1, arg2, arg3, arg4, arg5, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V, W, X> Action = d as Action<T, U, V, W, X>;
                if (Action != null)
                {
                    Action(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
    // Six parameters
    static public class Messenger<T, U, V, W, X, Y>
    {
        private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

        static public void AddListener(string eventType, Action<T, U, V, W, X, Y> handler)
        {
            MessengerInternal.OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W, X, Y>)eventTable[eventType] + handler;
        }

        static public void RemoveListener(string eventType, Action<T, U, V, W, X, Y> handler)
        {
            MessengerInternal.OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W, X, Y>)eventTable[eventType] - handler;
            MessengerInternal.OnListenerRemoved(eventType);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6)
        {
            Broadcast(eventType, arg1, arg2, arg3, arg4, arg5, arg6, MessengerInternal.DEFAULT_MODE);
        }

        static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6, MessengerMode mode)
        {
            MessengerInternal.OnBroadcasting(eventType, mode);
            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V, W, X, Y> Action = d as Action<T, U, V, W, X, Y>;
                if (Action != null)
                {
                    Action(arg1, arg2, arg3, arg4, arg5, arg6);
                }
                else
                {
                    if (GameConfig.GetDefineStatus(EDefineType.DEBUG))
                    {
                        GameDebug.LogError($"{eventType} not find");
                    }
                }
            }
        }
    }
}