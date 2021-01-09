using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RhythmTool;

public class Building : MonoBehaviour
{

    #region --------------------    Public Enumerations

    public enum BuildingType { Skyscraper, Shop, Apartment, Park, Restaurant, }

    #endregion

    #region --------------------    Public Properties



    #endregion

    #region --------------------    Private Fields



    [SerializeField] private MeshRenderer _renderer = null;
    private bool _isResetingBeat = false;

    #endregion

    #region --------------------    Private Methods

    private void Start()
    {
        MusicManager.OnBeatEvent += _OnBeat;
    }

    private void _OnBeat(Beat _pBeat)
    {
        _PerformBeatVisual(_pBeat);
    }

    /// <summary>
    /// Performs the visual portion of the beat actions
    /// </summary>
    /// <param name="_pBeat"></param>
    private void _PerformBeatVisual(Beat _pBeat)
    {
        if (_isResetingBeat) return;
        _isResetingBeat = true;
        float _speed = (1f / Mathf.Max((_pBeat.bpm / 60f), 0.01f));
        float _resetTime = _speed * 0.9f;
        DOTween.To(() => _renderer.material.GetFloat("Vector1_Beat"), x => _renderer.material.SetFloat("Vector1_Beat", x), 1f, _speed)
            .OnComplete(() => {
                DOTween.To(() => _renderer.material.GetFloat("Vector1_Beat"), x => _renderer.material.SetFloat("Vector1_Beat", x), 0f, _resetTime)
                    .OnComplete(() => { _isResetingBeat = false; });
            });
    }

    #endregion

}