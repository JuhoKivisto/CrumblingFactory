using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPanelInfo : MonoBehaviour {

    public int id;

    public ControlPanel controlPanelInfo;

    private void Awake()
    {
        controlPanelInfo = new ControlPanel(id);
        ObjectiveManager.instance.InitControlPanel(controlPanelInfo);
    }


}
