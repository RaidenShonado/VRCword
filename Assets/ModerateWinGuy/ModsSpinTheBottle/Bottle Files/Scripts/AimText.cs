
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AimText : UdonSharpBehaviour
{
    public Transform ObjectToPointAtTarget;
    private Vector3 playerHead;

    private float yLevel;

    void Start()
    {
        yLevel = ObjectToPointAtTarget.transform.position.y;
    }

    private void Update()
    {
        playerHead = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;

        Vector3 point = playerHead;
        point.y = yLevel;
        ObjectToPointAtTarget.LookAt(point);
    }

}
