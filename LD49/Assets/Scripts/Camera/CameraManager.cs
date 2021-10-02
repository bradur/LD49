using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{

   public static CameraManager main;
   private void Awake() {
      main = this;
   }

   [SerializeField]
   private CinemachineVirtualCamera playerCamera;

   [SerializeField]
   private CinemachineVirtualCamera beforePlayerCamera;
   [SerializeField]
   private CinemachineBrain cameraBrain;
   private bool isBlending = false;
   private bool blendingStarted = false;

   private UnityAction afterBlendCallback;

   public void SetUp(UnityAction afterBlend) {
      afterBlendCallback = afterBlend;
      GameObject player = GameObject.FindGameObjectWithTag("Player");
      if (player == null) {
         Debug.LogWarning("Can't set up camera - GameObject with tag 'Player' missing!");
         return;
      }
      beforePlayerCamera.Follow = player.transform;
      beforePlayerCamera.LookAt = player.transform;
      playerCamera.Follow = player.transform;
      playerCamera.LookAt = player.transform;
      playerCamera.Priority = beforePlayerCamera.Priority + 10;
   }

   void Update() {
      if (!blendingStarted) {
         if (cameraBrain.IsBlending) {
            isBlending = true;
            blendingStarted = true;
         }
      }
      if (!isBlending) {
         return;
      }
      if (!cameraBrain.IsBlending) {
         isBlending = false;
         afterBlendCallback.Invoke();
      }
   }
}

