using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FeverBase : MonoBehaviour {

	public DirectBase feverEndDirect;

    public virtual void FirstInit() { }

	public abstract void Initialize();
	public abstract void Progress();
	public abstract void EndEvent();
}
