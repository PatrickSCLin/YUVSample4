#pragma once

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
					return GetOrCreate<CanvasDevice>(d2d_device.Get());
				}
			}

			void DrawImage(CanvasDrawingSession^ session, const Platform::Array<byte>^ bytes, int width, int height);

		private:

			D3D_FEATURE_LEVEL d3d_feature;

			ComPtr<ID3D11Device> d3d_device;

			ComPtr<ID3D11DeviceContext> d3d_context;

			ComPtr<ID2D1Device1> d2d_device;

			ComPtr<ID2D1DeviceContext2>d2d_context;

			YUVHelper();

			~YUVHelper();

		};
	}
}