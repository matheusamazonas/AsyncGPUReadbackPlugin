using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using AsyncGPUReadbackPluginNs;
using UnityEngine.Experimental.Rendering;

/// <summary>
/// Exemple of usage inspirated from https://github.com/keijiro/AsyncCaptureTest/blob/master/Assets/AsyncCapture.cs
/// </summary>
public class UsePlugin : MonoBehaviour
{
    #region Unity editor fields

    [SerializeField] private Image _image;
    [SerializeField] private Camera _camera;

    #endregion

    #region Fields

    private RenderTexture _renderTexture;
    private Texture2D _texture;
    private readonly Queue<AsyncGPUReadbackPluginRequest> _requests = new Queue<AsyncGPUReadbackPluginRequest>();

    #endregion

    #region Setup

    private void Start()
    {
        _renderTexture = new RenderTexture(400, 300, 32, GraphicsFormat.R8G8B8A8_UNorm);
        _texture = new Texture2D(400, 300, TextureFormat.RGBA32, false, true);
        _image.sprite = Sprite.Create(_texture, new Rect(0, 0, 400, 300), Vector2.zero);
        _camera.targetTexture = _renderTexture;
    }

    #endregion

    #region Update

    private void Update()
    {
        RenderTexture.active = _renderTexture;
        RenderTexture.active = null;
        
        if (_requests.Count < 8)
            _requests.Enqueue(AsyncGPUReadbackPlugin.Request(_renderTexture));
        else
            Debug.LogWarning("Too many requests.");

        while (_requests.Count > 0)
        {
            var req = _requests.Peek();

            // You need to explicitly ask for an update regularly
            req.Update();

            if (req.hasError)
            {
                Debug.LogError("GPU readback error detected.");
                req.Dispose();
                _requests.Dequeue();
            }
            else if (req.done)
            {
                var data = req.GetNativeRawData();

                _texture.LoadRawTextureData(data);
                _texture.Apply(false);

                // You need to explicitly Dispose data after using them
                req.Dispose();

                _requests.Dequeue();
            }
            else
            {
                break;
            }
        }
    }
    
    #endregion

}
