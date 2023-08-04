using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonitorScreen;

public class MonitorScreen : MonoBehaviour
{
    [SerializeField] List<GameObject> screens = new();
    [SerializeField] GameObject exit;
    [SerializeField] GameObject discordFirstMessage;
    [SerializeField] GameObject taskBar;
    public enum ScreenType
    {
        desktop,
        camera,
        discord,
        trash,
        cat,
        credits,
        store,
        audio,
    }

    public ScreenType activeScreen = ScreenType.camera;

    public void SetScreenType(ScreenType screenType)
    {
        if (GameManager.instance.IsActivePopup)
            return;

        screens[(int)activeScreen].SetActive(false);
        activeScreen = screenType;
        screens[(int)activeScreen].SetActive(true);

        if ((int)activeScreen <= 3)
            exit.SetActive(false);
        else
            exit.SetActive(true);

        if (screenType == ScreenType.desktop || screenType == ScreenType.discord)
            taskBar.SetActive(true);
        else
            taskBar.SetActive(false);

    }

    public void LaunchCatApp()
    {
        SetScreenType(ScreenType.cat);
        AudioManager.instance.PlayPurr();
    }

    public void LaunchCreditsApp()
    {
        SetScreenType(ScreenType.credits);
    }

    public void LaunchStoreApp()
    {
        SetScreenType(ScreenType.store);
    }

    public void LaunchTrashApp()
    {
        SetScreenType(ScreenType.trash);
    }

    public void LaunchDiscordApp()
    {
        SetScreenType(ScreenType.discord);
        discordFirstMessage.SetActive(true);
    }

    public void LaunchAudioApp()
    {
        SetScreenType(ScreenType.audio);
    }


    public void ReturnToDesktop()
    {
        if (activeScreen == ScreenType.cat && StoryManager.instance.StoryProgression >= StoryManager.Story.wave)
            AudioManager.instance.StopPurr();

        if (StoryManager.instance.StoryProgression <= StoryManager.Story.introStart)
            StoryManager.instance.ActivateIntroSequence();

        SetScreenType(ScreenType.desktop);
    }

}
