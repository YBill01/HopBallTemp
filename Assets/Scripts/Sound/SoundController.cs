using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
	public static SoundController Instance { get; private set; }

	[Header("AudioMixer")]
	public AudioMixer audioMixer;

	[Header("AudioSources")]
	[SerializeField]
	private AudioSource musicSource;
	[SerializeField]
	private AudioSource sfxSource;

	[Header("AudioClips")]
	public AudioClip mainMusic01;
	public AudioClip mainMusic02;
	public AudioClip mainMusic03;
	public AudioClip mainMusic04;
	public AudioClip defeat;
	public AudioClip newRecord;
	[Space(10)]
	public AudioClip sfxBallInteract01;
	public AudioClip sfxBallInteract02;
	public AudioClip sfxBonusShow;
	public AudioClip sfxBonusHide;
	public AudioClip sfxBonusCollectedApple;
	public AudioClip sfxBonusCollectedCake;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			throw new System.Exception("An instance of this singleton already exists.");
		}
		Instance = this;
	}

	public void PlayMusic(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}
	public void StopMusic()
	{
		musicSource.Stop();
	}
	public void PlaySFX(AudioClip clip)
	{
		sfxSource.PlayOneShot(clip);
	}
}