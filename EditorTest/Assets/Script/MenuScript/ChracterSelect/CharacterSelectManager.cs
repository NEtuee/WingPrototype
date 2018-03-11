using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour {

    public Button[] selectButtons;
    public int selected = 0;

    public SpriteRenderer sprite;
    public Text nameText;
    public Text explText;

    public CharacterDatabase database;

    public void Start()
    {
        ButtonPressed(selected);
    }

    public void SetInteractive()
    {
        int count = selectButtons.Length;
        for(int i = 0; i < count; ++i)
        {
            selectButtons[i].interactable = true;
        }
    }

    public void ButtonPressed(int num)
    {
        SetInteractive();
        selected = num;
        selectButtons[selected].interactable = false;

        //ChracterSelect();
    }

    public void ChracterSelect()
    {
        CharacterDatabase.CharacterInfo info = database.data[selected];
        sprite.sprite = info.portrait;
        nameText.text = info.name;
        explText.text = info.expl;
    }
}
