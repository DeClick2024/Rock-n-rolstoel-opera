using extOSC;
using UnityEngine;
using UnityEngine.Video;

//namespace extOSC.Examples
//{
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
        public int LastReceivedVelocity = 0; //probably can be removed

        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;
        public OSCReceiver Receiver;

        [Header("Visuals")]
        [SerializeField] TMPro.TextMeshPro NoteNameMesh;
        public GameObject eyeInteractObject;
        public VideoClip videoClip;
        public VideoPlayer videoPlayer;

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

        public void PlayNote(int newVelocity) //there should be (volume)
        {
            SendMidiNote(pitch, newVelocity);
        }

        public void StopNote() 
        {
            SendMidiNote(pitch, 0);
        }

        private OSCMessage CreateMidiNote(int pitch, int velocity) //seems this method works w/ ableton only
        {
            var message = new OSCMessage(SEND_ADRESS);
            message.AddValue(OSCValue.Int(pitch));
            message.AddValue(OSCValue.Int(velocity)); //velocity IS a volume
            return message;
        }

        private void SendMidiNote(int pitch, int velocity)
        {
            OSCMessage message = CreateMidiNote(pitch, velocity);
            Transmitter.Send(message);
        }

        private void ChangeGuideObjectColorTo(Color toColor) //this func sshould be moved somewhere else 
        {
            //GuideObject.material.SetColor("_Color", toColor);
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
//}