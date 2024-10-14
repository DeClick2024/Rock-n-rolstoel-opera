using UnityEngine;
using UnityEngine.Video;

namespace extOSC.Examples
{
    public class SendNoteOnOver : MonoBehaviour
    {
        //adresses
        public const string SEND_ADRESS = "/hand0";
        public const string RECEIVE_NOTE = "/Note1";
        public const string RECEIVE_VELOCITY = "/Velocity1";
        //public bool OnMouseEnterActive = false;

        [Header("Notes Settings")]
        public int pitch;
        public string NoteName;
        public int velocity;


        public TMPro.TextMeshProUGUI NoteNameMesh;
        public GameObject eyeInteractObject;
        //public GuideBall GuideBall;

        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;
        public OSCReceiver Receiver;

        public int LastReceivedVelocity = 0; //probably can be removed
        public Renderer IdleObject;
        //public Renderer GuideObject;
        public VideoClip videoClip;
        public VideoPlayer videoPlayer;
        public Renderer targetRenderer;

        //helper vars for setting note nr and name
        private int _lastPitch;
        private string _lastNoteName;

        protected virtual void Start()
        {
            Receiver.Bind(RECEIVE_NOTE, ReceivedNote);
            Receiver.Bind(RECEIVE_VELOCITY, ReceivedVelocity);
            _lastNoteName = NoteName;
            _lastPitch = pitch;
            SetNoteNameFromPitch();
        }

        private void SetNoteNameFromPitch()
        {
            NoteName = NoteToMIDIConverter.ValueToNode(pitch);
            if (NoteNameMesh != null) NoteNameMesh.text = NoteName;
        }

        private void SetNotePitchFromName()
        {
            pitch = NoteToMIDIConverter.NoteToValue(NoteName);
        }

        private void OnValidate()
        {
            //FillNoteLookup();
            //Debug.Log("NoteName=" + NoteName + " lastName=" + lastNoteName);
            //Debug.Log("NoteNr=" + pitch + " lastNr=" + lastPitch);

            if (_lastNoteName != NoteName)
            {
                SetNotePitchFromName();
                _lastNoteName = NoteName;
            }
            if (_lastPitch != pitch)
            {
                SetNoteNameFromPitch();
                _lastPitch = pitch;
            }
            gameObject.name = "Note_" + NoteName;

        }

        public void PlayNote()
        {
            SendMidiNote(pitch, velocity);
        }

        public void StopNote()
        {
            SendMidiNote(pitch, 0);
        }

        private OSCMessage CreateMidiNote(int pitch, int velocity) //seems this method works w/ ableton only
        {
            var message = new OSCMessage(SEND_ADRESS);
            message.AddValue(OSCValue.Int(pitch));
            message.AddValue(OSCValue.Int(velocity));
            return message;
        }

        private void SendMidiNote(int pitch, int velocity)
        {
            OSCMessage message = CreateMidiNote(pitch, velocity);
            Transmitter.Send(message);
        }

        private void changeColorTo(Color toColor) //should be deleted
        {
            // Get the Renderer component from the new cube
            //var cubeRenderer = GetComponent<Renderer>();
            var bigCubeRenderer = eyeInteractObject.GetComponent<Renderer>();
            // Call SetColor using the shader property name "_Color" and setting the color to red
            //cubeRenderer.material.SetColor("_Color", toColor);
            bigCubeRenderer.material.SetColor("_Color", toColor);
        }

        private void ChangeIdleObjectColorTo(Color toColor) //should be deleted
        {
            IdleObject.material.SetColor("_Color", toColor);
        }

        private void ChangeGuideObjectColorTo(Color toColor) //this func sshould be moved somewhere else 
        {
            //GuideObject.material.SetColor("_Color", toColor);
        }
        private void ChangeMainCubeColor(Color toColor) //should be deleted

        {
            targetRenderer.material.SetColor("_Color", toColor);
        }


        //Receiving messages code
        //Velocity first, then note

        private void ReceivedNote(OSCMessage message) //seems this method works w/ ableton only
        {
            Debug.LogFormat("Received: {0}", message);

            int NoteValue;
            if (message.ToInt(out NoteValue))
            {
                if (NoteValue == pitch)
                {
                    if (LastReceivedVelocity > 0)
                    {
                        ChangeGuideObjectColorTo(Color.green);
                        videoPlayer = eyeInteractObject.GetComponent<VideoPlayer>();
                        videoPlayer.Play();
                        Debug.Log("Received note value: " + NoteValue); 
                    }
                    else
                    {
                        videoPlayer.Stop();
                        ChangeGuideObjectColorTo(Color.white);
                    }
                }
            }
        }

        private void ReceivedVelocity(OSCMessage message) //seems this method works w/ ableton only
        {
            //Debug.LogFormat("Received: {0}", message);

            int VelocityValue;
            if (message.ToInt(out VelocityValue))
            {
                LastReceivedVelocity = VelocityValue;
                //velocity = VelocityValue; 
            }
        }
    }
}