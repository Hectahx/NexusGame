using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sprite SoundIcon;
    public Sprite MuteIcon;
    public Button muteButton;
    AudioSource mainMenu;

    bool isMute = true;

    void Start()
    {
        //DontDestroyOnLoad(this);
        muteButton.image.sprite = MuteIcon;
        mainMenu = gameObject.GetComponent<AudioSource>();
        mainMenu.Play();
    }

    public void toggleMute()
    {
        if (!isMute)
        {
            muteButton.image.sprite = MuteIcon;
            mainMenu.Pause();
            isMute = true;
            Debug.Log("Muted");
        }
        else if (isMute)
        {
            muteButton.image.sprite = SoundIcon;
            mainMenu.UnPause();
            isMute = false;
            Debug.Log("UnMuted");
        }
    }


}
