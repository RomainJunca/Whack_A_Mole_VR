﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

/*
Class doing the interface between the PatternPlayer and the different classes of the game it needs to interact with.
Reads and parses a dictionary of actions to do and calls the different game classes in consequence.
The dictionary has a mandatory "FUNCTION" key containing the action to do, then other keys corresponding to the arguments of the given action.
*/
public class PatternInterface : MonoBehaviour
{
    private PatternPlayer patternPlayer;
    private WallManager wallManager;
    private ModifiersManager modifiersManager;
    private GameDirector gameDirector;
    private float randVar = 0f;

    void Awake()
    {
        patternPlayer = FindObjectOfType<PatternPlayer>();
        wallManager = FindObjectOfType<WallManager>();
        modifiersManager = FindObjectOfType<ModifiersManager>();
        gameDirector = FindObjectOfType<GameDirector>();
    }

    // Plays an action
    public void PlayAction(Dictionary<string, string> action)
    {
        string[] keys = new string[action.Keys.Count];
        action.Keys.CopyTo(keys, 0);

        // If one of the argument is "RAND", replaces it with the random variable.
        foreach(string key in keys)
        {
            if (action[key] == "RAND") action[key] = randVar.ToString();
        }

        // Matches the "FUNCTION" key, corresponding to the action to do.
        switch(action["FUNCTION"])
        {
                        
            case "START":
                CallPlay();
                break;
            
            case "STOP":
                CallStop();
                break;
            
            case "WALL":
                try
                {
                    SetWall(action);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in WALL: " + e.Message);
                }
                break;
            
            case "MOLE":
                try
                {
                    SetMole(action["X"], action["Y"], action["LIFETIME"]);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in MOLE: " + e.Message);
                }
                break;
            
            case "DISTRACTOR":
                try
                {
                    SetDistractor(action["X"], action["Y"], action["LIFETIME"]);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in MOLE: " + e.Message);
                }
                break;
            
            case "DIFFICULTY":
                try
                {
                    SetDifficulty(action["SPEED"]);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in DIFFICULTY: " + e.Message);
                }
                break;
            
            case "MODIFIER":
                try
                {
                    SetModifier(action);
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in MODIFIER: " + e.Message);
                }
                break;
            
            case "RANDGEN":
                try
                {
                    RegenRand(ParseFloat(action["STARTVALUE"]), ParseFloat(action["ENDVALUE"]), bool.Parse(action["ISINT"]));
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Error in RANDGEN: " + e.Message);
                }
                break;
        }
    }

    // Calls the GameDirector to start the game
    public void CallPlay()
    {
        gameDirector.StartGame();
    }

    // Calls the GameDirector to stop the game
    public void CallStop()
    {
        gameDirector.StopGame();
    }

    // Updates one or multiple Wall's properties
    private void SetWall(Dictionary<string, string> action)
    {
        string tempValue;

        wallManager.Clear();

        if (action.TryGetValue("ROW", out tempValue))
        {
            wallManager.UpdateMoleCount(newRowCount: int.Parse(tempValue));
        }
        if (action.TryGetValue("COL", out tempValue))
        {
            wallManager.UpdateMoleCount(newColumnCount: int.Parse(tempValue));
        }
        if (action.TryGetValue("SIZEX", out tempValue))
        {
            wallManager.UpdateWallSize(newWallSizeX: ParseFloat(tempValue));
        }
        if (action.TryGetValue("SIZEY", out tempValue))
        {
            wallManager.UpdateWallSize(newWallSizeY: ParseFloat(tempValue));
        }
        if (action.TryGetValue("SIZEZ", out tempValue))
        {
            wallManager.UpdateWallSize(newWallSizeZ: ParseFloat(tempValue));
        }
        if (action.TryGetValue("CURVEX", out tempValue))
        {
            wallManager.UpdateWallCurveRatio(newCurveRatioX: ParseFloat(tempValue));
        }
        if (action.TryGetValue("CURVEY", out tempValue))
        {
            wallManager.UpdateWallCurveRatio(newCurveRatioY: ParseFloat(tempValue));
        }
        if (action.TryGetValue("MAXANGLE", out tempValue))
        {
            wallManager.UpdateWallMaxAngle(ParseFloat(tempValue));
        }
        if (action.TryGetValue("MOLESCALEX", out tempValue))
        {
            wallManager.UpdateMoleScale(newMoleScaleX: ParseFloat(tempValue));
        }
        if (action.TryGetValue("MOLESCALEY", out tempValue))
        {
            wallManager.UpdateMoleScale(newMoleScaleY: ParseFloat(tempValue));
        }
        if (action.TryGetValue("MOLESCALEZ", out tempValue))
        {
            wallManager.UpdateMoleScale(newMoleScaleZ: ParseFloat(tempValue));
        }
        wallManager.Enable();
    }

    // Spawns a Mole
    private void SetMole(string xIndex, string yIndex, string lifeTime)
    {
        int moleId = ((int.Parse(xIndex)) * 100) + (int.Parse(yIndex));
        wallManager.ActivateMole(moleId, ParseFloat(lifeTime), gameDirector.GetMoleExpiringDuration(), false);
    }

    // Spawns a distractor (fake Mole)
    private void SetDistractor(string xIndex, string yIndex, string lifeTime)
    {
        int moleId = ((int.Parse(xIndex)) * 100) + (int.Parse(yIndex));
        wallManager.ActivateMole(moleId, ParseFloat(lifeTime), gameDirector.GetMoleExpiringDuration(), true);
    }

    // Updates the game difficulty
    private void SetDifficulty(string difficulty)
    {
        gameDirector.SetDifficulty(difficulty);
    }

    // Updates one or multiple modifiers
    private void SetModifier(Dictionary<string, string> action)
    {
        string tempValue;
        
        if (action.TryGetValue("EYEPATCH", out tempValue))
        {
            modifiersManager.SetEyePatch((ModifiersManager.EyePatch)System.Enum.Parse( typeof(ModifiersManager.EyePatch), tempValue));
        }
        if (action.TryGetValue("MIRROR", out tempValue))
        {
            modifiersManager.SetMirrorEffect(bool.Parse(tempValue));
        }
        if (action.TryGetValue("DUALTASK", out tempValue))
        {
            modifiersManager.SetDualTask(bool.Parse(tempValue));
        }
        if (action.TryGetValue("CONTROLLEROFFSET", out tempValue))
        {
            modifiersManager.SetControllerOffset(ParseFloat(tempValue));
        }
        if (action.TryGetValue("MAINCONTROLLER", out tempValue))
        {
            modifiersManager.SetMainController(bool.Parse(tempValue));
        }
    }

    // Regenerates the random variable
    private void RegenRand(float startValue, float endValue, bool isInt)
    {
        randVar = Random.Range(startValue, endValue);
        if (isInt) randVar = Mathf.Round(randVar);
    }

    // Parses a string into a float
    private float ParseFloat(string value)
    {
        return float.Parse(value, CultureInfo.InvariantCulture);
    }
}
