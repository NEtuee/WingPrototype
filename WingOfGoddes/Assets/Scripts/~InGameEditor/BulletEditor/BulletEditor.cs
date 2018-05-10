using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class BulletEditor : EditorBase {

	public InputField newBulletNameUI;
	public InputField bulletNameUI;
	public InputField newGroupNameUI;
	public Dropdown groupUI;
	public Dropdown bulletListUI;

	public SpriteRenderer bulletImage;

	public InputField currBulletNameUI;
	public InputField bulletAttackUI;
	public Dropdown bulletSpriteListUI;

	private string newGroupName = "";
	private string newBulletName = "";

	//private string bulletName = "";
	//private float bulletAttack = 1f;

	private int groupSelect = 0;
	private int bulletListSelect = 0;

	private int bulletSpriteGroup = 0;

	private SpriteDatabase spriteDatabase;
	private BulletDatabase bulletDatabase;

	public void Awake()
	{
		spriteDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/SpriteDatabase.asset", typeof(SpriteDatabase)) as SpriteDatabase;
		bulletDatabase = AssetDatabase.LoadAssetAtPath("Assets/Database/BulletDatabase.asset", typeof(BulletDatabase)) as BulletDatabase;

		bulletSpriteGroup = spriteDatabase.GetKeyPosition("BulletSpriteGroup");

		BulletSpriteListSync();
		BulletGroupSync();
		GetBulletName();
		BulletCurrAttackSync();
	}

	public void DeleteCurrGroup()
	{
		bulletDatabase.DeleteGroup(groupSelect);
		BulletGroupSync();
	}

	public void DeleteCurrBullet()
	{
		bulletDatabase.DeleteBullet(groupSelect,bulletListSelect);

		int count = bulletDatabase.bulletList[groupSelect].list.Count;

		bulletListSelect = bulletListSelect < count ? bulletListSelect : (count - 1 < 0 ? 0 : count - 1);

		BulletListSync();

		GetBulletName();
		BulletCurrAttackSync();
	}

	public void AddNewBullet()
	{
		if(bulletDatabase.bulletList.Count == 0)
		{
			Debug.Log("group is null");
			return;
		}

		if(newBulletName.CompareTo("") == 0)
		{
			Debug.Log("bullet name is empty");
			return;
		}
		if(!bulletDatabase.BulletNameOverlapCheck(groupSelect,newBulletName))
		{
			Debug.Log("bullet is alread exists");
			return;
		}

		bulletDatabase.AddNewBullet(groupSelect,newBulletName, new SpriteDatabase.SpriteIndexInfo());

		newBulletName = "";
		newBulletNameUI.text = "";

		bulletListSelect = bulletDatabase.bulletList[groupSelect].list.Count - 1;
		BulletListSync();
	}

	public void AddNewSpriteGroup()
	{
		if(newGroupName.CompareTo("") == 0)
		{
			Debug.Log("group name is empty");
			return;
		}
		if(!bulletDatabase.GroupNameOverlapCheck(newGroupName))
		{
			Debug.Log("group is alread exists");
			return;
		}

		bulletDatabase.AddNewGroup(newGroupName);

		newGroupName = "";
		newGroupNameUI.text = "";

		groupSelect = bulletDatabase.bulletList.Count - 1;
		BulletGroupSync();
	}

	public void BulletCurrAttackSync()
	{
		if(bulletDatabase.bulletList.Count > groupSelect)
		{
			if(bulletDatabase.bulletList[groupSelect].list.Count > bulletListSelect)
			{
				bulletAttackUI.text = bulletDatabase.bulletList[groupSelect].list[bulletListSelect].attack.ToString();
			}
		}
	}

	public void BulletAttackSync()
	{
		if(bulletDatabase.bulletList.Count > groupSelect)
		{
			if(bulletDatabase.bulletList[groupSelect].list.Count > bulletListSelect)
			{
				if(bulletAttackUI.text == "")
				{
					BulletCurrAttackSync();
				}
				else
				{
					bulletDatabase.bulletList[groupSelect].list[bulletListSelect].attack = float.Parse(bulletAttackUI.text);
				}
			}
		}
	}

	public void GetBulletName()
	{
		if(bulletDatabase.bulletList.Count > groupSelect)
		{
			if(bulletDatabase.bulletList[groupSelect].list.Count > bulletListSelect)
			{
				currBulletNameUI.text = bulletDatabase.bulletList[groupSelect].list[bulletListSelect].name;
			}
		}
	}

	public void BulletNameSync()
	{
		if(bulletDatabase.bulletList.Count > groupSelect)
		{
			if(bulletDatabase.bulletList[groupSelect].list.Count > bulletListSelect)
			{
				if(currBulletNameUI.text != "")
				{
					bulletDatabase.bulletList[groupSelect].list[bulletListSelect].name = currBulletNameUI.text;
				}
				else
					GetBulletName();
			}
		}
	}

	public void BulletCurrSpriteSync()
	{
		SpriteDatabase.SpriteIndexInfo info = GetSpriteInfo();

		if(info == null)
		{
			Debug.Log("list is null");
			return;
		}

		info.group = bulletSpriteGroup;
		info.set = 0;
		info.index = bulletSpriteListUI.value;

		bulletImage.sprite = spriteDatabase.GetSprite(info);
	}

	public void BulletSpriteInfoSync()
	{
		bulletDatabase.bulletList[groupSelect].list[bulletListSelect].spriteInfo.index = bulletSpriteListUI.value;
		BulletSpriteSync();
	}

	public void NewGroupNameSync()
	{
		newGroupName = newGroupNameUI.text;
	}

	public void NewBulletNameSync()
	{
		newBulletName = newBulletNameUI.text;
	}

	public void NewSpriteListChanged()
	{
		bulletListSelect = bulletListUI.value;

		GetBulletName();
		BulletCurrAttackSync();
		BulletSpriteSync();
	}

	public void GroupChanged()
	{
		groupSelect = groupUI.value;
		bulletListSelect = 0;

		BulletListSync();
		GetBulletName();
		BulletCurrAttackSync();

		BulletSpriteSync();
	}

	public void BulletGroupSync()
	{
		DropdownSync(ref groupUI,bulletDatabase.GetGroupNames(),groupSelect);
		BulletListSync();
	}

	public void BulletListSync()
	{
		DropdownSync(ref bulletListUI,bulletDatabase.GetListNames(groupSelect),bulletListSelect);

		if(bulletListUI.options.Count == 0)
		{
			bulletImage.sprite = null;
		}
		else
		{
			BulletSpriteSync();
		}
	}

	public void BulletSpriteListSync()
	{
		DropdownSync(ref bulletSpriteListUI,spriteDatabase.GetSpriteNames(bulletSpriteGroup,0),0);
	}

	public void DropdownSync(ref Dropdown dropdown,Dropdown.OptionData[] data,int select)
	{
		dropdown.options.Clear();
		if(data == null)
		{
			dropdown.captionText.text = "";
			return;
		}
		for(int i = 0; i < data.Length; ++i)
		{
			dropdown.options.Add(data[i]);
		}

		dropdown.value = select;
		dropdown.captionText.text = dropdown.options[select].text;
	}

	public void DropdownSync(ref Dropdown dropdown,string[] data,int select)
	{
		dropdown.options.Clear();
		if(data == null)
		{
			dropdown.captionText.text = "";
			return;
		}
		for(int i = 0; i < data.Length; ++i)
		{
			dropdown.options.Add(new Dropdown.OptionData(data[i]));
		}

		dropdown.value = select;
		dropdown.captionText.text = dropdown.options[select].text;
	}

	public SpriteDatabase.SpriteIndexInfo GetSpriteInfo()
	{
		if(bulletDatabase.bulletList.Count > groupSelect)
		{
			if(bulletDatabase.bulletList[groupSelect].list.Count > bulletListSelect)
			{
				return bulletDatabase.bulletList[groupSelect].list[bulletListSelect].spriteInfo;
			}
		}

		return null;
	}

	public void BulletSpriteSync()
	{
		SpriteDatabase.SpriteIndexInfo info = GetSpriteInfo();
		bulletImage.sprite = spriteDatabase.GetSprite(info);
		bulletSpriteListUI.value = info.index;
	}
}
