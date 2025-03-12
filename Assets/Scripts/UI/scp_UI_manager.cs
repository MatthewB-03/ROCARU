using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the pause menu on screen and displays the cursor when paused
/// </summary>
public class scp_UI_manager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    Animator menuAnim;
    [SerializeField] menuStates menuState;
    enum menuStates
    {
        play,
        pause,
        menu
    }
    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip closeSound;
    [SerializeField] float volume;

    void PlaySound(AudioClip sound)
    {
        AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        audio.volume = volume;
        audio.clip = sound;
    }
    // Start is called before the first frame update
    void Start()
    {
        menuAnim = menu.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState)
        {
            case menuStates.play:
                PlayUpdate();
                break;
            case menuStates.pause:
                PauseUpdate();
                break;
            case menuStates.menu:
                MenuUpdate();
                break;
        }
    }

    // Menu invisible when not paused
    void PlayUpdate()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuAnim.SetBool("onScreen", false);
        //Time.timeScale = 1;

        if (Input.GetKeyDown(KeyCode.Tab) | Input.GetKeyDown(KeyCode.Escape))
        {
            menuState = menuStates.pause;
            PlaySound(openSound);
        }
    }

    // Menu visible when paused
    void PauseUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        menuAnim.SetBool("onScreen", true);
        //Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.Tab) | Input.GetKeyDown(KeyCode.Escape))
        {
            menuState = menuStates.play;
            PlaySound(closeSound);
        }
    }

    // Main menu
    void MenuUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
