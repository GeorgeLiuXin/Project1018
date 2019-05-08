using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XWorld
{
	public enum GameEntityType
	{
		Empty,
		Player,
		Enemy,
	}

	public interface GameBaseEntity
	{
		void Init();
		void Tick(float fElapseTimes);

		int GetAvatarID();
		GameEntityType GetEntityType();

		bool CreateEngineObj();
		void OnEngineObjectLoadEnd(LoadResult result);
		void DestroyEngineObj();
		GameObject GetEngineObj();

		void SetPos(Vector3 vPos);
		void SetDir(Vector3 vForward);
		Vector3 GetPos();
		Vector3 GetDir();

		void RegEngineObjEvent();
		void UnRegEngineObjEvent();
	}
}
