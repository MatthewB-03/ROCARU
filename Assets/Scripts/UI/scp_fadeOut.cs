using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Fades static in and out between scenes
/// </summary>
public class scp_fadeOut : MonoBehaviour
{
    float strength = 100;
    float speed = 150f;
    [SerializeField] Animator aStatic;
    int sceneToLoad;
    int direction = -1;


    [SerializeField] GameObject LoadText;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        aStatic.SetFloat("strength", strength);
    }

    // Update is called once per frame
    void Update()
    {
        LoadText.SetActive(false);
        aStatic.SetFloat("strength", strength);
        strength += Time.deltaTime/Time.timeScale * speed * direction;
        if (strength < 0)
        {
            direction *= -1;
            strength = 0;

            LoadText.SetActive(true);
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (strength > 101)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetScene(int scene)
    {
        sceneToLoad = scene;
    }
}
