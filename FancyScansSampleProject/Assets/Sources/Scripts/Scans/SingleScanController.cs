using UnityEngine;
namespace FancyScans
{
    public class SingleScanController : MonoBehaviour
    {
        #region Public Fields
        public MeshRenderer targetedMeshRenderer;
        public Vector3 ScanOrigin;
        public int ScanSpeed;
        #endregion

        #region Private Fields
        private Material _targetMaterial;
        private float _scanDistance = 0;
        private bool _scanActive = false;
        private bool _initPerformed;
        #endregion

        #region Public API
        /// <summary>
		/// Emits one scan at the location set as scan origin on the class. 
        /// The scan line travels until stopped or a new scan is emitted.
		/// </summary>
        public void EmitScan()
        {
            StartPulsingInternal();
        }

        /// <summary>
        /// Emits one scan at the location set as scan origin through the parameter. This method ignores and overrides the origin set through the editor (if any).
        /// The scan line travels until stopped or a new scan is emitted.
        /// </summary>
        /// <param name="origin">The origin of the scan, as a Vector3.</param>
        public void EmitScan(Vector3 origin)
        {
            ScanOrigin = new Vector3(origin.x, 0, origin.z);
            StartPulsingInternal();
        }

        /// <summary>
        /// Overrides the origin set previously. 
        /// </summary>
        /// <param name="origin">The new origin of the scan, as a Vector3.</param>
        public void UpdateScanOrigin(Vector3 newOrigin)
        {
            if (_targetMaterial == null)
            {
                Debug.LogWarning("No material found in the meshRenderer. Did you init this scan controller?");
                //some error, do nothing
                return;
            }
            ScanOrigin = new Vector3(newOrigin.x, 0, newOrigin.z); ;
            _targetMaterial.SetVector("_ScanOrigin", ScanOrigin);
        }

        /// <summary>
        /// Stops any active scan. No-op if no scan is active.
        /// </summary>
        public void StopScan()
        {
            _scanActive = false;
            _scanDistance = 0;
        }
        #endregion

        #region Unity Events
        void Update()
        {
            if (_scanActive)
            {
                float addedDistance = Time.deltaTime * ScanSpeed;
                _scanDistance += addedDistance;
                _targetMaterial.SetFloat("_ScanDistance", _scanDistance);
            }
        }
        #endregion

        #region Private Methods
        private void DoInit()
        {
            if (!_initPerformed)
            {
                _targetMaterial = targetedMeshRenderer.GetComponent<MeshRenderer>().material;
                if (_targetMaterial != null)
                {
                    _initPerformed = true;
                }
                else
                {
                    Debug.LogWarning("No material found in the meshRenderer. Init ignored.");
                    //some error, do nothing
                    return;
                }
            }
        }

        private void StartPulsingInternal()
        {
            DoInit();
            if (_targetMaterial == null)
            {
                Debug.LogWarning("No material found in the meshRenderer. No scans done.");
                //some error, do nothing
                return;
            }

            _scanActive = true;
            _targetMaterial.SetVector("_ScanOrigin", ScanOrigin);
        }
        #endregion
    }
}
