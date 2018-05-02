using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class LinkProgressManagerBase<S> : MonoBehaviour {

	public static S instance;

	public class ObjectLink<T>
	{
		public T[] me;
		public ObjectLink<T> back = null;

		public ObjectLink() {}
		public ObjectLink(T[] m, ObjectLink<T> b = null)
		{
			me = m;
			back = b;
		}
	}

	public abstract void Initialize();
	public abstract void Progress();
	public abstract void Release();

}
