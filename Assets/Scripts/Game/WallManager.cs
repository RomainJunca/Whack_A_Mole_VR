﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
Spawns, references and activates the moles. Is the only component to directly interact with the moles.
*/

public class WallInfo {
    public bool active = false;
    public Dictionary<int, Mole> moles;
    public Vector3 wallSize;
    public Vector3 wallCenter;
    public float highestX = -1f;
    public float highestY = -1f;
    public float lowestX = -1f;
    public float lowestY = -1f;
    public float lowestZ = -1f;
    public float highestZ = -1f;
    public float heightOffset;
}

public class WallManager : MonoBehaviour
{
    // The Mole object to be loaded
    [SerializeField]
    private Mole moleObject;

    // The count of rows to generate
    [SerializeField]
    private int rowCount;

    // The count of columns to generate
    [SerializeField]
    private int columnCount;

    // Offest of the height of the wall
    [SerializeField]
    private float heightOffset;

    // The size of the wall
    [SerializeField]
    private Vector3 wallSize;

    // Coefficient of the X curvature of the wall. 1 = PI/2, 0 = straight line
    [SerializeField]
    [Range(0.1f, 1f)]
    private float xCurveRatio = 1f;

    // Coefficient of the Y curvature of the wall. 1 = PI/2, 0 = straight line
    [SerializeField]
    [Range(0.1f, 1f)]
    private float yCurveRatio = 1f;

    // The angle of the edge moles if a curve ratio of 1 is given
    [SerializeField]
    [Range(0f, 90f)]
    private float maxAngle = 90f;

    // The scale of the Mole. Idealy shouldn't be scaled on the Z axis (to preserve the animations)
    [SerializeField]
    private Vector3 moleScale = Vector3.one;

    [System.Serializable]
    public class StateUpdateEvent : UnityEvent<WallInfo> { }
    public StateUpdateEvent stateUpdateEvent;

    private WallGenerator wallGenerator;
    private Vector3 wallCenter;
    private Dictionary<int, Mole> moles;
    private bool active = false;
    private bool isInit = false;
    private float updateCooldownDuration = .1f;
    private LoggerNotifier loggerNotifier;
    private int moleCount = 0;

    // Wall boundaries
    private float highestX = -1f;
    private float highestY = -1f;
    private float lowestX = -1f;
    private float lowestY = -1f;
    private float lowestZ = -1f;
    private float highestZ = -1f;

    void Start()
    {
        // Initialization of the LoggerNotifier.
        loggerNotifier = new LoggerNotifier(persistentEventsHeadersDefaults: new Dictionary<string, string>(){
            {"WallRowCount", "NULL"},
            {"WallColumnCount", "NULL"},
            {"WallSizeX", "NULL"},
            {"WallSizeY", "NULL"},
            {"WallSizeZ", "NULL"},
            {"WallCurveRatioX", "NULL"},
            {"WallCurveRatioY", "NULL"}
        });

        loggerNotifier.InitPersistentEventParameters(new Dictionary<string, object>(){
            {"WallRowCount", rowCount},
            {"WallColumnCount", columnCount},
            {"WallSizeX", wallSize.x},
            {"WallSizeY", wallSize.y},
            {"WallSizeZ", wallSize.z},
            {"WallCurveRatioX", xCurveRatio},
            {"WallCurveRatioY", yCurveRatio}
        });

        moles = new Dictionary<int, Mole>();
        wallGenerator = gameObject.GetComponent<WallGenerator>();
        wallCenter = new Vector3(wallSize.x/2f, wallSize.y/2f, 0);
        isInit = true;
    }

    private void UpdateWallLogs() {
        loggerNotifier.InitPersistentEventParameters(new Dictionary<string, object>(){
            {"WallRowCount", rowCount},
            {"WallColumnCount", columnCount},
            {"WallSizeX", wallSize.x},
            {"WallSizeY", wallSize.y},
            {"WallSizeZ", wallSize.z},
            {"WallCurveRatioX", xCurveRatio},
            {"WallCurveRatioY", yCurveRatio}
        });
    }

    void OnValidate()
    {
        UpdateWall();
    }

    public void Enable()
    {
        active = true;

        if (moles.Count == 0)
        {
            GenerateWall();
            UpdateWallLogs();
        }
    }

    public void Disable()
    {
        active = false;
        disableMoles();
    }

    public void Clear()
    {
        active = false;
        DestroyWall();
        var wallInfo = CreateWallInfo();
        stateUpdateEvent.Invoke(wallInfo);
    }

