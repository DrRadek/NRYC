using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject errorMessage;
    [SerializeField] AudioSource woodsSound;
    [SerializeField] Renderer endMaterialRenderer;
    Material endMaterial;
    [SerializeField] float fadeOutSpeed;

    void Start()
    {
        StartCoroutine(WaitForReboot());
        endMaterial = endMaterialRenderer.material;
    }

    bool fadeOut = false;

    private void Update()
    {
        if (fadeOut)
        {
            var a = endMaterial.color;
            a.a = Mathf.Max(0, a.a + Time.deltaTime * fadeOutSpeed);
            endMaterial.color = a;
        }
    }

    IEnumerator WaitForReboot()
    {
        yield return new WaitForSeconds(1.3f);
        ShowErrorMessage();
    }

    void ShowErrorMessage()
    {
        errorMessage.SetActive(true);
    }

    public void OnNewGameClicked()
    {
        if (fadeOut)
            return;

        GameManager.NewGame();
    }

    public void OnExitGameClicked()
    {
        if (fadeOut)
            return;

        StartCoroutine(EndGame());

    }

    private IEnumerator EndGame()
    {
        woodsSound.Play();
        fadeOut = true;
        yield return new WaitForSecondsRealtime(14);


        GameManager.EndGame();
    }
}
