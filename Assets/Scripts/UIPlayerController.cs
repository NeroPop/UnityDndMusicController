using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject DisabledPlayerControls;
    public List<GameObject> ActivityPlayers = new List<GameObject>();

    [Header("Variables")]
    private bool ActSelected = false;
    public int Act;
    private int PrevAct;

    private ActivityController ActivityController;

    public void ToggleActivity(int Act)
    {
        if (Act != PrevAct)
        {
            ActivityPlayers[Act].SetActive(true);
            DisabledPlayerControls.SetActive(false);
            ActSelected = true;
            ActivityController.ActSelected = ActSelected;
        }
        else if (Act == PrevAct)
        {
            ActivityPlayers[Act].SetActive(false);
            DisabledPlayerControls.SetActive(true);
            ActSelected = false;
            ActivityController.ActSelected = ActSelected;
        }

        PrevAct = Act;
    }

    public void ActivityOn()
    {
        ActivityPlayers[Act].SetActive(true);
        DisabledPlayerControls.SetActive(false);
        ActSelected = true;
        ActivityController.ActSelected = ActSelected;
    }

    public void ActivityOff(bool Inactive)
    {
        ActivityPlayers[Act].SetActive(false);
        ActSelected = false;
        ActivityController.ActSelected = ActSelected;
        if (Inactive)
        {
            DisabledPlayerControls.SetActive(true);
        }
    }
}