    private WallInfo CreateWallInfo() {
        var wallInfo = new WallInfo();
        wallInfo.active = active;
        wallInfo.moles = moles;
        wallInfo.wallSize = wallSize;
        wallInfo.wallCenter = wallCenter;
        wallInfo.heightOffset = heightOffset;
        wallInfo.highestX = highestX;
        wallInfo.highestY = highestY;
        wallInfo.lowestX = lowestX;
        wallInfo.lowestY = lowestY;
        wallInfo.lowestZ = lowestZ;
        wallInfo.highestZ = highestZ;
        return wallInfo;
    }

    // Activates a random Mole for a given lifeTime and set if is fake or not
    public void ActivateRandomMole(float lifeTime, float moleExpiringDuration, bool isFake)
    {
        if (!active) return;

        GetRandomMole().Enable(lifeTime, moleExpiringDuration, isFake, moleCount);
        moleCount++;
    }

    // Activates a specific Mole for a given lifeTime and set if is fake or not
    public void ActivateMole(int moleId, float lifeTime, float moleExpiringDuration, bool isFake)
    {
        if (!active) return;
        if (!moles.ContainsKey(moleId)) return;
        moles[moleId].Enable(lifeTime, moleExpiringDuration, isFake, moleCount);
        moleCount++;
    }

    // Pauses/unpauses the moles
    public void SetPauseMole(bool pause)
    {
        foreach(Mole mole in moles.Values)
        {
            mole.SetPause(pause);
        }
    }

    public void UpdateMoleCount(int newRowCount = -1, int newColumnCount = -1)
    {
        if (newRowCount >= 2) rowCount = newRowCount;
        if (newColumnCount >= 2) columnCount = newColumnCount;
        // UpdateWall();
    }

    public void UpdateWallSize(float newWallSizeX = -1, float newWallSizeY = -1, float newWallSizeZ = -1)
    {
        if (newWallSizeX >= 0) wallSize.x = newWallSizeX;
        if (newWallSizeY >= 0) wallSize.y = newWallSizeY;
        if (newWallSizeZ >= 0) wallSize.z = newWallSizeZ;
        // UpdateWall();
    }

    public void UpdateWallCurveRatio(float newCurveRatioX = -1, float newCurveRatioY = -1)
    {
        if (newCurveRatioX >= 0 && newCurveRatioX <= 1 ) xCurveRatio = newCurveRatioX;
        if (newCurveRatioY >= 0 && newCurveRatioY <= 1 ) yCurveRatio = newCurveRatioY;
        // UpdateWall();
    }

    public void UpdateWallMaxAngle(float newMaxAngle)
    {
        if (newMaxAngle >= 0 && newMaxAngle <= 90 ) maxAngle = newMaxAngle;
        // UpdateWall();
    }

    public void UpdateMoleScale(float newMoleScaleX = -1, float newMoleScaleY = -1, float newMoleScaleZ = -1)
    {
        if (newMoleScaleX >= 0) moleScale.x = newMoleScaleX;
        if (newMoleScaleY >= 0) moleScale.y = newMoleScaleY;
        if (newMoleScaleZ >= 0) moleScale.z = newMoleScaleZ;
        // UpdateWall();
    }

    public UnityEvent<WallInfo> GetUpdateEvent()
    {
        return stateUpdateEvent;
    }

    // Returns a random, inactive Mole. Can block the game if no Mole can be found. May need to be put in a coroutine.
    private Mole GetRandomMole()
    {
        Mole mole;
        Mole[] tempMolesList = new Mole[moles.Count];
        moles.Values.CopyTo(tempMolesList, 0);
        do
        {
            mole = tempMolesList[Random.Range(0, moles.Count)];
        }
        while (!mole.CanBeActivated());
        return mole;
    }

    private void disableMoles()
    {
        foreach(Mole mole in moles.Values)
        {
            mole.Reset();
        }
    }

    private void DestroyWall()
    {
        foreach(Mole mole in moles.Values)
        {
            Destroy(mole.gameObject);
        }
        moles.Clear();
        moleCount = 0;
    }

