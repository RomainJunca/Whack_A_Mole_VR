﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
An implementation of the Mole abstract class. Defines different parameters to modify and
overrides the actions to do on different events (enables, disables, popped...).
*/

public class DiskMole : Mole
{
    [SerializeField]
    private Color disabledColor;

    [SerializeField]
    private Color enabledColor;

    [SerializeField]
    private Color fakeEnabledColor;

    [SerializeField]
    private Color hoverColor;

    [SerializeField]
    private Color fakeHoverColor;

    [SerializeField]
    private Color popColor;

    [SerializeField]
    private AudioClip enableSound;

    [SerializeField]
    private AudioClip disableSound;

    [SerializeField]
    private AudioClip popSound;

    private Shader opaqueShader;
    private Shader glowShader;
    private Material meshMaterial;
    private AudioSource audioSource;
    private Animation animationPlayer;
    private Coroutine colorAnimation;
    private string playingClip = "";

    protected override void Start()
    {
        animationPlayer = gameObject.GetComponent<Animation>();
        meshMaterial = gameObject.GetComponentInChildren<Renderer>().material;
        opaqueShader = Shader.Find("Standard");
        glowShader = Shader.Find("Particles/Standard Unlit");
        audioSource = gameObject.GetComponent<AudioSource>();

        SwitchShader(false);
        PlayAnimation("EnableDisable");
        PlayTransitionColor(getAnimationDuration(), meshMaterial.color, disabledColor);

        base.Start();
    }

    public void EndPlayPop()
    {
        base.PlayPop();
    }

    /*
    Override of the event functions of the base class.
    */

    protected override void PlayEnabling()
    {
        PlaySound(enableSound);
        SwitchShader(false);
        PlayAnimation("EnableDisable");

        if (!fake)
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, enabledColor);
        }
        else
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, fakeEnabledColor);
        }
        base.PlayEnabling();
    }

    protected override void PlayDisabling()
    {
        PlaySound(enableSound);
        SwitchShader(false);
        PlayAnimation("EnableDisable");
        PlayTransitionColor(getAnimationDuration(), meshMaterial.color, disabledColor);
        base.PlayDisabling();
    }

    protected override void PlayHoverEnter() 
    {
        SwitchShader(true);
        PlayAnimation("HoverEnter");
        if (!fake)
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, hoverColor);
        }
        else
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, fakeHoverColor);
        }
    }

    protected override void PlayHoverLeave() 
    {
        SwitchShader(false);
        PlayAnimation("HoverExit");
        if (!fake)
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, enabledColor);
        }
        else
        {
            PlayTransitionColor(getAnimationDuration(), meshMaterial.color, fakeEnabledColor);
        }
    }

    protected override void PlayPop() 
    {
        SwitchShader(false);
        PlayAnimation("PopOscill");
        PlayTransitionColor(getAnimationDuration(), popColor, disabledColor);
        PlaySound(popSound);
    }

    protected override void PlayReset()
    {
        SwitchShader(false);
        PlayAnimation("EnableDisable");
        PlayTransitionColor(getAnimationDuration(), meshMaterial.color, disabledColor);
    }

    // Plays a sound.
    private void PlaySound(AudioClip audioClip)
    {
        if (!audioSource)
        {
            return;
        }
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    // Plays an animation clip.
    private void PlayAnimation(string animationName)
    {
        playingClip = animationName;
        animationPlayer.Play(animationName);
    }

    // Returns the duration of the currently playing animation clip.
    private float getAnimationDuration()
    {
        return animationPlayer.GetClip(playingClip).length;
    }

    // Sets up the TransitionColor coroutine to smoothly transition between two colors.
    private void PlayTransitionColor(float duration, Color startColor, Color endColor)
    {
        if (colorAnimation != null) StopCoroutine(colorAnimation);
        colorAnimation = StartCoroutine(TransitionColor(duration, startColor, endColor));
    }

    // Changes the color of the mesh.
    private void ChangeColor(Color color)
    {
        meshMaterial.color = color;
    }

    // Switches between the glowing and standard shader.
    private void SwitchShader(bool glow = false)
    {
        if (glow)
        {
            if (meshMaterial.shader.name == glowShader.name) return;
            meshMaterial.shader = glowShader;
        }
        else
        {
            if (meshMaterial.shader.name == opaqueShader.name) return;
            meshMaterial.shader = opaqueShader;
        }
    }

    // Ease function, Quart ratio.
    private float EaseQuartOut (float k) 
    {
        return 1f - ((k -= 1f)*k*k*k);
    }

    private IEnumerator TransitionColor(float duration, Color startColor, Color endColor)
    {
        float durationLeft = duration;
        float totalDuration = duration;

        // Generation of a color gradient from the start color to the end color.
        Gradient colorGradient = new Gradient();
        GradientColorKey[] colorKey = new GradientColorKey[2]{new GradientColorKey(startColor, 0f), new GradientColorKey(endColor, 1f)};
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2]{new GradientAlphaKey(startColor.a, 0f), new GradientAlphaKey(endColor.a, 1f)};
        colorGradient.SetKeys(colorKey, alphaKey);

        // Playing of the animation. The DiskMole color is interpolated following the easing curve
        while (durationLeft > 0f)
        {
            float timeRatio = (totalDuration - durationLeft) / totalDuration;

            ChangeColor(colorGradient.Evaluate(EaseQuartOut(timeRatio)));
            durationLeft -= Time.deltaTime;

            yield return null;
        }

        // When the animation is finished, resets the color to its end value.
        ChangeColor(endColor);
    }
}
