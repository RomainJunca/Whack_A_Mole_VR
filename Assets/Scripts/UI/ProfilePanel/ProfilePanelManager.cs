﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Class managing the profile panel. Does the transition between the UI panels (ProfileCreationManager, ProfileScrollViewManager) and the profile manager.
*/

public class ProfilePanelManager : MonoBehaviour
{
    private ProfileManager profileManager;
    private ProfileScrollViewManager profileScrollViewManager;
    private ProfileCreationManager profileCreationManager;
    private TherapistUi therapistUi;

    void Awake()
    {
        profileManager = FindObjectOfType<ProfileManager>();
        profileScrollViewManager = FindObjectOfType<ProfileScrollViewManager>();
        profileCreationManager = FindObjectOfType<ProfileCreationManager>();
        therapistUi = FindObjectOfType<TherapistUi>();
    }

    void Start()
    {
        RefreshDisplayedProfiles();
    }

    // Tells the ProfileManager to add a given profile for deletion.
    public bool ProfileRemoved(string id)
    {
        return profileManager.AddProfileForDeletion(id);
    }

    // Tells the ProfileManager to remove a given profile from deletion.
    public bool ProfileRestored(string id)
    {
        return profileManager.RemoveProfileFromDeletion(id);
    }

    // Tells the ProfileManager to select a given profile. Also tells the ProfileManager to delete the profiles stored for deletion and triggers
    // the transition between the profiles panel and the therapist panel.
    public void ProfileSelected(string id)
    {
        profileManager.SelectProfile(id);
        profileManager.ConfirmProfileDeletion();
        RefreshDisplayedProfiles();
        SwitchToTherapistPanel(profileManager.GetSelectedProfileProperties()["Name"]);
    }

    // Asks the ProfileManager to create a new profile. If the profile is created, tells the ProfileScrollViewManager to create a
    // new profile button accordingly.
    public bool ProfileCreated(string name, string mail, Dictionary<string, string> properties)
    {
        string createdId = profileManager.CreateProfile(name, mail, properties);
        if(createdId == null) return false;

        profileScrollViewManager.AddProfile(name, createdId);
        return true;
    }

    // Tells the ProfileScrollViewManager and the ProfileCreationManager to switch to "delete" mode.
    // Triggered by the DeleteProfileButton.
    public void SwitchToDeleteMode(bool deleteMode)
    {
        profileScrollViewManager.SwitchButtonsToDeleteMode(deleteMode);
        profileCreationManager.DisableInputs(deleteMode);
    }

    // Calls the TherapistUi to switch to the TherapistPanel.
    public void SwitchToTherapistPanel(string name)
    {
        therapistUi.SwitchPanel(PanelChoice.TherapistPanel, name);
        profileCreationManager.ClearFeedbackText();
    }

    // Tells the ProfileScrollViewManager to refresh the displayed profiles.
    private void RefreshDisplayedProfiles()
    {
        profileScrollViewManager.RefreshDisplayedProfiles(profileManager.GetAllProfilesNameId());
    }
}
