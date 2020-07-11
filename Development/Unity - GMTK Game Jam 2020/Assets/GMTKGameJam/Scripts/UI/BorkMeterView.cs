using TMPro;
using UnityEngine;

public class BorkMeterView : MonoBehaviour
{
    [SerializeField] [NonNull] private RollingCharacterController _character;
    private float _maxValue;
    private float _value;

    private float _lastUpdateValue;
    
    [SerializeField][NonNull] private TextMeshProUGUI _debugText;

    public void Start()
    {
        _character.BorkingBehavior.OnBorkChange += Set;
    }

    private void Set(float value, float max, float cooldown)
    {
        _value = value;
        _maxValue = max;
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_lastUpdateValue != _value && _debugText != null)
        {
            float perc = Mathf.Clamp01(_value / _maxValue);
            _lastUpdateValue = _value;
            _debugText.text = $"BORK [{perc:P0}]";
        }
    }
}
