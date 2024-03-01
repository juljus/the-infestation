using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDataPersistance
{
        [SerializeField] private GameObject player;
        [SerializeField] private PlayerScriptableObject[] playerScriptableObjectList = new PlayerScriptableObject[2];
        private int selectedCharacter;
        private int[] slotCharacterTypes;
        private PlayerScriptableObject playerScriptableObject;

        void Start()
        {
            // instantiate player
            player = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);

            // get player scriptable object
            if (slotCharacterTypes[selectedCharacter] == 1)
            {
                playerScriptableObject = playerScriptableObjectList[0];
            }
            else if (slotCharacterTypes[selectedCharacter] == 2)
            {
                playerScriptableObject = playerScriptableObjectList[1];
            }
            else
            {
                Debug.LogError("character type not selected");
            }
        }

        // Getters
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
            this.slotCharacterTypes = data.slotCharacterTypes;
            this.selectedCharacter = data.selectedCharacter;
        }
        public void SaveData(ref GameData data) {
            data.slotCharacterTypes = this.slotCharacterTypes;
            data.selectedCharacter = this.selectedCharacter;
        }
}
