using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
	public static App Instance { get; private set; }

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void Initialize()
	{
		Application.targetFrameRate = 60;
		Application.runInBackground = true;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	

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

		Debug.Log("App: Init -> Start");

		





		Debug.Log("App: Init -> End");
	}



	public void GoPlay()
	{
		SceneManager.LoadSceneAsync(1);
	}
	public void GoBack()
	{
		SceneManager.LoadSceneAsync(0);
	}


	public void Quit()
	{
		Application.Quit();
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			PlayerPrefs.Save();
		}
	}
}