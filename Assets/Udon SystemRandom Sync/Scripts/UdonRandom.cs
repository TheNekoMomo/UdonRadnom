using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

/// <summary>
/// Uses the System.random code and syncs the random numbers to everyone
/// </summary>
[AddComponentMenu("Udon Sharp/Utilities/UdonRandom")]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class UdonRandom : UdonSharpBehaviour
{
    [Tooltip("The master will generate a random seed on start")]
    [SerializeField] private bool generateSeedOnStart = true;
    [Space]
    [UdonSynced] public int Seed = 0;

    [HideInInspector, UdonSynced] public int inext;
    [HideInInspector, UdonSynced] public int inextp;
    [HideInInspector, UdonSynced] public int[] SeedArray = new int[56];
    [HideInInspector, UdonSynced] public bool seedChnaged = false;

    private void Start()
    {
        if (Networking.IsMaster)
        {
            if (generateSeedOnStart) Seed = (int)DateTime.Now.Ticks;

            seedChnaged = true;

            inext = 0;
            inextp = 21;

            SetupRandom();
        }
    }
    public override void OnDeserialization()
    {
        if (seedChnaged)
        {
            SetupRandom();
        }
    }

    /// <summary>
    /// Set a seed and sync it to everyone.
    /// </summary>
    /// <param name="seed"></param>
    public void SetSeed(int seed)
    {
        if (Networking.IsOwner(gameObject))
        {
            Seed = seed;
            seedChnaged = true;

            inext = 0;
            inextp = 21;

            SetupRandom();
        }
        else
        {
            Debug.LogWarning("Only the owner of the object can set the seed");
        }
    }

    private void SetupRandom()
    {
        seedChnaged = false;

        int num = ((Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed));
        int num2 = 161803398 - num;
        SeedArray[55] = num2;
        int num3 = 1;
        for (int i = 1; i < 55; i++)
        {
            int num4 = 21 * i % 55;
            SeedArray[num4] = num3;
            num3 = num2 - num3;
            if (num3 < 0)
            {
                num3 += int.MaxValue;
            }

            num2 = SeedArray[num4];
        }

        for (int j = 1; j < 5; j++)
        {
            for (int k = 1; k < 56; k++)
            {
                SeedArray[k] -= SeedArray[1 + (k + 30) % 55];
                if (SeedArray[k] < 0)
                {
                    SeedArray[k] += int.MaxValue;
                }
            }
        }

        RequestSerialization();
    }
    private int InternalSample()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);

        int num = inext;
        int num2 = inextp;
        if (++num >= 56)
        {
            num = 1;
        }

        if (++num2 >= 56)
        {
            num2 = 1;
        }

        int num3 = SeedArray[num] - SeedArray[num2];
        if (num3 == int.MaxValue)
        {
            num3--;
        }

        if (num3 < 0)
        {
            num3 += int.MaxValue;
        }

        SeedArray[num] = num3;
        inext = num;
        inextp = num2;

        RequestSerialization();

        return num3;
    }

    private double Sample()
    {
        return (double)InternalSample() * 4.6566128752457969E-10;
    }
    private double GetSampleForLargeRange()
    {
        int num = InternalSample();
        if ((InternalSample() % 2 == 0) ? true : false)
        {
            num = -num;
        }

        double num2 = num;
        num2 += 2147483646.0;
        return num2 / 4294967293.0;
    }

    /// <summary>
    /// Returns the current seed used.
    /// </summary>
    /// <returns></returns>
    public int GetSeed()
    {
        return Seed;
    }

    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    /// <returns></returns>
    public int Next()
    {
        return InternalSample();
    }

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int Next(int minValue, int maxValue)
    {
        if (minValue > maxValue)
        {
            Debug.LogWarning("minValue can not be larger then maxValue.");
            return 0;
        }

        long num = (long)maxValue - (long)minValue;
        if (num <= int.MaxValue)
        {
            return (int)(Sample() * (double)num) + minValue;
        }

        return (int)((long)(GetSampleForLargeRange() * (double)num) + minValue);
    }

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public int Next(int maxValue)
    {
        if (maxValue < 0)
        {
            Debug.LogWarning("maxValue must be Positive.");
            return 0;
        }
        return (int)(Sample() * (double)maxValue);
    }

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <returns></returns>
    public double NextDouble()
    {
        return Sample();
    }
}