    // Generates the wall of Moles
    private void GenerateWall()
    {
        wallGenerator.InitPointsLists(columnCount, rowCount);
        // Updates the wallCenter value
        wallCenter = new Vector3(wallSize.x/2f, wallSize.y/2f, 0);

        highestX = -1f;
        highestY = -1f;
        lowestX = -1f;
        lowestY = -1f;
        lowestZ = -1f;
        highestZ = -1f;

        // For each row and column:
        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                if((x == 0 || x == columnCount - 1) && (y == rowCount - 1 || y == 0))
                {
                    wallGenerator.AddPoint(x, y, DefineMolePos(x, y), DefineMoleRotation(x, y));
                    continue;
                }

                // Instanciates a Mole object
                Mole mole = Instantiate(moleObject, transform);
                // Get the Mole object's local position depending on the current row, column and the curve coefficient
                Vector3 molePos = DefineMolePos(x, y);

                // Sets the Mole local position, rotates it so it looks away from the wall (affected by the curve)
                mole.transform.localPosition = molePos;
                mole.transform.localRotation = DefineMoleRotation(x, y);
                // Sets the Mole ID, scale and references it
                int moleId = GetMoleId(x, y);
                mole.SetId(moleId);
                mole.SetNormalizedIndex(GetnormalizedIndex(x, y));
                mole.transform.localScale = moleScale;
                moles.Add(moleId, mole);

                wallGenerator.AddPoint(x, y, molePos, mole.transform.localRotation);

                // Check's the mole's position to find the outer boundaries of the all moles.
                if (highestX == -1f) highestX = mole.transform.position.x;
                if (lowestX == -1f) lowestX = mole.transform.position.x;
                if (highestY == -1f) highestY = mole.transform.position.y;
                if(lowestY == -1f) lowestY = mole.transform.position.y;
                if(lowestZ == -1f) lowestZ = mole.transform.position.z;
                if(highestZ == -1f) highestZ = mole.transform.position.z;

                highestX = mole.transform.position.x > highestX ? mole.transform.position.x : highestX;
                lowestX = mole.transform.position.x < lowestX ? mole.transform.position.x : lowestX;
                highestY = mole.transform.position.y > highestY ? mole.transform.position.y : highestY;
                lowestY = mole.transform.position.y < lowestY ? mole.transform.position.y : lowestY;
                lowestZ = mole.transform.position.z < lowestZ ? mole.transform.position.z : lowestZ;
                highestZ = mole.transform.position.z < highestZ ? mole.transform.position.z : highestZ;
            }
        }
        //stateUpdateEvent.Invoke(true, moles);
        var wallInfo = CreateWallInfo();
        stateUpdateEvent.Invoke(wallInfo);
        wallGenerator.GenerateWall();
    }

    // Updates the wall
    private void UpdateWall()
    {
        if (!(active && isInit)) return;
        StopAllCoroutines();
        StartCoroutine(WallUpdateCooldown());
    }

    // Gets the Mole position depending on its index, the wall size (x and y axes of the vector3), and also on the curve coefficient (for the z axis).
    private Vector3 DefineMolePos(int xIndex, int yIndex)
    {
        float angleX = ((((float)xIndex/(columnCount - 1)) * 2) - 1) * ((Mathf.PI * xCurveRatio) / 2);
        float angleY = ((((float)yIndex/(rowCount - 1)) * 2) - 1) * ((Mathf.PI * yCurveRatio) / 2);

        return new Vector3(Mathf.Sin(angleX) * (wallSize.x / (2 * xCurveRatio)), Mathf.Sin(angleY) * (wallSize.y / (2 * yCurveRatio)), ((Mathf.Cos(angleY) * (wallSize.z)) + (Mathf.Cos(angleX) * (wallSize.z))));
    }

    private int GetMoleId(int xIndex, int yIndex)
    {
        return ((xIndex + 1) * 100) + (yIndex + 1);
    }

    private Vector2 GetnormalizedIndex(int xIndex, int yIndex)
    {
        return (new Vector2((float)xIndex / (columnCount - 1), (float)yIndex / (rowCount - 1)));
    }

    // Gets the Mole rotation so it is always looking away from the wall, depending on its X local position and the wall's curvature (curveCoeff)
    private Quaternion DefineMoleRotation(int xIndex, int yIndex)
    {
        Quaternion lookAngle = new Quaternion();
        lookAngle.eulerAngles = new Vector3(-((((float)yIndex/(rowCount - 1)) * 2) - 1) * (maxAngle * yCurveRatio), ((((float)xIndex/(columnCount - 1)) * 2) - 1) * (maxAngle * xCurveRatio), 0f);
        return lookAngle;
    }

    private IEnumerator WallUpdateCooldown()
    {
        yield return new WaitForSeconds(updateCooldownDuration);

        if(active)
        {
            Clear();
            Enable();
        }
    }
}
