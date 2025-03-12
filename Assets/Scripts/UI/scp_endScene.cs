using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Activates end scene text after a delay.
/// </summary>
public class scp_endScene : MonoBehaviour
{
    [SerializeField] int delay;
    [SerializeField] GameObject text;
    [SerializeField] scp_fadeOut fadeOut;
    bool canContinue = false;
    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
        StartCoroutine(StartDelay());
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(delay);
        text.SetActive(true);
        canContinue = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (canContinue & (Input.GetKey(KeyCode.Return) | Input.GetKey(KeyCode.KeypadEnter)))
        {
            fadeOut.SetScene(0);
            fadeOut.gameObject.SetActive(true);
        }
    }
}
