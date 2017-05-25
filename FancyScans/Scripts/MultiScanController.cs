using UnityEngine;
namespace FancyScans
{
    public class MultiScanController : MonoBehaviour
    {
        #region Public Fields
        public MeshRenderer targetedMeshRenderer;
        public Vector3 ScanOrigin;
        [Range(1, 5)]
        public int MaxScansNr = 1;
        public int ScanSpeed;
        public float StartDelay = 2f;
        public float RestartDelay = 1f;
        #endregion

        #region Private Fields
        private Material _targetMaterial;
        private float[] _scansDistance;
        private bool[] _activeScans;
        private int _availableScanIndex = 0;
        private bool _scanActive = false;
        private bool _initPerformed;
        #endregion

        #region Public API
        /// <summary>
        /// Starts emitting scans at the location set as scan origin. 
        /// The scan lines travel until stopped or the max scans limit is hit. Once the 
        /// limit is hit, old scan lines will be discarded and new ones will be emitted from the center.
        /// </summary>
        public void EmitScans()
        {
            StartScanningInternal();
        }

        /// <summary>
        /// Starts emitting scans at the location specified in the parameter. This method ignores and overrides the origin set through the editor (if any). 
        /// The scan lines travel until stopped or the max scans limit is hit. Once the 
        /// limit is hit, old scan lines will be discarded and new ones will be emitted from the center.
        /// </summary>
        /// <param name="origin">The origin of the scans, as a Vector3.</param>
        public void EmitScans(Vector3 origin)
        {
            ScanOrigin = new Vector3(origin.x, 0, origin.z);
            StartScanningInternal();
        }

        /// <summary>
        /// Overrides the origin set previously. 
        /// </summary>
        /// <param name="origin">The new origin of the scans, as a Vector3.</param>
        public void UpdateScansOrigin(Vector3 newOrigin)
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
        /// Stops any active scans. No-op if no scan is active.
        /// </summary>
        public void StopScans()
        {
            CancelInvoke("StartScan");
            _scanActive = false;
            _availableScanIndex = 0;

            for (int i = 0; i < MaxScansNr; i++)
            {
                _activeScans[i] = false;
                _scansDistance[i] = 0;
            }
        }
        #endregion

        #region Unity Events
        void Update()
        {
            if (_scanActive)
            {
                float addedDistance = Time.deltaTime * ScanSpeed;
                for (int i = 0; i < _activeScans.Length; i++)
                {
                    if (_activeScans[i])
                    {
                        _scansDistance[i] += addedDistance;
                    }
                }
                _targetMaterial.SetFloatArray("_ScanDistance", _scansDistance);
            }
        }
        #endregion

        #region Private Methods
        private void StartScanningInternal()
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
            InvokeRepeating("StartScan", StartDelay, RestartDelay);
        }

        private void DoInit()
        {
            if (!_initPerformed)
            {
                _targetMaterial = targetedMeshRenderer.GetComponent<MeshRenderer>().material;
                if (_targetMaterial != null)
                {
                    _scansDistance = new float[MaxScansNr];
                    _activeScans = new bool[MaxScansNr];
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

        private void StartScan()
        {

            if (_availableScanIndex == MaxScansNr)
            {
                _availableScanIndex = 0;
            }

            _activeScans[_availableScanIndex] = true;
            _scansDistance[_availableScanIndex] = 0;

            _availableScanIndex++;
        }
        #endregion
    }
}
