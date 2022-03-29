using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sprite SoundIcon;
    public Sprite MuteIcon;
    public Button muteButton;

    Button volButton;
    AudioSource mainMenu;


    bool isMute = true;

    void Start()
    {
        DontDestroyOnLoad(this);
        muteButton.image.sprite = MuteIcon;
        mainMenu = gameObject.GetComponent<AudioSource>();
        mainMenu.Play();
    }

    public void toggleMute()
    {
        if (!isMute)
        {
            if (SceneManager.GetActiveScene().name == "MainDevelop") muteButton.image.sprite = MuteIcon;
            else volButton.image.sprite = MuteIcon;

            mainMenu.mute = true;
            isMute = true;
            Debug.Log("Muted");
        }
        else if (isMute)
        {
            if (SceneManager.GetActiveScene().name == "MainDevelop") muteButton.image.sprite = SoundIcon;
            else volButton.image.sprite = SoundIcon;

            mainMenu.mute = false;
            isMute = false;
            Debug.Log("UnMuted");
        }
    }

    void OnEnable()
    {

        SceneManager.sceneLoaded += MainGameScene;

    }
    void OnDisable()
    {

        SceneManager.sceneLoaded -= MainGameScene;

    }


    void MainGameScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameOnline")
        {
            volButton = GameObject.FindGameObjectWithTag("muteButton").GetComponent<Button>();
            volButton.onClick.AddListener(toggleMute);
            if(isMute) volButton.image.sprite = MuteIcon;
        }
    }




}
