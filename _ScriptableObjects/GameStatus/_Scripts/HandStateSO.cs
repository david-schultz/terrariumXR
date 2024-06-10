using UnityEngine;
using TerrariumXR.Interaction;

namespace TerrariumXR
{
    [CreateAssetMenu(fileName="HandStateData", menuName="Global/HandStateData")]
    public class HandStateSO : ScriptableObject
    {
        public HandState LeftHand;
        public HandState RightHand;
        public HandState PrimaryHand
        {
            get => LeftHand.IsPrimary ? LeftHand : RightHand;
        }
        public HandState AltHand
        {
            get => !LeftHand.IsPrimary ? LeftHand : RightHand;
        }

        public bool isInitialized = false;

        public void Initialize()
        {
            Initialize(false, "n/a");
            isInitialized = true;
        }

        public void Initialize(bool isRightPrimary, string activePose)
        {
            LeftHand = new HandState("LeftHand");
            RightHand = new HandState("RightHand");
            
            if (isRightPrimary)
            {
                RightHand.IsPrimary = true;
                LeftHand.ActivePose = activePose;
            }
        }
    }
}