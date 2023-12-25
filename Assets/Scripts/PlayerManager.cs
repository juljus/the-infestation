using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
        [SerializeField] private GameObject player;

        public Transform GetPlayerTransform {
            get { return player.transform; }
        }
}
