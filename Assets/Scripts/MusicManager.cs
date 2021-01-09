using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;

public class MusicManager : MonoBehaviour
{

    #region --------------------    Public Properties

    /// <summary>
    /// Returns the clip for the manager.
    /// </summary>
    public AudioClip clip => _clip;

    #endregion

    #region --------------------    Public Methods

    /// <summary>
    /// Sets the clip with an Audio Source.
    /// </summary>
    /// <param name="_pSource"></param>
    public void SetClip(AudioSource _pSource) => SetClip(_pSource.clip);

    /// <summary>
    /// Sets the clip for the manager.
    /// </summary>
    /// <param name="_pClip"></param>
    public void SetClip(AudioClip _pClip)
    {
        _clip = _pClip;
        _data = _analyzer.Analyze(_clip);
        Track<Beat> _track = _data.GetTrack<Beat>();
    }

    #endregion

    #region --------------------    Private Fields

    [SerializeField] private RhythmAnalyzer _analyzer = null;
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private RhythmData _data = null;

    #endregion

    #region --------------------    Private Methods



    #endregion

}