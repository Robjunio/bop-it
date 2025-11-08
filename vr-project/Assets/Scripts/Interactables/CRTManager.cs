using UnityEditorInternal;
using UnityEngine;
public class CRTManager : MonoBehaviour
{
    [SerializeField] private GameObject _crtPrefab;
    private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }


    private void Update()
    {
    

        //Invoke("DefaultCRTState", 1f);
        Invoke("OffCRTState", 2f);
    }

    private void DefaultCRTState()
    {
        _material.SetFloat("NoiseScale", 0f);
        _material.SetFloat("_ChannelSwitch", 0.28f); 
        _material.SetFloat("_VerticalSpeed", 0f);
        _material.SetFloat("NoiseScale", 0f);
        _material.SetFloat("_VeticalSpeedModifier", 0f);
        _material.SetFloat("_ModuloControl", -1f); 
        _material.SetVector("_TillingAndOffset", new Vector4(0f,0f,0f,0f));
        _material.SetFloat("_MaskTilingPixels", 0f);
        _material.SetFloat("_PixelHeight", 0f);
        _material.SetFloat("_PixelWIdth", 0f);
        _material.SetFloat("_Brightness", 0f);
    }

    private void OffCRTState()
    {
        _material.SetFloat("NoiseScale", 0f);
        _material.SetFloat("_ChannelSwitch", 0f);
        _material.SetFloat("_VerticalSpeed", 0f);
        _material.SetFloat("NoiseScale", 0f);
        _material.SetFloat("_VeticalSpeedModifier", 0f);
        _material.SetFloat("_ModuloControl", -1f);
        _material.SetVector("_TillingAndOffset", new Vector4(0f, 0f, 0f, 0f));
        _material.SetFloat("_MaskTilingPixels", 0f);
        _material.SetFloat("_PixelHeight", 0f);
        _material.SetFloat("_PixelWIdth", 0f);
        _material.SetFloat("_Brightness", 0f);
    }

    public void ChangeCRTImage(Texture _image)
    {
        _material.SetTexture("_TVimage", _image);
    }

}
