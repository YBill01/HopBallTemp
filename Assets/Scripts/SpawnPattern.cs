using UnityEngine;

public abstract class SpawnPattern
{
	public abstract bool TryGetSpawnData(float distance, int count, out Vector3 position, out InteractType interactType);
	
}