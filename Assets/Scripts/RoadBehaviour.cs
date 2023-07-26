using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadBehaviour : MonoBehaviour
{
	//[SerializeField]
	//private Game game;

	[Space(10)]
	public float speed = 1.0f;
	

	[Space(10)]
	[SerializeField]
	private Transform platformsSpawn;
	[SerializeField]
	private Transform platformsStart;
	[SerializeField]
	private Transform platformsEnd;
	[SerializeField]
	private Transform platformsInteract;

	[Space(10)]
	[SerializeField]
	private PlatformBehaviour platformPrefab;
	[SerializeField]
	private Transform platformContainer;

	[Space(10)]
	[SerializeField]
	private float switchColorTime = 5.0f;
	[SerializeField]
	private float switchColorDuration = 2.0f;



	[SerializeField]
	private Color switchColorStart;
	[SerializeField]
	private Color[] switchColors;
	private Color _prevColor;
	//[SerializeField]
	//private Material material;

	public SpawnPattern spawnPattern;




	
	private float _distance;
	private int _countPlatform;


	private Material _material;

	private List<PlatformBehaviour> _platforms;


	private void Awake()
	{
		_material = GetComponentInChildren<MeshRenderer>().material;
	}

	private void Start()
	{
		_platforms = new List<PlatformBehaviour>();
		
	}

	public void Init()
	{
		InvokeRepeating("SwitchColor", 4, switchColorTime);

		_distance = Vector3.Distance(platformsSpawn.position, platformsStart.position);
		_countPlatform = 0;

		SpawnUpdate();

		_material.color = switchColorStart;
		_prevColor = _material.color;

		GameEvents.roadReadyToPlay?.Invoke(this);
	}

	public void Clear()
	{
		CancelInvoke("SwitchColor");

		foreach (PlatformBehaviour platform in _platforms)
		{
			Destroy(platform.gameObject);
		}
		_platforms.Clear();
	}

	public void SwitchColor()
	{
		Color newColor = _prevColor;
		while (newColor.Equals(_prevColor))
		{
			newColor = switchColors[UnityEngine.Random.Range(0, switchColors.Length)];
		}
		_prevColor = newColor;
		_material.DOColor(newColor, switchColorDuration);
	}


	public void GameUpdate(float deltaTime)
	{
		_distance += speed * deltaTime;

		SpawnUpdate();

		foreach (PlatformBehaviour platform in _platforms)
		{
			platform.transform.Translate(new Vector3(0, 0, -speed * deltaTime));
		}
	}

	

	private void SpawnUpdate()
	{
		while (spawnPattern.TryGetSpawnData(_distance, _countPlatform, out Vector3 position, out InteractType interactType))
		{
			PlatformBehaviour platform = _platforms.FirstOrDefault();
			PlatformBehaviour platformLast = _platforms.LastOrDefault();

			if (platform != null && platform.transform.position.z <= platformsEnd.position.z)
			{
				_platforms.Remove(platform);
			}
			else
			{
				platform = Instantiate<PlatformBehaviour>(platformPrefab, platformContainer);
			}

			platform.Init();
			platform.SetInteractType(interactType);
			if (platformLast != null)
			{
				platform.transform.position = new Vector3(0.0f, platformLast.transform.position.y, platformLast.transform.position.z) + position;
			}
			else
			{
				platform.transform.position = platformsStart.position + position;
			}

			_platforms.Add(platform);

			_countPlatform++;
		}
	}

	public PlatformBehaviour GetNextPlatform()
	{
		return _platforms.Find(x => !x.isInteracted);
	}
	public PlatformBehaviour GetNextPlatformAt(int count, InteractType interactType)
	{
		int i = 0;
		foreach (PlatformBehaviour platform in _platforms)
		{
			if (!platform.isInteracted)
			{
				i++;
				if (i >= count && platform.interactType == interactType)
				{
					return platform;
				}
			}
		}

		return GetNextPlatform();
	}
	public bool HasBonus(InteractType interactType)
	{
		foreach (PlatformBehaviour platform in _platforms)
		{
			if (!platform.isInteracted)
			{
				if (platform.interactType == interactType)
				{
					if (platform.bonus.bonusType != BonusType.None)
					{
						return true;
					}
				}
			}
		}

		return false;
	}
	public void ClearBonuses()
	{
		foreach (PlatformBehaviour platform in _platforms)
		{
			if (!platform.isInteracted)
			{
				if (platform.bonus.bonusType != BonusType.None)
				{
					platform.bonus.SetBonus(BonusType.None);
				}
			}
		}
	}

	public Vector3 GetInteractPoint()
	{
		return platformsInteract.position;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(platformsSpawn.position, Vector3.one);
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(platformsStart.position, Vector3.one);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(platformsEnd.position, Vector3.one);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(platformsInteract.position, Vector3.one);
		Gizmos.color = Color.white;
	}
}