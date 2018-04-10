using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {
	
	private static Dictionary<GlobalEvent, HashSet<Action>> container = new Dictionary<GlobalEvent, HashSet<Action>>();


	public static void FireEvent(GlobalEvent evt) {
		HashSet<Action> listeners;
		lock (container) {
			if (container.TryGetValue(evt, out listeners)) {
				foreach (Action listener in listeners) {
					listener.Invoke();
				}
			}
		}
	}


	public static void AddListener(GlobalEvent evt, Action listener) {
		HashSet<Action> listeners;
		lock (container) {
			if (!container.TryGetValue(evt, out listeners)) {
				listeners = new HashSet<Action>();
				container.Add(evt, listeners);
			}
			listeners.Add(listener);
		}
	}

	public static void RemoveListeners(GlobalEvent evt) {
		lock (container) {
			container.Remove(evt);
		}
	}

}
