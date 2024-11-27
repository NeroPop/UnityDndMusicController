using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomisationInterface : MonoBehaviour
{
    [SerializeField] private GameObject CustomisationMenu;

    [Header("Custom One-Shots UI")]
    [SerializeField] private GameObject NewOneshot;
    [SerializeField] private GameObject OneshotTitle;
    [SerializeField] private GameObject OneshotInput;
    [SerializeField] private GameObject OneshotButtonAudioImport;
    [SerializeField] private GameObject OneshotButtonDone;
    [SerializeField] private GameObject OneshotButtonCancel;

    private void Start()
    {
        CustomisationMenu.SetActive(false);
        NewOneshot.SetActive(false);
        OneshotTitle.SetActive(true);
        OneshotInput.SetActive(true);
        OneshotButtonAudioImport.SetActive(false);
        OneshotButtonDone.SetActive(false);
        OneshotButtonCancel.SetActive(true);
    }

    public void ResetCustomOneshotUI()
    {
        OneshotInput.GetComponent<TMP_InputField>().text = "";

        CustomisationMenu.SetActive(false);
        NewOneshot.SetActive(false);
        OneshotTitle.SetActive(true);
        OneshotInput.SetActive(true);
        OneshotButtonAudioImport.SetActive(false);
        OneshotButtonDone.SetActive(false);
        OneshotButtonCancel.SetActive(true);
    }
}
