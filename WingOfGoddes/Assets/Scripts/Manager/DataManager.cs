using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataManager {

	[DllImport("user32.dll")]
	private static extern void OpenFileDialog();
	[DllImport("user32.dll")]
	private static extern void SaveFileDialog();

	public delegate void CallBack(string s);

#if UNITY_EDITOR
	public static void SetDirty(Object target)
	{
		EditorUtility.SetDirty(target);
	}
#endif

	public static void WriteStringToFile(string[] str,string fileName)
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

	public string[] ReadStringFromFile(string fileName, string p = "0")
	{
#if !WEB_BUILD

		string path = p;

		if(path == "0")
		{
			PathForDocumentsFile(fileName);
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

	public static void SaveDialog(CallBack s)
	{
		System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
		sfd.Filter = "DataFile|*.dat|TextFile|*.txt";
		sfd.ShowDialog();
		if(sfd.FileName != "")
		{
			s(sfd.FileName);
		}
	}

	public static void LoadDialog(CallBack s)
	{
		System.Windows.Forms.OpenFileDialog lfd = new System.Windows.Forms.OpenFileDialog();
 
      	lfd.ShowDialog();
		if(lfd.FileName != "")
		{
			s(lfd.FileName);
		}
	}
}
