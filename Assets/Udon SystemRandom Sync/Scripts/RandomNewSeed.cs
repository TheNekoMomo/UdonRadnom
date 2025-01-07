
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RandomNewSeed : UdonSharpBehaviour
{
    [SerializeField] private UdonRandom udonRandom = null;

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, udonRandom.gameObject);

        int newSeed = (int)DateTime.Now.Ticks;
        udonRandom.SetSeed(newSeed);
    }
}
