using System;
using UnityEngine;

[Serializable]


public class RangedValue<TValue> where TValue : IComparable<TValue>
{
    public delegate void DelegateOnClamp<TValue>();
    public event DelegateOnClamp<TValue> OnMax;
    public event DelegateOnClamp<TValue> OnMin;

    private bool _isClampedMax;
    private bool _isClampedMin;
    
    [SerializeField] protected TValue _min;
    [SerializeField] protected TValue _max;
    [SerializeField] protected TValue _value;
    public TValue Value
    {
        get => _value;
        set
        {
            _value = Clamp(_min, _max, value);
            if (_value.Equals(_max) && !_isClampedMax)
            {
                _isClampedMax = true;
                OnMax?.Invoke();
            }

            if (_value.Equals(_min)  && !_isClampedMin)
            {
                _isClampedMin = true;
                OnMin?.Invoke();
            }
        }
    }

    public static TValue Clamp(TValue min, TValue max, TValue value)
    {
        if (value.CompareTo(max) > 0)
        {
            return max;
        }
        if (value.CompareTo(min) < 0)
        {
            return min;
        }
        return value;
    }
}

public class RangedFloat : RangedValue<float>
{
    
}
