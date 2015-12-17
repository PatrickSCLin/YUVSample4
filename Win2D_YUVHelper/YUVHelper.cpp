#include "pch.h"
#include "YUVHelper.h"

using namespace Win2D::YUV;
using namespace Platform;
using namespace Windows::UI;

namespace DX
{
	inline void ThrowIfFailed(HRESULT hr)
	{
		if (FAILED(hr))
		{
			throw Platform::Exception::CreateException(hr);
		}
	}
}

YUVHelper::YUVHelper()
{
	D3D_FEATURE_LEVEL features[] = {
		D3D_FEATURE_LEVEL_11_1,
		D3D_FEATURE_LEVEL_11_0,
		D3D_FEATURE_LEVEL_10_1,
		D3D_FEATURE_LEVEL_10_0,
	};

	DX::ThrowIfFailed(D3D11CreateDevice(
		NULL,
		D3D_DRIVER_TYPE::D3D_DRIVER_TYPE_HARDWARE,
		NULL,
		D3D11_CREATE_DEVICE_FLAG::D3D11_CREATE_DEVICE_BGRA_SUPPORT,
		features,
		ARRAYSIZE(features),
		D3D11_SDK_VERSION,
		&d3d_device,
		&d3d_feature,
		&d3d_context
		));

	ComPtr<IDXGIDevice> dxgiDevice;

	DX::ThrowIfFailed(d3d_device.As(&dxgiDevice));

	D2D1_CREATION_PROPERTIES properties;

	properties.debugLevel = D2D1_DEBUG_LEVEL::D2D1_DEBUG_LEVEL_INFORMATION;

	properties.options = D2D1_DEVICE_CONTEXT_OPTIONS::D2D1_DEVICE_CONTEXT_OPTIONS_NONE;

	properties.threadingMode = D2D1_THREADING_MODE::D2D1_THREADING_MODE_SINGLE_THREADED;

	ComPtr<ID2D1Device> d2dDevice;

	DX::ThrowIfFailed(D2D1CreateDevice(dxgiDevice.Get(), &properties, &d2dDevice));

	DX::ThrowIfFailed(d2dDevice.As(&d2d_device));

	ComPtr<ID2D1DeviceContext1> d2dContext;

	DX::ThrowIfFailed(d2d_device->CreateDeviceContext(D2D1_DEVICE_CONTEXT_OPTIONS::D2D1_DEVICE_CONTEXT_OPTIONS_NONE, &d2dContext));

	DX::ThrowIfFailed(d2dContext.As(&d2d_context));
}

YUVHelper::~YUVHelper()
{

}

void YUVHelper::DrawImage(CanvasDrawingSession^ session, const Platform::Array<byte>^ bytes, int width, int height)
{
	D3D11_TEXTURE2D_DESC desc;

	desc.Width = width;

	desc.Height = height;

	desc.MipLevels = 1;

	desc.ArraySize = 1;

	desc.Format = DXGI_FORMAT::DXGI_FORMAT_NV12;

	desc.Usage = D3D11_USAGE::D3D11_USAGE_DEFAULT;

	desc.SampleDesc.Count = 1;

	desc.SampleDesc.Quality = 0;

	desc.BindFlags = D3D11_BIND_FLAG::D3D11_BIND_SHADER_RESOURCE;

	desc.CPUAccessFlags = 0;

	desc.MiscFlags = 0;

	D3D11_SUBRESOURCE_DATA data;

	data.pSysMem = bytes->Data;

	data.SysMemPitch = width;

	ComPtr<ID3D11Texture2D> texture;

	DX::ThrowIfFailed(d3d_device->CreateTexture2D(&desc, &data, &texture));

	ComPtr<IDXGISurface> surface;

	DX::ThrowIfFailed(texture.As(&surface));

	ComPtr<ID2D1ImageSource> imageSource;

	DX::ThrowIfFailed(d2d_context->CreateImageSourceFromDxgi(&surface, 1, DXGI_COLOR_SPACE_TYPE::DXGI_COLOR_SPACE_YCBCR_FULL_G22_NONE_P709_X601, D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS::D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS_NONE, &imageSource));

	CanvasVirtualBitmap^ bitmap = GetOrCreate<CanvasVirtualBitmap>(Device, imageSource.Get());

	session->Antialiasing = CanvasAntialiasing::Aliased;

	session->Clear(Colors::Black);

	session->DrawImage(bitmap);
}