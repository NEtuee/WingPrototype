using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager : MonoBehaviour {

	public PathDatabase pathDatabase;
	public EnemyDatabase enemyDatabase;

	private bool saveCrash = false;
	private string saveName = "save.txt";

	public int PlayerInfoLoad()
	{
		return PlayerPrefs.GetInt("player");
	}

	public void PlayerInfoSave(int value)
	{
		PlayerPrefs.SetInt("player",value);
	}

#if UNITY_EDITOR
	public void SaveAllData()
	{
		EditorUtility.SetDirty(pathDatabase);
		EditorUtility.SetDirty(enemyDatabase);
	}
#endif

	public void WriteStringToFile(string[] str,string p = "0")
	{
#if !WEB_BUILD
		string path = p;
		if(path == "0")
		{
			PathForDocumentsFile(saveName);
		}
		FileStream file = new FileStream ( path, FileMode.Create, FileAccess.Write );
		StreamWriter sw = new StreamWriter( file );
		int line = str.Length;
		for(int i = 0; i < line; ++i)
		{
			sw.WriteLine(str[i]);
		}

		sw.Write("////");

		sw.Close();
		file.Close();
#endif
	}

	public static void WriteStringToFile_NoMark(string[] str,string fileName)
	{
#if !WEB_BUILD
		string path = PathForDocumentsFile(fileName);
		FileStream file = new FileStream ( path, FileMode.Create, FileAccess.Write );
		StreamWriter sw = new StreamWriter( file );
		int line = str.Length;

		for(int i = 0; i < line; ++i)
		{
			sw.WriteLine(str[i]);
		}

		sw.Close();
		file.Close();
#endif
	}

	public string[] ReadStringFromFile(string p = "0")
	{
#if !WEB_BUILD

		string path = p;

		if(path == "0")
		{
			PathForDocumentsFile(saveName);
		}
		if(File.Exists(path))
		{
			FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read);
			StreamReader st = new StreamReader(file);

			if(file == null || st == null)
			{
				Debug.Log("file is null");
				return null;
			}

			string[] s = null;
			s = st.ReadToEnd().Split('\n');

			st.Close();
			file.Close();

			return s;
		}
		else
			return null;
#else
		return null;
#endif
	}

	public static string ReadStringFromFile_NoSplit(string fileName)
	{
#if !WEB_BUILD

		string path = PathForDocumentsFile(fileName);

		if(File.Exists(path))
		{
			FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read);
			StreamReader st = new StreamReader(file);

			if(file == null || st == null)
			{
				Debug.Log("file is null");
				return null;
			}

			string s = null;
			s = st.ReadToEnd();

			st.Close();
			file.Close();

			return s;
		}
		else
		{
			Debug.Log("file is empty");
			return null;
		}
#else
		return null;
#endif
	}

	public static string PathForDocumentsFile(string str)
	{
		string path = "";
		if(Application.platform == RuntimePlatform.IPhonePlayer)
		{
			Debug.Log("notyet");
		}
		else if(Application.platform == RuntimePlatform.Android)
		{
			path = Application.persistentDataPath;
			path = path.Substring(0,path.LastIndexOf('/'));
		}
		else
		{
			path = Application.dataPath;
			path = path.Substring(0,path.LastIndexOf('/'));
		}

		return Path.Combine(path,str);
	}

}
