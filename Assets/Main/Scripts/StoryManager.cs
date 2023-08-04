using System.Collections;
using UnityEngine;
using TMPro;

public class StoryManager : MonoBehaviour
{
    [SerializeField] GameObject introNotification;
    [SerializeField] bool skipIntro = true;
    [SerializeField] bool skipFight = true;

    [SerializeField] TextMeshProUGUI goalText;
    [SerializeField] GameObject discordAppFirstScreen;
    [SerializeField] GameObject discordApp;
    [SerializeField] GameObject nextWaveButton;
    [SerializeField] GameObject cheatedEnding;
    [SerializeField] GameObject normalEnding;
    [SerializeField] NotificationTransition choiceTransition;

    [SerializeField] GameObject nonLinuxTaskBar;

    public enum Story
    {
        introStart,
        introInProgress,
        wave,
        end,
        afterEndChoice,
        afterEnd,
    }

    public bool gotLinux = false;


    private Story storyProgression = Story.introStart;

    public static StoryManager instance;

    public Story StoryProgression { get => storyProgression; private set => storyProgression = value; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;

    }

    private void Start()
    {
        if (skipIntro)
            storyProgression = Story.wave;

        if (skipFight)
            ActivateEndSequence();

        StartCoroutine(WaitForIntroActivation());
    }

    IEnumerator WaitForIntroActivation()
    {
        yield return new WaitForSeconds(8);

        if (storyProgression > Story.introStart)
            yield break;

        ActivateIntroSequence();
    }

    public void ActivateIntroSequence()
    {
        StoryProgression = Story.introInProgress;

        GameManager.instance.IsActivePopup = true;
        introNotification.SetActive(true);
    }

    public void ActivateWaveSequence()
    {
        StoryProgression = Story.wave;
    }

    public void ActivateEndSequence()
    {
        goalText.text = "Go to your pc, you will find a new app here";
        discordApp.SetActive(true);
        StoryProgression = Story.end;
    }

    public void ActivateAfterEndSequence()
    {
        goalText.gameObject.SetActive(false);
        discordApp.SetActive(false);
        nextWaveButton.SetActive(true);

        StoryProgression = Story.afterEnd;
    }

    public void DecideEnding()
    {
        if (StoryProgression == Story.afterEndChoice)
            return;

        StoryProgression = Story.afterEndChoice;

        if (gotLinux)
        {
            cheatedEnding.SetActive(true);
            choiceTransition.OnNotificationTransition();
        }
        else
        {
            normalEnding.SetActive(true);
        }

    }

    public void ReturnBackInStory()
    {
        StoryProgression = Story.end;
        discordAppFirstScreen.SetActive(true);
    }

    public void ActivateLinux()
    {
        gotLinux = true;
        nonLinuxTaskBar.SetActive(false);
    }

}
