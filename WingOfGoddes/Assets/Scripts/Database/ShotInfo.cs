[System.Serializable]
public class ShotInfo {

	public int bulletGroup;
	public int bulletIndex;

	public float speed = 5f;
	public float attack = 1f;
	public float angle = 0f;
	public float lifeTime = -1f;

	public float angleAccel = 0f;
	public float speedAccel = 0f;

	public bool penetrate = false;
	public bool rotationLock = false;
	public bool guided = false;

	public ShotInfo(){}
	public ShotInfo(float sp,float at,float an,float life,float angac,float spac,bool pen,bool rot, bool gu)
	{
		speed = sp;
		attack = at;
		angle = an;
		lifeTime = life;
		angleAccel = angac;
		speedAccel = spac;
		penetrate = pen;
		rotationLock = rot;
		guided = gu;
	}
}
