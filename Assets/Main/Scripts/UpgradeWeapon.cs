using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeWeapon : MonoBehaviour
{
    public int index;

    public void UpgradeClicked()
    {
        GameManager.instance.shotgun.UpgradeAtIndex(index);
    } 
}
