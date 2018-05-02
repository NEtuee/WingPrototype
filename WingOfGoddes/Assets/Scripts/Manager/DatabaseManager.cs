using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour {

	public static DatabaseManager instance;

	public void Awake()
	{
		instance = this;
	}

	public SpriteDatabase spriteDatabase;
	
	public Sprite GetSprite(int group, int set, int index)
	{
		return spriteDatabase.GetSprite(group,set,index);
	}
}
