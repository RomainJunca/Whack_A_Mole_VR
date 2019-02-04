using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Valve.VR;

public class PointingSystem : MonoBehaviour
{
    public enum AxisType
    {
        XAxis,
        ZAxis
    }
    public Color color;
    public float thickness = 0.002f;
    public AxisType facingAxis = AxisType.ZAxis;

    public LineRenderer lineRender;
    RaycastHit hitObject;

    Vector3 cursorScale = new Vector3(0.05f, 0.05f, 0.05f);
    float contactDistance = 0f;
    Transform contactTarget = null;


    public delegate void OnPressTrigger(Collider collider);
    public delegate void PointingAtMoleDeleg(Collider collider);
    public static event OnPressTrigger onPressTrigger;
    public static event PointingAtMoleDeleg isPointingAtMole;


    void Start()
    {
        SetupPointingSystem();
    }

    internal void SetupPointingSystem()
    {

        lineRender.SetPosition(0, transform.position);
        lineRender.SetPosition(1, transform.forward * 5);
        lineRender.startColor = color;
        lineRender.endColor = color;
        lineRender.widthMultiplier = thickness;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward * 5);
        Physics.Raycast(ray, out hitObject, 100f);

        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
            PressTrigger();

        if (hitObject.collider != null)
            if (hitObject.collider.gameObject.tag == "mole")
                IsPointingOnMole();
    }


    public void PressTrigger()
    {
        if (hitObject.collider != null)
        {
            if (onPressTrigger != null)
            {
                if (hitObject.collider.gameObject.tag == "mole")
                    onPressTrigger(hitObject.collider);
            }
        }
    }

    public void IsPointingOnMole()
    {
        isPointingAtMole(hitObject.collider);
    }
}