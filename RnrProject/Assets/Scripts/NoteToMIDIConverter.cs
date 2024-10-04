using System;
using System.Collections.Generic;
using UnityEngine;

public class NoteToMIDIConverter : MonoBehaviour
{
    [SerializeField] private string noteName; //could be public, need to think about it
    private int _noteValue;

    //should i add some idiot prootection?

    Dictionary<string, int> noteDictionary = new Dictionary<string, int>()
    {
        {"C", 24 },
        {"C#", 25 },
        {"D", 26 },
        {"D#", 27 },
        {"E", 28 },
        {"F", 29 },
        {"F#", 30 },
        {"G", 31 },
        {"G#", 32 },
        {"A", 33 },
        {"A#", 34 },
        {"B", 35 }
    };

    int NoteToValue(string noteName)
    {
        string note = "";
        string octave = "";

        for (int i=0; i<noteName.Length;i++)
        {
            if (noteName[i] >= 65 && noteName[i] <= 71 || noteName[i] >= 97 && noteName[i] <= 103 || noteName[i] == 35) note += noteName[i];
            else if (noteName[i] >= 48 && noteName[i] <= 56 || noteName[i] == 45) octave += noteName[i];
        }

        Int32.TryParse(octave, out int octaveValue);
        note = note.ToUpper();

        return noteDictionary[note] + (octaveValue * 12);
    }

    private void Start()
    {
        Debug.LogWarning(NoteToValue(noteName));
    }
}
