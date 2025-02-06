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

        //! data persistance
        // TODO: implement InGameSave in the form of campfires and then move the needed saves to ingamesave instead of save

        public void InGameSave(ref GameData data)
        {
        }

        public void LoadData(GameData data)
        {
            this.selectedCharacter = data.selectedChar;
        }

        public void SaveData(ref GameData data)
        {
            data.selectedChar = this.selectedCharacter;
        }
}
