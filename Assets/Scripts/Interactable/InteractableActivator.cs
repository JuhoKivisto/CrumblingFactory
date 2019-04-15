using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableActivator : MonoBehaviour
{

    public void Activate(InteractableType type, GameObject interactable)
    {

        switch (type)
        {

            case InteractableType.Button:
                ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<InteractableInfo>().objectiveInfo);
                break;
            case InteractableType.Valve:
                ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<InteractableInfo>().objectiveInfo);
                break;
        }

    }

    public void Activate(InteractableType type, LeverType lType, GameObject interactable)
    {
        switch (lType) {
            case LeverType.Normal:
                ObjectiveManager.instance.CompleteObjective(interactable.GetComponentInParent<InteractableInfo>().objectiveInfo);
                break;
            case LeverType.First:
                ObjectiveManager.instance.ActivateCrumbling();
                break;
            case LeverType.Reactor:
                ObjectiveManager.instance.CompleteReactorShutDown();
                break;            
        }
    }
}
