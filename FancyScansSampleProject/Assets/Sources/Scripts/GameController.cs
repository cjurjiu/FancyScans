using UnityEngine;
using FancyScans;

public class GameController : MonoBehaviour {

    public SingleScanController singleScanController;
    public Transform singleScanTarget;

    public MultiScanController multiScanController;
    public Transform multiScanTarget;

    void Start () {
        if (singleScanController != null)
        {
            singleScanController.EmitScan();
        }

        if (multiScanController != null)
        {
            if (multiScanTarget != null){
                multiScanController.EmitScans(multiScanTarget.localPosition);
            }
            else
            {
                multiScanController.EmitScans();
            }
        }
    }

}