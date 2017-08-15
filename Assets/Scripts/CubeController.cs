using HoloToolkit.Unity.InputModule;
using UnityEngine;
using HoloToolkit.Unity;

namespace MARKit
{
    public class CubeController : MonoBehaviour, IInputClickHandler, ISpeechHandler {

        #region PUBLIC_MEMBER_VARIABLES

        public ulong ID;
        public TextToSpeechManager TextToSpeech;
        public string Color;
        #endregion

        #region PUBLIC_MEMBER_METHODS


        // Use this for initialization
        public void Start()
        {
            if (TextToSpeech == null)
            {
                Debug.LogError("No TextToSpeech Manager selected");
            }
        }

        // Update is called once per frame
        public void Update() {

        }

        /// <summary>
        /// Callback for click input
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            DoTextToSpeech();
        }

        /// <summary>
        /// Callback for speechinput
        /// </summary>
        /// <param name="eventData"></param>
        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            switch (eventData.RecognizedText.ToLower())
            {
                case "remove":
                    {
                        Delete();
                        break;
                    }
                case "drop":
                    {
                        AddRigidbody();
                        break;
                    }
                default:
                    break;
            }
        }



        #endregion


        #region PRIVATE_MEMBER_METHODS
        /// <summary>
        /// Deletes this object
        /// </summary>
        private void Delete()
        {
            GameObjectManager.Instance.DeleteObjectByGameObject(gameObject);
        }


        private void DoTextToSpeech()
        {
            // Create message
            var msg = "Hello. I am a " + Color + " Cube! My ID is " + ID + ".";

            // Speak message
            TextToSpeech.SpeakText(msg);
        }

        /// <summary>
        /// Adds a rigidbody to the cube
        /// </summary>
        private void AddRigidbody()
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();

            //test if there is already a rigidbody
            if (rigidbody == null)
            {
                gameObject.AddComponent<Rigidbody>();
            }
        }
        #endregion

    }
}