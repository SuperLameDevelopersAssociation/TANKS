using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterVolumeSlider;
    public Slider tankVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider ambienceVolumeSlider;

    public Toggle muteToggle;
    bool muted;

    private void Start()
    {
        if(PlayerPrefs.GetInt("NewGame") == 0)                                  //Check to see if this is the first time game ran on this computer
        {
            PlayerPrefs.SetInt("NewGame", 1);                                   
            
            mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
            mixer.SetFloat("TankVolume", tankVolumeSlider.value);
            mixer.SetFloat("MusicVolume", musicVolumeSlider.value);             //Sets volume to default values
            mixer.SetFloat("AmbientVolume", ambienceVolumeSlider.value);
        }
        else
        {
            if(PlayerPrefs.GetInt("Muted") == 0)
            {
                masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
                tankVolumeSlider.value = PlayerPrefs.GetFloat("TankVolume");
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
                ambienceVolumeSlider.value = PlayerPrefs.GetFloat("AmbientVolume");

                mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
                mixer.SetFloat("TankVolume", tankVolumeSlider.value);
                mixer.SetFloat("MusicVolume", musicVolumeSlider.value);             //Sets volume to previous settings
                mixer.SetFloat("AmbientVolume", ambienceVolumeSlider.value);
            }
            else
            {
                muteToggle.isOn = true;
                Mute();
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteAll();
            print("Prefs Reset");
        }
    }

    public void SetMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        if(!muted)
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
    }


    public void SetTankVolume()
    {
        mixer.SetFloat("TankVolume", tankVolumeSlider.value);
        if (!muted)
            PlayerPrefs.SetFloat("TankVolume", tankVolumeSlider.value);
    }

    public void SetMusicVolume()
    {
        mixer.SetFloat("MusicVolume", musicVolumeSlider.value);
        if (!muted)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
    }

    public void SetAmbienceVolume()
    {
        mixer.SetFloat("AmbientVolume", ambienceVolumeSlider.value);
        if (!muted)
            PlayerPrefs.SetFloat("AmbientVolume", ambienceVolumeSlider.value);
    }

    public void MuteAll()
    {
        if(PlayerPrefs.GetInt("Muted") == 0)
            Mute();
        else
            UnMute();
    }

    void Mute()
    {
        muted = true;
        muteToggle.isOn = true;

        PlayerPrefs.SetInt("Muted", 1);
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.SetFloat("TankVolume", tankVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);          //Saves current value
        PlayerPrefs.SetFloat("AmbientVolume", ambienceVolumeSlider.value);

        masterVolumeSlider.value = -80;
        tankVolumeSlider.value = -80;
        musicVolumeSlider.value = -80;          //Sets ui to old values
        ambienceVolumeSlider.value = -80;

        masterVolumeSlider.interactable = false;
        tankVolumeSlider.interactable = false;
        musicVolumeSlider.interactable = false;
        ambienceVolumeSlider.interactable = false;

        mixer.SetFloat("MasterVolume", -80);                                      //Mute all
        mixer.SetFloat("TankVolume", -80);
        mixer.SetFloat("MusicVolume", -80);
        mixer.SetFloat("AmbientVolume", -80);

    }

    void UnMute()
    {
        muted = false;
        muteToggle.isOn = false;

        PlayerPrefs.SetInt("Muted", 0);
        print(PlayerPrefs.GetFloat("MasterVolume"));
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        tankVolumeSlider.value = PlayerPrefs.GetFloat("TankVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");          //Sets ui to old values
        ambienceVolumeSlider.value = PlayerPrefs.GetFloat("AmbientVolume");

        masterVolumeSlider.interactable = true;
        tankVolumeSlider.interactable = true;
        musicVolumeSlider.interactable = true;
        ambienceVolumeSlider.interactable = true;

        mixer.SetFloat("MasterVolume", masterVolumeSlider.value);
        mixer.SetFloat("TankVolume", tankVolumeSlider.value);
        mixer.SetFloat("MusicVolume", musicVolumeSlider.value);                 //Sets old volume
        mixer.SetFloat("AmbientVolume", ambienceVolumeSlider.value);
    }
}
