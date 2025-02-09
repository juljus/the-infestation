using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
        [SerializeField] private GameObject player;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        //! getters
        public Transform GetPlayerTransform {
            get { return player.transform; }
        }
        public GameObject GetPlayer {
            get { return player; }
        }
        public PlayerScriptableObject GetPlayerScriptableObject {
            get { return playerScriptableObject; }
        }
}
