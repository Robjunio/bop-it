using UnityEngine;
using TMPro;



public class TimerManagement : MonoBehaviour
{
    [SerializeField] TextMeshPro clockText;
    [SerializeField] private float _startTimerInSeconds = 60f;
    private float _timeChangeRate = -1f;

    private bool _isClockRuning = true;
    private float _currentTimeInSeconds;
    void Start()
    {
        _currentTimeInSeconds = _startTimerInSeconds;
        UpdateClockDisplay();
    }

    // Update is called once per frame
    void Update()
    {
     if(_isClockRuning)
        {
            _currentTimeInSeconds += _timeChangeRate * Time.deltaTime;
            if (_currentTimeInSeconds < 0)
            {
                _currentTimeInSeconds = 0;
            }
            UpdateClockDisplay();
        }
    }
    private void UpdateClockDisplay()
    {
        int minutes = Mathf.FloorToInt(_currentTimeInSeconds/60f);
        int seconds = Mathf.FloorToInt(_currentTimeInSeconds % 60);

        clockText.text = $"{minutes:D2}:{seconds:D2}";
    }

    public void StartClock()
    {
        _isClockRuning = true;
    }

    public void StopClock()
    {
        _isClockRuning = false;
    }

    public void ResetClock()
    {
        _currentTimeInSeconds = _startTimerInSeconds;
        UpdateClockDisplay() ;
    }

    public void SetTimeChangeRate(float newRate)
    {
        _timeChangeRate = newRate;
    }
    
    public void AddTime(float secondsToAdd)
    {
        _currentTimeInSeconds += secondsToAdd;
    }
    public void SubtractTime(float secondsToRemove)
    {
        _currentTimeInSeconds = Mathf.Max(0, _currentTimeInSeconds - secondsToRemove);
        UpdateClockDisplay();
    }
}
