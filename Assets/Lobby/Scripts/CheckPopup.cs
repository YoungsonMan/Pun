using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckPopup : BaseUI
{
    [SerializeField] GameObject alreadyExistMsg;
    [SerializeField] GameObject availableAddressMsg;
    [SerializeField] GameObject availabilityOkButton;

    AuthError error = AuthError.EmailAlreadyInUse;
    void Start()
    {
        alreadyExistMsg = GetUI("AlreadyExistMsg");
        availableAddressMsg = GetUI("AvailableAddressMsg");
        availabilityOkButton = GetUI("AvailabilityOkButton");
        GetUI<Button>("AvailabilityOkButton").onClick.AddListener(Close);
    }
 
    void Update()
    {
        CheckAvailability();
        
    }
    public void CheckAvailability()
    {
        if (error == AuthError.EmailAlreadyInUse)
        {
            alreadyExistMsg.SetActive(true);
            availableAddressMsg.SetActive(false);
        }
        else
        {
            availableAddressMsg.SetActive(true);
            alreadyExistMsg.SetActive(false);
        }
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
