using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMaterialAlphaBehavior : MonoBehaviour
{
    [SerializeField][NonNull] protected MeshRenderer _renderer;
    public float alpha = 1.0f;

    private static readonly int Alpha = Shader.PropertyToID("Alpha");
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (_renderer != null)
        {
            foreach (var item in _renderer.materials)
            {
                item.SetFloat(Alpha, alpha);
            }
        }
    }
}
