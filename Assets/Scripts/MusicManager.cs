using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RhythmTool;
using DG.Tweening;

public class MusicManager : MonoBehaviour
{

    #region --------------------    Public Properties

    /// <summary>
    /// The singleton instance of the class
    /// </summary>
    public static MusicManager instance { get; private set; } = null;

    /// <summary>
    /// Definition for the music event
    /// </summary>
    public delegate void MusicEvent(Beat _pBeat);

    /// <summary>
    /// The event used to signal that a beat has occured
    /// </summary>
    public static MusicEvent OnBeatEvent { get; set; } = null;

    /// <summary>
    /// Returns the clip for the manager.
    /// </summary>
    public AudioClip clip => _nextClip;

    #endregion

    #region --------------------    Public Methods

    /// <summary>
    /// Imports the clip
    /// </summary>
    /// <param name="_pPath"></param>
    public void ImportClip(string _pPath)
    {
        _importer.Loaded += SetClip;
        _importer.Import(_pPath);
    }

    /// <summary>
    /// Sets the clip for the manager.
    /// </summary>
    /// <param name="_pClip"></param>
    public void SetClip(AudioClip _pClip)
    {
        _importer.Loaded -= SetClip;
        _nextClip = _pClip;
        _analyzer.Analyze(_nextClip);
        _isLoading = true;
    }

    #endregion

    #region --------------------    Private Fields

    [SerializeField] private AudioSource _source = null;
    [SerializeField] private Browser _browser = null;
    [SerializeField] private AudioImporter _importer = null;
    [SerializeField] private RhythmAnalyzer _analyzer = null;
    [SerializeField] private RhythmPlayer _player = null;

    private bool _isLoading = false;
    private AudioClip _nextClip = null;

    #endregion

    #region --------------------    Private Methods

    /// <summary>
    /// Sets the singleton
    /// </summary>
    private void Awake() => _SetSingleton();

    /// <summary>
    /// Configures the singleton instance of the class
    /// </summary>
    private bool _SetSingleton()
    {
        instance = instance ?? this;
        if (instance == this) return true;
        Destroy(gameObject);
        return false;
    }

    /// <summary>
    /// Hookup with the file browser
    /// </summary>
    private void Start()
    {
        _browser.FileSelected += ImportClip;
        ((RhythmEventProvider)_player.targets[0]).Register<Beat>(_OnBeat);
    }

    /// <summary>
    /// Unregister with the event provider
    /// </summary>
    private void OnDestroy() => ((RhythmEventProvider)_player.targets[0]).Unregister<Beat>(_OnBeat);

    /// <summary>
    /// Lets all subscribers know that a beat has occured
    /// </summary>
    /// <param name="_pBeat"></param>
    private void _OnBeat(Beat _pBeat) => OnBeatEvent?.Invoke(_pBeat);

    /// <summary>
    /// Looks to see if new music is loading in and when it's ready, does a transition to the new music.
    /// </summary>
    private void Update()
    {
        if (!_isLoading) return;
        if (_analyzer.isDone)
        {
            _isLoading = false;
            _player.rhythmData = _analyzer.rhythmData;
            //  Fades the track out and fades in the new track
            DOTween.To(() => _source.volume, x => _source.volume = x, 0f, 1f)
               .OnComplete(() => {
                   _isLoading = false;
                   _source.clip = _nextClip;
                   _source.Play();
                   DOTween.To(() => _source.volume, y => _source.volume = y, 1f, 1f);
               });
        }
    }

    #endregion

}