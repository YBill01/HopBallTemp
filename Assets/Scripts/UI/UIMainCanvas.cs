using UnityEngine;

public class UIMainCanvas : MonoBehaviour
{
	public static UIMainCanvas Instance;


	



	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			throw new System.Exception("An instance of this singleton already exists.");
		}

		Instance = this;
	}

	private void Start()
	{
		DontDestroyOnLoad(gameObject);


	}



}