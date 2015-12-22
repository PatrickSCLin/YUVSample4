#pragma once

using namespace Windows::Foundation;
using namespace Windows::Graphics::DirectX::Direct3D11;
using namespace Microsoft::Graphics::Canvas;
using namespace Microsoft::WRL;

namespace Win2D
{
	namespace YUV
	{
		public ref class YUVHelper sealed
		{

		public:

			static property YUVHelper^ SharedInstance
			{
				YUVHelper^ get()
				{
					static YUVHelper^ SharedInstance = ref new YUVHelper();

					return SharedInstance;
				}
			}

			property CanvasDevice^ Device
			{
				CanvasDevice^ get()
				{
					return win2d_device;
				}
			}

			void DrawImage(CanvasDrawingSession^ session, int32 dataPtr, int width, int height);

		private:

			D3D_FEATURE_LEVEL d3d_feature;

			ComPtr<ID3D11Device> d3d_device;

			ComPtr<ID3D11DeviceContext> d3d_context;

			ComPtr<ID2D1Device1> d2d_device;

			CanvasDevice^ win2d_device;

			CanvasVirtualBitmap^ CreateVirtuaBimapFromBytes(ComPtr<ID2D1DeviceContext1> context, void* data, int width, int height);

			YUVHelper();

			~YUVHelper();

		};
	}
}