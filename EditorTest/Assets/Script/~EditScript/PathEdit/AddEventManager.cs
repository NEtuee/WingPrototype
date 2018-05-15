using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddEventManager : MonoBehaviour {

	public int eventCode = 0;
	public EventInfoManager infoManager;

	public void SetEventCode(int c)
	{
		eventCode = c;
		infoManager.MenuActive(c);
	}

	public EventBase SetEvent(int frameCode)
	{
		switch((Test.Events)eventCode)
		{
		case Test.Events.Test:
			return new TestEvent();
		case Test.Events.ObjectCreate:
			CreateObjInfo.Info info = infoManager.GetCreateObjInfo();
			if(info == null)
			{
				return new ObjectCreateEvent(frameCode);
			}
			return new ObjectCreateEvent(info.code,info.pos,info.angle,frameCode);
		case Test.Events.EnemyCreate:
			string[] s = infoManager.GetCreateEnemyInfo().Split('/');
			return new EnemyCreateEvent(frameCode,int.Parse(s[0]),int.Parse(s[1]));
		case Test.Events.Dialog:
			return new DialogEvent();
		case Test.Events.Static:
			return new StaticEvent();
		case Test.Events.WaitExtinc:
			return new WaitExtinc();
		default:
			return null;
		}
	}
}
