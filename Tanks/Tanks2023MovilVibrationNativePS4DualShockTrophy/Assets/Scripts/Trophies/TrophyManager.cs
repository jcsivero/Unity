using UnityEngine;
using System.Collections;
using UnityEngine.PS4;
using System;

public class TrophyManager : MonoBehaviour
{
    //The active current user logged
    public PS4Input.LoggedInUser loggedInUser;

    // NPToolkit2 and Trophy initialization
    void Start()
    {
        //For this example we use Aync Handling of the NPToolkit2 operations
        Sony.NP.Main.OnAsyncEvent += Main_OnAsyncEvent;
        Sony.NP.InitToolkit init = new Sony.NP.InitToolkit();
        init.contentRestrictions.DefaultAgeRestriction = 0;

        //You can add several age restrictions by region
        Sony.NP.AgeRestriction[] ageRestrictions = new Sony.NP.AgeRestriction[1];
        ageRestrictions[0] = new Sony.NP.AgeRestriction(10, new Sony.NP.Core.CountryCode("us"));
        init.contentRestrictions.AgeRestrictions = ageRestrictions;

        // you can set affinity to other cores this way: Sony.NP.Affinity.Core2 | Sony.NP.Affinity.Core4;
        init.threadSettings.affinity = Sony.NP.Affinity.AllCores;
        init.SetPushNotificationsFlags(Sony.NP.PushNotificationsFlags.None);

        //For this example we use the first user
        loggedInUser = PS4Input.RefreshUsersDetails(0);

        try
        {
            //Initialize the NPToolkit2
            Sony.NP.Main.Initialize(init);
            //Register the Trophy Pack
            RegisterTrophyPack();
        }
        catch (Sony.NP.NpToolkitException e)
        {
            Debug.Log("Error initializing the NPToolkit2 : " + e.ExtendedMessage);
        }
    }

    public void RegisterTrophyPack()
    {
        try
        {
            Sony.NP.Trophies.RegisterTrophyPackRequest request = new Sony.NP.Trophies.RegisterTrophyPackRequest();

            request.UserId = loggedInUser.userId;

            Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

            // Make the async call which returns the Request Id 
            int requestId = Sony.NP.Trophies.RegisterTrophyPack(request, response);
            Debug.Log("RegisterTrophyPack Async : Request Id = " + requestId);
        }
        catch (Sony.NP.NpToolkitException e)
        {
            Debug.Log("Exception : " + e.ExtendedMessage);
        }
    }

    public void UnlockTrophy(int nextTrophyId)
    {
        try
        {
            Sony.NP.Trophies.UnlockTrophyRequest request = new Sony.NP.Trophies.UnlockTrophyRequest();

            request.TrophyId = nextTrophyId;

            request.UserId = loggedInUser.userId;

            Sony.NP.Core.EmptyResponse response = new Sony.NP.Core.EmptyResponse();

            // Make the async call which returns the Request Id 
            int requestId = Sony.NP.Trophies.UnlockTrophy(request, response);
            Debug.Log("GetUnlockedTrophies Async : Request Id = " + requestId);
        }
        catch (Sony.NP.NpToolkitException e)
        {
            Debug.Log("Exception : " + e.ExtendedMessage);
        }
    }

    // NOTE : This is called on the "Sony NP" thread and is independent of the Behaviour update.
    // This thread is created by the SonyNP.dll when NpToolkit2 is initialised.
    private void Main_OnAsyncEvent(Sony.NP.NpCallbackEvent callbackEvent)
    {
        //Print some useful info on the current event: 
        Debug.Log("Event: Service = (" + callbackEvent.Service + ") : API Called = (" + callbackEvent.ApiCalled + ") : Request Id = (" + callbackEvent.NpRequestId + ") : Calling User Id = (" + callbackEvent.UserId + ")");
        HandleAsyncEvent(callbackEvent);
    }

    //Here we manage the response of the previous request
    private void HandleAsyncEvent(Sony.NP.NpCallbackEvent callbackEvent)
    {
        try
        {
            if (callbackEvent.Response != null)
            {
                //We got an error response 
                if (callbackEvent.Response.ReturnCodeValue < 0)
                {
                    Debug.LogError("Response : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));
                }
                else
                {
                    //The callback of the event is a trophyUnlock event
                    if (callbackEvent.ApiCalled == Sony.NP.FunctionTypes.TrophyUnlock)
                    {
                        Debug.Log("Trophy Unlock : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));
                    }
                }
            }
        }
        catch (Sony.NP.NpToolkitException e)
        {
            Debug.Log("Main_OnAsyncEvent Exception = " + e.ExtendedMessage);
        }
    }
}