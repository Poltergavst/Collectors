using UnityEngine;

public class BuilderPreview : MonoBehaviour
{
    [SerializeField] GameObject _preview;
    [SerializeField] Material _invalidPreview;

    private GameObject _instance;
    private Renderer _renderer;
    private Material _normalMaterial;

    public void Create()
    {
        _instance = Instantiate(_preview, transform);
        _instance.TryGetComponent(out _renderer);
        _instance.SetActive(false);

        _normalMaterial = _renderer.material;
    }

    public void ShowPreview() => _instance.SetActive(true);

    public void HidePreview() => _instance.SetActive(false);

    public void UpdateMaterial(bool isValid) => _renderer.material = isValid? _normalMaterial : _invalidPreview;

    public void UpdatePosition(Vector3 previewPosition) => _instance.transform.position = previewPosition + new Vector3(0, 0.1f, 0);
}
