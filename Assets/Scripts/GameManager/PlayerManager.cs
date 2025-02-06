using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataPersistance
{
        [SerializeField] private GameObject player;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        private int selectedCharacter;

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

        // data persistance
        public void LoadData(GameData data) {
            this.selectedCharacter = data.selectedChar;
        }

        public void SaveData(ref GameData data) {
            data.selectedChar = this.selectedCharacter;
        }
}
