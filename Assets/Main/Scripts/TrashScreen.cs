using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScreen : MonoBehaviour
{
    [SerializeField] GameObject choise;
    [SerializeField] GameObject exitButton;

    private void OnEnable()
    {
        if (StoryManager.instance.gotLinux)
        {
            StartCoroutine(WaitNextFrame());
        }
        else
        {
            StartCoroutine(WaitAfterEnabled());
        }
    }

    private IEnumerator WaitNextFrame()
    {
        yield return new WaitForEndOfFrame();
        exitButton.SetActive(true);
    }

    private IEnumerator WaitAfterEnabled()
    {
        yield return new WaitForSeconds(0.3f);
        choise.SetActive(true);
    }


    private void OnDisable()
    {
        choise.SetActive(false);
    }
}
