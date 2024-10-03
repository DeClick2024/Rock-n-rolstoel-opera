using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

namespace extOSC.Examples
{
    public class SendNoteOnOver : MonoBehaviour
    {

        //public bool OnMouseEnterActive = false;

        public int pitch;
        public string NoteName;
        public int velocity;
        public TMPro.TextMeshProUGUI NoteNameMesh;
        public GameObject eyeInteractObject;
        public string SendAddress = "/hand0";


        public string ReceiveNote = "/Note1";
        public string ReceiveVelocity = "/Velocity1";
        //public GuideBall GuideBall;

        [Header("OSC Settings")]
        public OSCTransmitter Transmitter;
        public OSCReceiver Receiver;

        public int LastReceivedVelocity = 0;
        public Renderer IdleObject;
        public Renderer GuideObject;
        public VideoClip videoClip;
        public VideoPlayer videoPlayer;
        public Renderer targetRenderer;

        //helper vars for setting note nr and name
        private int lastPitch;
        private string lastNoteName;

        struct NoteNrName
        {
            public int pitch;
            public string NoteName;

            public NoteNrName(int noteNr, string noteName)
            {
                this.pitch = noteNr;
                this.NoteName = noteName;
            }
        }

        List<NoteNrName> noteNameLookup = new List<NoteNrName>();

        protected virtual void Start()
        {
            Receiver.Bind(ReceiveNote, ReceivedNote);
            Receiver.Bind(ReceiveVelocity, ReceivedVelocity);
            FillNoteLookup();
            lastNoteName = NoteName;
            lastPitch = pitch;
            SetNoteNameFromPitch();
        }

        private void FillNoteLookup()
        {
            //fill lookup table with midi notes

            noteNameLookup.Add(new NoteNrName(60, "C3"));
            noteNameLookup.Add(new NoteNrName(61, "C#3"));
            noteNameLookup.Add(new NoteNrName(62, "D3"));
            noteNameLookup.Add(new NoteNrName(63, "D#3"));
            noteNameLookup.Add(new NoteNrName(64, "E3"));
            noteNameLookup.Add(new NoteNrName(65, "F3"));
            noteNameLookup.Add(new NoteNrName(66, "F#3"));
            noteNameLookup.Add(new NoteNrName(67, "G3"));
            noteNameLookup.Add(new NoteNrName(68, "G#3"));
            noteNameLookup.Add(new NoteNrName(69, "A3"));
            noteNameLookup.Add(new NoteNrName(70, "A#3"));
            noteNameLookup.Add(new NoteNrName(71, "B3"));
            noteNameLookup.Add(new NoteNrName(72, "C4"));
            noteNameLookup.Add(new NoteNrName(73, "C#4"));
            noteNameLookup.Add(new NoteNrName(74, "D4"));
            noteNameLookup.Add(new NoteNrName(75, "D#4"));
            noteNameLookup.Add(new NoteNrName(76, "E4"));
            noteNameLookup.Add(new NoteNrName(77, "F4"));
            noteNameLookup.Add(new NoteNrName(78, "F#4"));
            noteNameLookup.Add(new NoteNrName(79, "G4"));
            noteNameLookup.Add(new NoteNrName(80, "G#4"));
            noteNameLookup.Add(new NoteNrName(81, "A4"));
            noteNameLookup.Add(new NoteNrName(82, "A#4"));
            noteNameLookup.Add(new NoteNrName(83, "B4"));
            noteNameLookup.Add(new NoteNrName(84, "C5"));
        }

        private void SetNoteNameFromPitch()
        {
            foreach (NoteNrName note in noteNameLookup)
            {
                if (note.pitch == pitch)
                {
                    NoteName = note.NoteName;
                    if (NoteNameMesh != null)
                    {
                        NoteNameMesh.text = note.NoteName;
                    }
                }
            }
        }

        private void SetNotePitchFromName()
        {
            foreach (NoteNrName note in noteNameLookup)
            {
                //Debug.Log("Pitch=" + note.pitch + " name=" + note.NoteName);
                if (note.NoteName == NoteName)
                {
                    pitch = note.pitch;
                }
            }
        }

        private void OnValidate()
        {
            //FillNoteLookup();
            //Debug.Log("NoteName=" + NoteName + " lastName=" + lastNoteName);
            //Debug.Log("NoteNr=" + pitch + " lastNr=" + lastPitch);
            if (noteNameLookup.Count == 0)
            {
                FillNoteLookup();
            }

            if (lastNoteName != NoteName)
            {
                SetNotePitchFromName();
                lastNoteName = NoteName;
            }
            if (lastPitch != pitch)
            {
                SetNoteNameFromPitch();
                lastPitch = pitch;
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

        /* Function moved to eyeinteractable
        /// <summary>
        /// Helper function to simulate eyetracking interactiuon in editor
        /// </summary>
        private void OnMouseEnter()
        {
            if (!OnMouseEnterActive) return;

            SendMidiNote(pitch, velocity);
            changeColorTo(Color.red);
            videoPlayer.renderMode = VideoRenderMode.MaterialOverride;

        }

        /// <summary>
        /// Helper function to simulate eyetracking interactiuon in editor
        /// </summary>
        private void OnMouseExit()
        {
            if (!OnMouseEnterActive) return;

            SendMidiNote(pitch, 0);
            changeColorTo(Color.white);

        }
        */

        private OSCMessage CreateMidiNote(int pitch, int velocity)
        {
            var message = new OSCMessage(SendAddress);
            message.AddValue(OSCValue.Int(pitch));
            message.AddValue(OSCValue.Int(velocity));
            return message;
        }

        private void SendMidiNote(int pitch, int velocity)
        {
            OSCMessage message = CreateMidiNote(pitch, velocity);
            Transmitter.Send(message);
        }

        private void changeColorTo(Color toColor)
        {
            // Get the Renderer component from the new cube
            //var cubeRenderer = GetComponent<Renderer>();
            var bigCubeRenderer = eyeInteractObject.GetComponent<Renderer>();
            // Call SetColor using the shader property name "_Color" and setting the color to red
            //cubeRenderer.material.SetColor("_Color", toColor);
            bigCubeRenderer.material.SetColor("_Color", toColor);
        }

        private void ChangeIdleObjectColorTo(Color toColor)
        {
            IdleObject.material.SetColor("_Color", toColor);
        }

        private void ChangeGuideObjectColorTo(Color toColor)
        {
            GuideObject.material.SetColor("_Color", toColor);
        }
        private void ChangeMainCubeColor(Color toColor)

        {
            targetRenderer.material.SetColor("_Color", toColor);
        }


        //Receiving messages code
        //Velocity first, then note

        private void ReceivedNote(OSCMessage message)
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
                        //changeColorTo(Color.green);
                        //ChangeIdleObjectColorTo(Color.gray);
                        videoPlayer = eyeInteractObject.GetComponent<VideoPlayer>();
                        videoPlayer.Play();
                        // Move the guiding ball to the position corresponding to the note
                        //GuideBall.MoveToPosition(NoteValue); // Lauri Code
                        Debug.Log("Received note value: " + NoteValue); // Lauri Code
                    }
                    else
                    {
                        //changeColorTo(Color.gray);
                        videoPlayer.Stop();
                        ChangeGuideObjectColorTo(Color.white);
                        //ChangeIdleObjectColorTo(Color.yellow);
                    }
                }
            }
        }

        private void ReceivedVelocity(OSCMessage message)
        {
            //Debug.LogFormat("Received: {0}", message);

            int VelocityValue;
            if (message.ToInt(out VelocityValue))
            {
                LastReceivedVelocity = VelocityValue;
            }
        }
    }
}