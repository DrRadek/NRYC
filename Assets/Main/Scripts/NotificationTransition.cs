using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationTransition : MonoBehaviour
{
    [SerializeField] GameObject firstNotification;
    [SerializeField] GameObject secondNotification;

    public void OnNotificationTransition()
    {
        firstNotification.SetActive(false);

        if(secondNotification != null)
            secondNotification.SetActive(true);
        else
            GameManager.instance.IsActivePopup = false;
    }
}
