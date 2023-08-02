using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorScreen : MonoBehaviour
{
    [SerializeField] List<GameObject> screens = new List<GameObject>();
    [SerializeField] GameObject exit;
    public enum ScreenType
    {
        desktop = 0,
        camera = 1,
        cat = 2,
        credits = 3,
        store = 4,
        trash = 5,
    }

    ScreenType activeScreen = ScreenType.camera;

    public void SetScreenType(ScreenType screenType)
    {
        screens[(int)activeScreen].SetActive(false);
        activeScreen = screenType;
        screens[(int)activeScreen].SetActive(true);

        if ((int)activeScreen <= 1)
            exit.SetActive(false);
        else
            exit.SetActive(true);

    }

    public void LaunchCatApp()
    {
        SetScreenType(ScreenType.cat);
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

    public void ReturnToDesktop()
    {
        SetScreenType(ScreenType.desktop);
    }

}
