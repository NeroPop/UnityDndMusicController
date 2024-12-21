using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomisationInterface : MonoBehaviour
{
    [SerializeField] private GameObject CustomisationMenu;

    [Header("Custom Activities UI")]
    [SerializeField] private GameObject NewActivity;
    [SerializeField] private GameObject ActivityTitle;
    [SerializeField] private GameObject ActivityInput;
    [SerializeField] private GameObject ActivityButtonAudioImport;
    [SerializeField] private GameObject ActivityButtonDone;
    [SerializeField] private GameObject ActivityButtonCancel;

    [Header("Custom One-Shots UI")]
    [SerializeField] private GameObject NewOneshot;
    [SerializeField] private GameObject OneshotTitle;
    [SerializeField] private GameObject OneshotInput;
    [SerializeField] private GameObject OneshotButtonAudioImport;
    [SerializeField] private GameObject OneshotButtonDone;
    [SerializeField] private GameObject OneshotButtonCancel;

    [Header("Custom Ambience UI")]
    [SerializeField] private GameObject NewAmbient;
    [SerializeField] private GameObject AmbientTitle;
    [SerializeField] private GameObject AmbientInput;
    [SerializeField] private GameObject AmbientButtonAudioImport;
    [SerializeField] private GameObject AmbientButtonDone;
    [SerializeField] private GameObject AmbientButtonCancel;

    private void Start()
    {
        CustomisationMenu.SetActive(false);

        NewActivity.SetActive(false);
        ActivityTitle.SetActive(true);
        ActivityInput.SetActive(true);
        ActivityButtonAudioImport.SetActive(false);
        ActivityButtonDone.SetActive(false);
        ActivityButtonCancel.SetActive(true);

        NewOneshot.SetActive(false);
        OneshotTitle.SetActive(true);
        OneshotInput.SetActive(true);
        OneshotButtonAudioImport.SetActive(false);
        OneshotButtonDone.SetActive(false);
        OneshotButtonCancel.SetActive(true);

        NewAmbient.SetActive(false);
        AmbientTitle.SetActive(true);
        AmbientInput.SetActive(true);
        AmbientButtonAudioImport.SetActive(false);
        AmbientButtonDone.SetActive(false);
        AmbientButtonCancel.SetActive(true);
    }

    public void ResetCustomActivityUI()
    {
        ActivityInput.GetComponent<TMP_InputField>().text = "";

        CustomisationMenu.SetActive(false);
        NewActivity.SetActive(false);
        ActivityTitle.SetActive(true);
        ActivityInput.SetActive(true);
        ActivityButtonAudioImport.SetActive(false);
        ActivityButtonDone.SetActive(false);
        ActivityButtonCancel.SetActive(true);
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

    public void ResetCustomAmbienceUI()
    {
        AmbientInput.GetComponent<TMP_InputField>().text = "";

        CustomisationMenu.SetActive(false);
        NewAmbient.SetActive(false);
        AmbientTitle.SetActive(true);
        AmbientInput.SetActive(true);
        AmbientButtonAudioImport.SetActive(false);
        AmbientButtonDone.SetActive(false);
        AmbientButtonCancel.SetActive(true);
    }
}
