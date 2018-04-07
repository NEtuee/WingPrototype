using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseContainer : MonoBehaviour {

	public static DatabaseContainer instance;
	public void Awake() {instance = this;}

	public CharacterDatabase characterDatabase;
	public EnemyDatabase enemyDatabase;
	public WorldDatabase worldDatabase;
	public PathDatabase pathDatabase;
}
