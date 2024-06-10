using UnityEngine;
using TerrariumXR.EventSystem;

namespace TerrariumXR
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Debugger _debugger;
        [SerializeField] private PlanetStateSO _planetStateSO;
        [SerializeField] private HandStateSO _handStateSO;
        [SerializeField] private GrabbableGeometrySO _grabbableSO;

        // [SerializeField] private Color32 _defaultColor;
        // [SerializeField] private float _radius;
        [SerializeField] private GameObject _testPrefab;
        [SerializeField] private VoidEventChannelSO _meshUpdateChannel;

    // ================== Initialization ==================
        void Start()
        {
            _debugger.Log("GameManager started.");

            // _planetStateSO.Initialize("Timber Hearth");
            _planetStateSO.Initialize("Timber Hearth", 0.25f, 3, false);
            _handStateSO.Initialize(true, "VertexSelector");
            _grabbableSO.Initialize(0.25f, 0.23f, 0.31f);

            _meshUpdateChannel?.RaiseEvent();
            
            DisplayDebug();
        }

        private void DisplayDebug()
        {
            _debugger.SetFieldTitle("FieldA|Planet Name");
            _debugger.SetFieldData( "FieldA|" + _planetStateSO.Name);
            _debugger.SetFieldTitle("FieldB|# of Vertices");
            _debugger.SetFieldData( "FieldB|" + _planetStateSO.Octree.Count);
            _debugger.SetFieldTitle("FieldC|Height Range");
            _debugger.SetFieldData( "FieldC|//TODO");

            _debugger.SetFieldTitle("FieldD|Primary Hand");
            _debugger.SetFieldData( "FieldD|" + _handStateSO.PrimaryHand.Tag);
            _debugger.SetFieldTitle("FieldE|Alt Hand's Pose");
            _debugger.SetFieldData( "FieldE|" + _handStateSO.AltHand.ActivePose);
        }
    }
}