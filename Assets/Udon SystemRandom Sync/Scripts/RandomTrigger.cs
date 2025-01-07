
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class RandomTrigger : UdonSharpBehaviour
{
    [SerializeField] private UdonRandom udonRandom = null;
    [SerializeField] private TextMeshProUGUI text = null;

    [HideInInspector, UdonSynced] public int randomNumber = 0;

    public override void Interact()
    {
        Networking.SetOwner(Networking.LocalPlayer, gameObject);
        randomNumber = udonRandom.Next(-1000, 1000);

        text.text = randomNumber.ToString();
        RequestSerialization();
    }

    public override void OnDeserialization()
    {
        text.text = randomNumber.ToString();
    }
}
