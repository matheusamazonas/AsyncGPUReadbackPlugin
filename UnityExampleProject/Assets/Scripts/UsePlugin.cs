using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using System.IO;
using AsyncGPUReadbackPluginNs;

/// <summary>
/// Exemple of usage inspirated from https://github.com/keijiro/AsyncCaptureTest/blob/master/Assets/AsyncCapture.cs
/// </summary>
public class UsePlugin : MonoBehaviour
{
	[SerializeField] RawImage _uiImage;

	Queue<AsyncGPUReadbackPluginRequest> _requests = new Queue<AsyncGPUReadbackPluginRequest>();

	bool _inited = false;

	void Start()
	{
		Invoke("_Init", 5f);
	}

	void _Init()
	{
		_inited = true;
	}

	void Update()
	{
		if(!_inited){return;}

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
				// Get data from the request when it's done
				byte[] buffer = req.GetRawData();

				// Save the image
				Camera cam = GetComponent<Camera>();
				SaveBitmap(buffer, cam.pixelWidth, cam.pixelHeight);

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

	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if(!_inited) return;


		Graphics.Blit(source, destination);

		if (Time.frameCount % 60 == 0)
		{
			if (_requests.Count < 8)
				_requests.Enqueue(AsyncGPUReadbackPlugin.Request(source));
			else
				Debug.LogWarning("Too many requests.");
		}
	}

	void SaveBitmap(byte[] buffer, int width, int height)
	{

		Debug.Log($"Write to file : {buffer.Length}");

		// var tex = new Texture2D(width, height, TextureFormat.RGBAHalf, false);
		var tex = new Texture2D(width, height, TextureFormat.ARGB32, false);
		tex.LoadRawTextureData(buffer);
		tex.Apply();

		
		// File.WriteAllBytes("test.png", ImageConversion.EncodeToPNG(tex));
		
		Texture2D texture2 = new Texture2D(width, height, TextureFormat.RGB24, false);
		texture2.SetPixels(tex.GetPixels());
		texture2.Apply();
		_uiImage.texture = texture2;

		
		Destroy(tex);
	}
}
