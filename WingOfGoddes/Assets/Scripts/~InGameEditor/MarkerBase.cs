using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerBase : MonoBehaviour {

	public SpriteRenderer spriteRenderer;
	public Collider2D[] coll;

	protected Vector2 mousePos;
	protected Vector2 clickPos;
	protected Vector2 originPos;

	public delegate void Sync();

	public virtual void Progress(Sync[] s){}

	public bool isMove = false;

	public virtual void ColorCheck(bool select)
	{
		if(select)
		{
			spriteRenderer.color = Color.green;
		}
		else
		{
			spriteRenderer.color = Color.white;
		}
	}

	public Vector2 GetMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public int CollisionCheck(Vector2 point)
	{
		for(int i = 0; i < coll.Length; ++i)
		{
			if(coll[i].OverlapPoint(point))
				return i;
		}

		return -1;
	}
}
