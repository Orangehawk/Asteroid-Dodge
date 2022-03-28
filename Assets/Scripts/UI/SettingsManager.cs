using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
	//Settings
	//Control settings
	//Video settings
	[SerializeField]
	Resolution[] resolutions;
	[SerializeField]
	Dropdown resolutionDropdown;
	[SerializeField]
	Dropdown qualityDropdown;
	[SerializeField]
	Dropdown textureDropdown;
	[SerializeField]
	Dropdown aaDropdown;
	[SerializeField]
	Toggle fullscreenToggle;
	[SerializeField]
	Dropdown fullscreenModeDropdown;
	int currentResolutionIndex;
	//Game settings
	//Sound settings
	[SerializeField]
	AudioMixer audioMixer;
	[SerializeField]
	Slider soundVolumeSlider;
	[SerializeField]
	Slider musicVolumeSlider;
	float currentSoundVolume;
	float currentMusicVolume;


	public void SaveSettings()
	{
		PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
		PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
		PlayerPrefs.SetInt("TextureQualityPreference", textureDropdown.value);
		PlayerPrefs.SetInt("AntiAliasingPreference", aaDropdown.value);
		PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
		PlayerPrefs.SetInt("FullscreenModePreference", (int)Screen.fullScreenMode);
		PlayerPrefs.SetFloat("SoundVolumePreference", currentSoundVolume);
		PlayerPrefs.SetFloat("MusicVolumePreference", currentMusicVolume);
	}

	public void LoadSettings(int resolutionIndex = -1)
	{
		if (PlayerPrefs.HasKey("QualitySettingPreference"))
			qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
		else
			qualityDropdown.value = 3;

		if (PlayerPrefs.HasKey("ResolutionPreference"))
			resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
		else
			resolutionDropdown.value = currentResolutionIndex;

		if (PlayerPrefs.HasKey("TextureQualityPreference"))
			textureDropdown.value = PlayerPrefs.GetInt("TextureQualityPreference");
		else
			textureDropdown.value = 0;

		if (PlayerPrefs.HasKey("AntiAliasingPreference"))
			aaDropdown.value = PlayerPrefs.GetInt("AntiAliasingPreference");
		else
			aaDropdown.value = 1;

		if (PlayerPrefs.HasKey("FullscreenPreference"))
			Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
		else
			Screen.fullScreen = true;

		if (PlayerPrefs.HasKey("FullscreenModePreference"))
			Screen.fullScreenMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenModePreference");
		else
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;

		if (PlayerPrefs.HasKey("SoundVolumePreference"))
			soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolumePreference");
		else
			soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolumePreference");

		if (PlayerPrefs.HasKey("MusicVolumePreference"))
			musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");
		else
			musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");
	}

	//Control settings
	//Video settings
	void PopulateResolutions(Dropdown resolutionDrop)
	{
		resolutionDropdown.ClearOptions();
		List<string> options = new List<string>();
		resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60).ToArray();
		currentResolutionIndex = 0;

		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " +
					 resolutions[i].height;
			options.Add(option);
			if (resolutions[i].width == Screen.currentResolution.width
				  && resolutions[i].height == Screen.currentResolution.height)
				currentResolutionIndex = i;
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.RefreshShownValue();
	}

	public void SetFullscreen(bool fullscreen)
	{
		Screen.fullScreen = fullscreen;
	}

	public void SetFullscreenMode(int mode)
	{
		Screen.fullScreenMode = (FullScreenMode)mode;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetQuality(int qualityIndex)
	{
		if (qualityIndex != 6) // if the user is not using 
							   //any of the presets
			QualitySettings.SetQualityLevel(qualityIndex);
		switch (qualityIndex)
		{
			case 0: // quality level - very low
				textureDropdown.value = 3;
				aaDropdown.value = 0;
				break;
			case 1: // quality level - low
				textureDropdown.value = 2;
				aaDropdown.value = 0;
				break;
			case 2: // quality level - medium
				textureDropdown.value = 1;
				aaDropdown.value = 0;
				break;
			case 3: // quality level - high
				textureDropdown.value = 0;
				aaDropdown.value = 0;
				break;
			case 4: // quality level - very high
				textureDropdown.value = 0;
				aaDropdown.value = 1;
				break;
			case 5: // quality level - ultra
				textureDropdown.value = 0;
				aaDropdown.value = 2;
				break;
		}

		qualityDropdown.value = qualityIndex;
	}

	public void SetTexture(int textureIndex)
	{
		QualitySettings.masterTextureLimit = textureIndex;
		qualityDropdown.value = 6;
	}

	public void SetAntiAliasing(int aaIndex)
	{
		QualitySettings.antiAliasing = aaIndex;
		qualityDropdown.value = 6;
	}

	//Game settings
	//Sound settings
	public void SetSoundVolume(float volume)
	{
		audioMixer.SetFloat("Sound", volume);
		currentSoundVolume = volume;
	}

	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("Music", volume);
		currentMusicVolume = volume;
	}

	// Start is called before the first frame update
	void Start()
	{
		PopulateResolutions(resolutionDropdown);
		LoadSettings(currentResolutionIndex);
	}

	// Update is called once per frame
	void Update()
	{

	}
}
