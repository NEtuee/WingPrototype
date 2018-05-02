using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class SpriteEditor : EditorWindow
{
    [MenuItem("Window/SpriteEditor")]
    static void Open()
    {
        GetWindow<SpriteEditor>();
    }

    SpriteDatabase spriteDatabase;

    string[] spriteGroup;
    string[] names;
    int selectSpriteGroupIndex = 0;
    int prevSpriteGroupIndex = 0;
    int selectSpriteSetIndex = 0;

   // int selectSpriteSet = 0;
    Vector2 spritesScrollPos = new Vector2();


    Sprite[] sprites = null;

    string createGroupName = "";
    string createSpriteSetName = "";

    string splitName = "";
    Sprite splitSprite = null;
    int splitGroup = 0;
    int splitX = 3;
    int splitY = 3;
    Vector2 splitPivot = new Vector2(.5f,.5f);


    void OnEnable()
    {
        spriteDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/SpriteDatabase.asset", typeof(SpriteDatabase)) as SpriteDatabase;

        GetSpriteGroup();
        GetNames();
        GetSprites();

        SpriteGroupSelect();
    }
   // EditorGUI.DrawTextureTransparent(new Rect(25, 0, 100f, 100f))
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        createGroupName = EditorGUILayout.TextField(createGroupName);
        if(GUILayout.Button("AddSpriteGroup",GUILayout.Width(110f)))
        {
            if(spriteDatabase.KeyOverlapCheck(createGroupName))
            {
                int i = spriteDatabase.AddSpriteGroup(createGroupName);

                if(i != -1)
                {
                    selectSpriteGroupIndex = i;
                    GetSpriteGroup();
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Group Name Is Already Exists","ok");
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        createSpriteSetName = EditorGUILayout.TextField(createSpriteSetName);
        if (GUILayout.Button("AddSpriteSet",GUILayout.Width(110f)))
        {
            if(spriteDatabase.FindListFromIndex(selectSpriteGroupIndex) != null)
            {
                if(spriteDatabase.SpriteSetNameOverlapCheck(selectSpriteGroupIndex,createSpriteSetName))
                {
                    int i = spriteDatabase.AddSpriteSet(selectSpriteGroupIndex, new SpriteDatabase.SpriteSet(createSpriteSetName));
                
                    selectSpriteSetIndex = i == -1 ? selectSpriteSetIndex : i;
                    GetNames();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Group Name Is Already Exists", "ok");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "List is null", "ok");
            }
        }
        EditorGUILayout.EndHorizontal();

        // GUILayout.Label("ViewImageSize");

        // EditorGUILayout.BeginHorizontal();

        // // GUILayout.Label("Width",GUILayout.Width(50f));
        // // imageWidth = EditorGUILayout.IntField(imageWidth,GUILayout.Width(100f));
        // // GUILayout.Label("Height",GUILayout.Width(50f));
        // // imageHeight = EditorGUILayout.IntField(imageHeight,GUILayout.Width(100f));

        // EditorGUILayout.EndHorizontal();

       // spriteListArea = new Rect(position.width - 150f,0f,150f,250f);
       // GUILayout.BeginArea(spriteListArea);

        selectSpriteGroupIndex = EditorGUILayout.Popup("Group",selectSpriteGroupIndex, spriteGroup);
        if(prevSpriteGroupIndex != selectSpriteGroupIndex)
        {
            SpriteGroupSelect();

            selectSpriteSetIndex = 0;

            GetNames();
            GetSprites();
        }

        if(names != null && names.Length != 0)
        {
            selectSpriteSetIndex = EditorGUILayout.Popup("SpriteSet",selectSpriteSetIndex, names);
            GetSprites();
        }
        GUILayout.BeginVertical("box");

        UnityEngine.Object[] objects = DropAreaGUI();

        spritesScrollPos = GUILayout.BeginScrollView(spritesScrollPos);

        if(objects != null)
        {
            List<SpriteDatabase.SpriteSet> list = spriteDatabase.FindListFromIndex(selectSpriteGroupIndex);
            if(list != null)
            {
                if(list.Count > selectSpriteSetIndex)
                {
                    if(list[selectSpriteSetIndex].sprites == null)
                    {
                        list[selectSpriteSetIndex].sprites = new Sprite[objects.Length];
                    }
                    else
                    {
                        Array.Resize(ref list[selectSpriteSetIndex].sprites,objects.Length);
                    }

                    Type type = objects[0].GetType();

                    if(type == typeof(Texture2D))
                    {
                        for(int i = 0; i < objects.Length; ++i)
                        {
                            string path = AssetDatabase.GetAssetPath(objects[i]);
                            list[selectSpriteSetIndex].sprites[i] = AssetDatabase.LoadAssetAtPath(path,typeof(Sprite)) as Sprite;
                        }
                    }
                    else if(type == typeof(Sprite))
                    {
                        for(int i = 0; i < objects.Length; ++i)
                        {
                            list[selectSpriteSetIndex].sprites[i] = objects[i] as Sprite;
                        }
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "List is null", "ok");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "List is null", "ok");
            }

        }

        if(sprites != null)
        {
            for(int i = 0; i < sprites.Length; ++i)
            {
                sprites[i] = EditorGUILayout.ObjectField(sprites[i],typeof(Sprite),true) as Sprite;
            }
        }

        GUILayout.EndScrollView();

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("DeleteSpriteSet"))
        {
            int i = spriteDatabase.DeleteSpriteSet(selectSpriteGroupIndex,selectSpriteSetIndex);

            if(i != -2)
            {
                selectSpriteSetIndex = i == -1 ? 0 : i;
                GetSprites();
                GetNames();
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "List is Empty", "ok");
            }
        }
        if(GUILayout.Button("DeleteGroup"))
        {
            int i = spriteDatabase.DeleteSpriteGroup(selectSpriteGroupIndex);

            if(i != -2)
            {
                selectSpriteGroupIndex = i == -1 ? 0 : i;

                GetSprites();
                GetNames();
                GetSpriteGroup();
            }
            else
                EditorUtility.DisplayDialog("Error", "List is Empty", "ok");
        }

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();

        if(GUILayout.Button("Add Sprite"))
        {
            List<SpriteDatabase.SpriteSet> list = spriteDatabase.FindListFromIndex(selectSpriteGroupIndex);

            if(list != null)
            {
                if(list.Count > selectSpriteSetIndex)
                {
                    if(list[selectSpriteSetIndex].sprites == null)
                    {
                        list[selectSpriteSetIndex].sprites = new Sprite[1];
                    }
                    else
                    {
                        Array.Resize(ref list[selectSpriteSetIndex].sprites,list[selectSpriteSetIndex].sprites.Length + 1);
                    }

                    EditorUtility.SetDirty(spriteDatabase);

                    GetSprites();
                }
                else if(list.Count == 0)
                    EditorUtility.DisplayDialog("Error", "List is Empty", "ok");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "List is null", "ok");
            }

        }

        if(GUILayout.Button("Delete Sprite"))
        {
            List<SpriteDatabase.SpriteSet> list = spriteDatabase.FindListFromIndex(selectSpriteGroupIndex);

            if(list != null)
            {
                if(list.Count > selectSpriteSetIndex)
                {
                    if(list[selectSpriteSetIndex].sprites == null)
                    {
                        EditorUtility.DisplayDialog("Error", "Array is null", "ok");
                    }
                    else if(list[selectSpriteSetIndex].sprites.Length == 0)
                    {
                        EditorUtility.DisplayDialog("Error", "Array is Empty", "ok");
                    }
                    else
                    {
                        Array.Resize(ref list[selectSpriteSetIndex].sprites,list[selectSpriteSetIndex].sprites.Length - 1);

                        EditorUtility.SetDirty(spriteDatabase);

                        GetSprites();
                    }
                }
                else if(list.Count == 0)
                    EditorUtility.DisplayDialog("Error", "List is Empty", "ok");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "List is null", "ok");
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.Space(10f);
        GUILayout.Label("TextureSplit");
        splitName =  EditorGUILayout.TextField("SpriteSetName",splitName);
        splitGroup = EditorGUILayout.Popup("Group",splitGroup, spriteGroup);
        splitSprite = EditorGUILayout.ObjectField(splitSprite,typeof(Sprite),false) as Sprite;

        GUILayout.BeginHorizontal();

        GUILayout.Label("Tiles");
        GUILayout.Label("X");
        splitX = EditorGUILayout.IntField(splitX);
        GUILayout.Label("Y");
        splitY = EditorGUILayout.IntField(splitY);

        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();

        GUILayout.Label("Pivot");
        GUILayout.Label("X");
        splitPivot.x = EditorGUILayout.FloatField(splitPivot.x);
        GUILayout.Label("Y");
        splitPivot.y = EditorGUILayout.FloatField(splitPivot.y);

        GUILayout.EndHorizontal();

        if(GUILayout.Button("Split"))
        {
            if(splitName == "")
            {
                EditorUtility.DisplayDialog("Error", "Name is Empty", "ok");
            }
            else if(spriteDatabase.FindListFromIndex(splitGroup) == null)
            {
                EditorUtility.DisplayDialog("Error", "Group Does Not Exist", "ok");
            }
            else
            {
                if(spriteDatabase.SpriteSetNameOverlapCheck(splitGroup,splitName))
                {
                    UnityEngine.Object[] objs = SetMultiSprite(splitSprite.texture,splitX,splitY,splitPivot);
                    SpriteDatabase.SpriteSet sprSet = new SpriteDatabase.SpriteSet();

                    sprSet.name = splitName;
                    sprSet.sprites = new Sprite[objs.Length - 1];

                    for(int i = 1; i < objs.Length; ++i)
                        sprSet.sprites[i - 1] = objs[i] as Sprite;

                    selectSpriteGroupIndex = splitGroup;
                    int j = spriteDatabase.AddSpriteSet(splitGroup,sprSet);

                    if(j != -1)
                    {
                        selectSpriteSetIndex = j;

                        GetNames();
                        GetSprites();
                    }
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Name Already Exists In That Group", "ok");
                }
            }
        }
      //  GUILayout.EndArea();
    }

    void SpriteGroupSelect()
    {
        if (spriteGroup.Length == 0 || spriteGroup.Length - 1 < selectSpriteGroupIndex)
        {
            selectSpriteGroupIndex = prevSpriteGroupIndex;
            Debug.Log("overflow");
        }
        else
        {
            prevSpriteGroupIndex = selectSpriteGroupIndex;
        }
    }

    public void GetSprites()
    {
        sprites = spriteDatabase.GetSprites(selectSpriteGroupIndex,selectSpriteSetIndex);
    }

    public void GetNames()
    {
        names = spriteDatabase.GetNames(selectSpriteGroupIndex);
    }

    public void GetSpriteGroup()
    {
        spriteGroup = spriteDatabase.GetKeys();
    }

    UnityEngine.Object[] DropAreaGUI()
    {
        Event evt = Event.current;
        Rect drop_area = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
        GUI.Box(drop_area, "Sprites Drop");

        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!drop_area.Contains(evt.mousePosition))
                    return null;

                if(DragAndDrop.objectReferences[0].GetType() != typeof(Texture2D) &&
                    DragAndDrop.objectReferences[0].GetType() != typeof(Sprite))
                    return null;

                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    return DragAndDrop.objectReferences;
                }
                break;
        }
        return null;
    }

    public UnityEngine.Object[] SetMultiSprite(Texture2D tex, int w, int h, Vector2 pivot)
    {
        string path = AssetDatabase.GetAssetPath(tex);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
        ti.isReadable = true;
        ti.spriteImportMode = SpriteImportMode.Multiple;

        List<SpriteMetaData> newData = new List<SpriteMetaData>();

        int widthOrigin = 0;
        int heightOrigin = 0;

        GetImageSize(tex,out widthOrigin, out heightOrigin);

        int width = widthOrigin / w;
        int height = heightOrigin / h;

        for (int i = 0; i < widthOrigin; i += width)
        {
            for (int j = heightOrigin; j > 0; j -= height)
            {
                SpriteMetaData smd = new SpriteMetaData();
                smd.pivot = pivot;
                smd.alignment = 9;
                smd.name = (heightOrigin - j) / height + ", " + i / width;
                smd.rect = new Rect(i, j - height, width, height);

                newData.Add(smd);
            }
        }

        ti.spritesheet = newData.ToArray();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        UnityEngine.Object[] sprite = AssetDatabase.LoadAllAssetsAtPath(path);
        return sprite;
    }

    public bool GetImageSize(Texture2D asset, out int width, out int height) {
    if (asset != null) {
        string assetPath = AssetDatabase.GetAssetPath(asset);
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
 
        if (importer != null) {
            object[] args = new object[2] { 0, 0 };
            MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            mi.Invoke(importer, args);
 
            width = (int)args[0];
            height = (int)args[1];
 
            return true;
        }
    }
 
    height = width = 0;
    return false;
}
}
