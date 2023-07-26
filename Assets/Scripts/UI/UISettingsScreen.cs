using UnityEngine;
using UnityEngine.UI;

public class UISettingsScreen : UIScreen
{
	[SerializeField]
	private Slider musicSlider;
	[SerializeField]
	private Slider soundSlider;

	private float _musicSliderValue;
	private float _soundSliderValue;

	private void Start()
	{
		musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("musicVolume", 0.75f));
		soundSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("sfxVolume", 0.75f));

		_musicSliderValue = musicSlider.value;
		_soundSliderValue = soundSlider.value;

		SoundController.Instance.audioMixer.SetFloat("music", Mathf.Log10(musicSlider.value) * 20);
		SoundController.Instance.audioMixer.SetFloat("sfx", Mathf.Log10(soundSlider.value) * 20);
	}

	/*public override void Open()
	{
		base.Open();


	}*/

	public void MusicSliderOnValueChanged(float value)
	{
		_musicSliderValue = value;
		ApplyMusicValue(value);
	}
	public void SoundSliderOnValueChanged(float value)
	{
		_soundSliderValue = value;
		ApplySoundValue(value);
	}

	public void SoundSliderOnValueDone()
	{
		SoundController.Instance.PlaySFX(SoundController.Instance.sfxBallInteract01);
	}

	public void MusicButtonClick()
	{
		if (_musicSliderValue > 0.0001f)
		{
			if (musicSlider.value > 0.0001f)
			{
				musicSlider.SetValueWithoutNotify(0.0001f);
				ApplyMusicValue(0.0001f);
			}
			else
			{
				musicSlider.SetValueWithoutNotify(_musicSliderValue);
				ApplyMusicValue(_musicSliderValue);
			}
		}
		else
		{
			_musicSliderValue = 0.75f;
			musicSlider.SetValueWithoutNotify(_musicSliderValue);
			ApplyMusicValue(_musicSliderValue);
		}
	}
	public void SoundButtonClick()
	{
		if (_soundSliderValue > 0.0001f)
		{
			if (soundSlider.value > 0.0001f)
			{
				soundSlider.SetValueWithoutNotify(0.0001f);
				ApplySoundValue(0.0001f);
			}
			else
			{
				soundSlider.SetValueWithoutNotify(_soundSliderValue);
				ApplySoundValue(_soundSliderValue);
				SoundSliderOnValueDone();
			}
		}
		else
		{
			_soundSliderValue = 0.75f;
			soundSlider.SetValueWithoutNotify(_soundSliderValue);
			ApplySoundValue(_soundSliderValue);
			SoundSliderOnValueDone();
		}
	}

	private void ApplyMusicValue(float value)
	{
		SoundController.Instance.audioMixer.SetFloat("music", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("musicVolume", value);
	}
	private void ApplySoundValue(float value)
	{
		SoundController.Instance.audioMixer.SetFloat("sfx", Mathf.Log10(value) * 20);
		PlayerPrefs.SetFloat("sfxVolume", value);
	}
}