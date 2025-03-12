using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Saves and loads the player and level objectives 
/// to playerPrefs as comma seperated strings
/// </summary>
public class scp_saveManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerHolo;

    [SerializeField] scp_AICounter enemyScript;

    GameObject[] enemies;
    [SerializeField] GameObject[] repairs;

    bool load = false;

    [SerializeField] GameObject audioPrefab;
    [SerializeField] AudioClip yesSound;
    [SerializeField] AudioClip noSound;

    void PlaySound(AudioClip sound, float volume)
    {
        if ((audioPrefab != null) & (sound != null))
        {
            AudioSource audio = Instantiate(audioPrefab, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            audio.volume = volume;
            audio.clip = sound;
        }
    }
    // Start is called before the first frame update
    public void LoadScene()
    {
        int index = PlayerPrefs.GetInt("BI");
        if (index >= 2)
        {
            PlaySound(yesSound, 0.5f);
            DontDestroyOnLoad(this.gameObject);
            load = true;
            SceneManager.LoadScene(index);
        }
        else
        {
            PlaySound(noSound, 0.5f);
        }
        //SceneManager.LoadScene(PlayerPrefs.GetInt("BI"));
    }

    void OnLevelWasLoaded(int level)
    {
        if (load == true)
        {
            scp_robotDog dogScript = GameObject.Find("RoboGameObject").GetComponent<scp_robotDog>();
            player = dogScript.dogAnim.gameObject;
            playerHolo = dogScript.holoDog;



            string[] text = PlayerPrefs.GetString("PL").Split(" ");

            Debug.Log(PlayerPrefs.GetString("PL"));
            Debug.Log(text[0]);

            player.transform.position = new Vector3(float.Parse(text[0]), float.Parse(text[1]), float.Parse(text[2]));
            player.transform.eulerAngles = new Vector3(float.Parse(text[3]), float.Parse(text[4]), float.Parse(text[5]));

            Debug.Log(player.transform.position);

            playerHolo.transform.parent.position = new Vector3(float.Parse(text[0]), float.Parse(text[1]), float.Parse(text[2]));
            playerHolo.transform.parent.eulerAngles = new Vector3(float.Parse(text[3]), float.Parse(text[4]) + 180f, float.Parse(text[5]));
            playerHolo.transform.localPosition = Vector3.zero;

            text = PlayerPrefs.GetString("EN").Split("\n");
            foreach (string enemyTxt in text)
            {
                string[] text2 = enemyTxt.Split(",");
                GameObject enemy = GameObject.Find(text2[0]);
                if (enemy != null)
                {
                    Vector3 parentPos = new Vector3(float.Parse(text2[1]), float.Parse(text2[2]), float.Parse(text2[3]));
                    Vector3 parentRot = new Vector3(float.Parse(text2[4]), float.Parse(text2[5]), float.Parse(text2[6]));
                    enemy.GetComponent<scp_enemyScript>().SetSaveValues(text2[7], text2[8], text2[9], parentPos, parentRot);
                }

            }

            text = PlayerPrefs.GetString("RP").Split("\n");
            foreach (string repairTxt in text)
            {
                Debug.Log(repairTxt);
                string[] text2 = repairTxt.Split(",");
                Debug.Log(text2[0]);
                if (text2[0] != "")
                {
                    Debug.Log(text2[0]);
                    scp_repairScript repair = GameObject.Find(text2[0]).GetComponent<scp_repairScript>();
                    if (repair != null)
                    {
                        Debug.Log(text2[1]);
                        if (text2[1] == "True" & repair.broken != true)
                        {
                            repair.broken = true;
                            if (repair.type == scp_repairScript.repairTypes.booster)
                            {
                                scp_signalBooster boosterScript = repair.gameObject.GetComponent<scp_signalBooster>();
                                if (boosterScript != null)
                                {
                                    boosterScript.active = false;
                                }
                            }
                        }
                        else if (text2[1] == "False" & repair.broken == true)
                        {

                            repair.broken = false;
                            repair.ShowFixedSprite();
                            if (repair.type == scp_repairScript.repairTypes.booster)
                            {
                                scp_signalBooster boosterScript = repair.gameObject.GetComponent<scp_signalBooster>();
                                if (boosterScript != null)
                                {
                                    boosterScript.active = true;
                                }
                            }
                        }
                    }
                }
            }

            Destroy(this.gameObject);
        }
        
    }
    public void SaveGame()
    {
        enemies = enemyScript.enemies; 
        PlayerPrefs.SetInt("BI", SceneManager.GetActiveScene().buildIndex); // BuildIndex
        PlayerPrefs.SetString("PL", PlayerTransform()); // PlayerTransform
        PlayerPrefs.SetString("EN", Enemies()); // Enemies
        PlayerPrefs.SetString("RP", Repairs()); // Repairs
        PlaySound(yesSound, 0.5f);
        //Debug.Log(PlayerPrefs.GetString("RP"));
    }

    string PlayerTransform()
    {
        string text = player.transform.position.x.ToString();
        text = text + " " + player.transform.position.y.ToString();
        text = text + " " + player.transform.position.z.ToString();
        text = text + " " + player.transform.eulerAngles.x.ToString();
        text = text + " " + player.transform.eulerAngles.y.ToString();
        text = text + " " + player.transform.eulerAngles.z.ToString();

        return text;
    }

    string Enemies()
    {
        string text = "";
        foreach (GameObject enemy in enemies)
        {
            scp_enemyScript enemyScript = enemy.GetComponent<scp_enemyScript>();
            text = text + enemy.name.ToString();
            /*
            text = text + "," + enemy.transform.position.x.ToString();
            text = text + "," + enemy.transform.position.y.ToString();
            text = text + "," + enemy.transform.position.z.ToString();
            text = text + "," + enemy.transform.eulerAngles.x.ToString();
            text = text + "," + enemy.transform.eulerAngles.y.ToString();
            text = text + "," + enemy.transform.eulerAngles.z.ToString();
            */
            text = text + enemy.GetComponent<scp_enemyScript>().GetSaveValues() + "\n";
        }
        return text;
    }

    string Repairs()
    {
        string text = "";
        foreach (GameObject repair in repairs)
        {
            text = text + repair.name.ToString();
            text = text + "," + repair.GetComponent<scp_repairScript>().broken.ToString() + "\n";
        }
        return text;
    }
}
