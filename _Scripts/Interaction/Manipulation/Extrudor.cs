using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrariumXR;
using TerrariumXR.EventSystem;
using TerrariumXR.Geometry;

namespace TerrariumXR.UI
{
    public class Extrudor : MonoBehaviour
    {
        [SerializeField] private Debugger _debugger;
        [SerializeField] private GameStatusSO _gameSO;
        [SerializeField] private IntIntIntEventChannelSO _stepUpdateChannel;
        [SerializeField] private CommandEventChannelSO _commandChannel;
        [SerializeField] private IntEventChannelSO _triangleSelectionChannel; 

        [SerializeField] private VoidEventChannelSO _extrudeChannel;
        [SerializeField] private VoidEventChannelSO _flattenChannel;
        [SerializeField] private SteppedOneGrabTransformer _grabTransformer;

        // Inside the extrudor,
        // on grab display the normal line
        // Limit position between start and end of line
        // When pulled to end, extrude selection


    // ================== Event Management ==================


        private void OnEnable()
        {
            _stepUpdateChannel.OnEventRaised += ActivateExtrude;
            _triangleSelectionChannel.OnEventRaised += TranslateToAverageSelectionPoint;
        }
        private void OnDisable()
        {
            _stepUpdateChannel.OnEventRaised -= ActivateExtrude;
            _triangleSelectionChannel.OnEventRaised -= TranslateToAverageSelectionPoint;
        }

        private void ActivateExtrude(int xStep, int yStep, int zStep)
        {
            if (zStep == 0)
            {
                _flattenChannel?.RaiseEvent();
            }
            else if (zStep == 1)
            {
                _extrudeChannel?.RaiseEvent();
            }
            else if (zStep == 2)
            {
                _extrudeChannel?.RaiseEvent();
            }
        }

        private void TranslateToAverageSelectionPoint(int tri)
        {
            if (_gameSO.State.SelectedTriangles.Count > 0)
            {
                // Get average midpoint from all triangles
                Vector3 midpoint = GetMidpoint(_gameSO.State.SelectedTriangles);

                // Move and rotate above midpoint
                Vector3 forwardAxis = midpoint.normalized;
                transform.localPosition = forwardAxis * 0.3f;
                transform.rotation = Quaternion.FromToRotation(transform.parent.transform.forward, forwardAxis);

                // Set 0 or 1 position
                if (GetExtrudedAverage(_gameSO.State.SelectedTriangles))
                {
                    // set to 1 position
                    _grabTransformer.InjectCurrentStep(-1, -1, 1);
                }
                else
                {
                    // set to 0 position
                    _grabTransformer.InjectCurrentStep(-1, -1, 0);
                }
            }
        }

        private Vector3 GetMidpoint(List<int> selection)
        {
            float x = 0f;
            float y = 0f;
            float z = 0f;

            foreach (int triangleId in selection)
            {
                Vector3 polyMidpoint = _gameSO.State.PlanetState.GetTriangleMidPoint(triangleId);
                // _debugger.Log("tri" + triangleId + ": " + polyMidpoint.ToString());
                x += polyMidpoint.x;
                y += polyMidpoint.y;
                z += polyMidpoint.z;
            }

            x = x / (float)selection.Count;
            y = y / (float)selection.Count;
            z = z / (float)selection.Count;

            return new Vector3(x, y, z);
        }

        // Returns true if the majority (inclusive) of selected triangles are extruded.
        private bool GetExtrudedAverage(List<int> selection)
        {
            int avg = 0;
            foreach (int triangleId in selection)
            {
                MeshTriangle tri = _gameSO.State.PlanetState.MeshTriangles[triangleId];
                if (tri.IsExtruded) avg--;
                else avg++;
            }

            return avg < 0;
        }
    }
}