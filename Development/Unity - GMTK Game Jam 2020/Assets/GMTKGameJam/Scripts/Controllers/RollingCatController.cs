using UnityEngine;

public class RollingCatController : MonoBehaviour
{
    [SerializeField] private Texture _texture;
    [SerializeField] [NonNull] private SkinnedMeshRenderer _renderer;
    private static readonly int Detail = Shader.PropertyToID("Detail");

    public void Start()
    {
        if (_renderer != null)
        {
            _renderer.material.mainTexture = _texture;
        }
    }
}
