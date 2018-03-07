using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : ObjectBase {

    public SpriteContainer.AnimationSet animationSet;
    public SpriteRenderer sprRenderer;

    private float aniTime = 0f;
	private int aniCount = 0;


    public override void Initialize()
    {
        GetTransform();
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Progress()
    {
        aniTime += Time.deltaTime;
		if(aniTime >= 0.0625)
		{
			sprRenderer.sprite = animationSet.sprites[aniCount++];
			aniTime = 0f;

			if(aniCount > animationSet.aniInfo[0].end)
            {
                if(animationSet.aniInfo[0].loop)
                {
                    aniCount = 0;
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
		}
    }

    public override void Release()
    {

    }

    public void SetAnimation(Vector2 pos,SpriteContainer.AnimationSet anim)
	{
        tp.position = pos;
		animationSet = anim;
		aniTime = 0f;
		aniCount = 0;

		sprRenderer.sprite = animationSet.sprites[aniCount++];

        gameObject.SetActive(true);
	}
}
