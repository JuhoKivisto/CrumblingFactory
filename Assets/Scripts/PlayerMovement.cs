//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(Player))]
//public class PlayerMovement : MonoBehaviour {

//    private Player player;

//	void OnEnable ()
//    {
//        player = GetComponent<Player>();
//	}
	
//	public void TeleportPlayerToPosition(Vector3 toPosition)
//    {
//        if (toPosition == null || player.PlayerStatus.PlayerLaserActive == false)
//            return;

//        TeleportPlayer(toPosition);
//    }

//    void TeleportPlayer(Vector3 teleportPoint)
//    {
//        if (teleportPoint != Vector3.zero)
//        {
//            Vector3 playerOffset = CalculatePlayerOffset();
//            Vector3 convertedTeleportPoint = player.PlayAreaCenterObj.InverseTransformPoint(teleportPoint);
//            Vector3 newPoint = convertedTeleportPoint + playerOffset;

//            transform.position = transform.TransformPoint(newPoint);
//        }
//    }

//    Vector3 CalculatePlayerOffset()
//    {
//        Vector3 offset = Vector3.zero;
//        offset.x = player.PlayAreaCenterObj.localPosition.x - Camera.main.transform.localPosition.x;
//        offset.z = player.PlayAreaCenterObj.localPosition.z - Camera.main.transform.localPosition.z;

//        return offset;
//    }
//}
