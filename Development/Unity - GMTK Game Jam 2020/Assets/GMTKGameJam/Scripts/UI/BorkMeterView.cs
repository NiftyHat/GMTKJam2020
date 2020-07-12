using TMPro;
using UnityEngine;

public class BorkMeterView : MonoBehaviour
{
    [SerializeField] [NonNull] private RollingCharacterController _character;
    private float _maxValue;
    private float _value;
    [SerializeField][NonNull] private RectTransform _fill;
    [SerializeField] [NonNull] private GameObject _goView;

    private float _lastUpdateValue = 0;
	 public bool startWithBorkDisplayed = false;

    public void Start()
    {
        _character.BorkingBehavior.OnBorkChange += Set;
		  if(startWithBorkDisplayed) _goView.SetActive(true);
    }

    private void Set(float value, float max, float cooldown)
    {
        _value = value;
        _maxValue = max;
        if (cooldown > 0 && !_goView.activeSelf)
        {
            _goView.SetActive(true);
        }
    }
    
    // Update is called once per frame
    private void Update()
    {
        if (_lastUpdateValue != _value && _fill != null)
        {
            float perc = Mathf.Clamp01(_value / _maxValue);
            _fill.localScale = new Vector3(_fill.localScale.x, perc, _fill.localScale.z);
        }
    }
}
