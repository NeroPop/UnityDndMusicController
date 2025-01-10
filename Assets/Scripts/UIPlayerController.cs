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

    private void Start()
    {
        if (ActivityPlayers.Count > 0)
        {
            foreach (GameObject player in ActivityPlayers)
            {
                player.GetComponent<UIActivitySetup>().LoadActivity();
            }
        }
    }
    public void ToggleActivity(int Act)
    {
        if (Act != PrevAct)
        {
            ActivityPlayers[Act].SetActive(true);
            DisabledPlayerControls.SetActive(false);
            ActSelected = true;
        }
        else if (Act == PrevAct)
        {
            ActivityPlayers[Act].SetActive(false);
            DisabledPlayerControls.SetActive(true);
            ActSelected = false;
        }

        PrevAct = Act;
    }

    public void ActivityOn()
    {
        ActivityPlayers[Act].SetActive(true);
        DisabledPlayerControls.SetActive(false);
        ActSelected = true;
    }

    public void ActivityOff(bool Inactive)
    {
        ActivityPlayers[Act].SetActive(false);
        ActSelected = false;
        if (Inactive)
        {
            DisabledPlayerControls.SetActive(true);
        }
    }
}
