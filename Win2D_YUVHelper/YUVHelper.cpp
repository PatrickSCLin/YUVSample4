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
	D3D_FEATURE_LEVEL _d3d_feature_levels[] = {
		D3D_FEATURE_LEVEL_11_1,
		D3D_FEATURE_LEVEL_11_0,
		D3D_FEATURE_LEVEL_10_1,
		D3D_FEATURE_LEVEL_10_0,
	};

	DX::ThrowIfFailed(D3D11CreateDevice(
		nullptr,
		D3D_DRIVER_TYPE::D3D_DRIVER_TYPE_HARDWARE,
		0,
		D3D11_CREATE_DEVICE_FLAG::D3D11_CREATE_DEVICE_BGRA_SUPPORT,
		_d3d_feature_levels,
		ARRAYSIZE(_d3d_feature_levels),
		D3D11_SDK_VERSION,
		&d3d_device,
		&d3d_feature,
		&d3d_context
		));

	ComPtr<IDXGIDevice> _dxgi_device;

	DX::ThrowIfFailed(d3d_device.As(&_dxgi_device));

	win2d_device = CanvasDevice::CreateFromDirect3D11Device(CreateDirect3DDevice(_dxgi_device.Get()));

	ComPtr<ID2D1Device> _d2d_device = GetWrappedResource<ID2D1Device>(win2d_device);

	DX::ThrowIfFailed(_d2d_device.As(&d2d_device));
}

YUVHelper::~YUVHelper()
{

}

void YUVHelper::DrawImage(CanvasDrawingSession^ session, const Platform::Array<byte>^ data, int width, int height)
{
	if (win2d_bitmap == nullptr)
	{
		D3D11_TEXTURE2D_DESC _d3d_texture_desc;

		_d3d_texture_desc.Width = width;

		_d3d_texture_desc.Height = height;

		_d3d_texture_desc.MipLevels = 1;

		_d3d_texture_desc.ArraySize = 1;

		_d3d_texture_desc.Format = DXGI_FORMAT::DXGI_FORMAT_NV12;

		_d3d_texture_desc.Usage = D3D11_USAGE::D3D11_USAGE_DEFAULT;

		_d3d_texture_desc.SampleDesc.Count = 1;

		_d3d_texture_desc.SampleDesc.Quality = 0;

		_d3d_texture_desc.BindFlags = D3D11_BIND_FLAG::D3D11_BIND_SHADER_RESOURCE;

		_d3d_texture_desc.CPUAccessFlags = 0;

		_d3d_texture_desc.MiscFlags = 0;

		D3D11_SUBRESOURCE_DATA _d3d_texture_data;

		_d3d_texture_data.pSysMem = data->Data;

		_d3d_texture_data.SysMemPitch = width;

		ComPtr<ID3D11Texture2D> _d3d_texture;

		DX::ThrowIfFailed(d3d_device->CreateTexture2D(&_d3d_texture_desc, &_d3d_texture_data, &_d3d_texture));

		IDXGISurface* _dxgi_surface;

		DX::ThrowIfFailed(_d3d_texture.Get()->QueryInterface<IDXGISurface>(&_dxgi_surface));

		ComPtr<ID2D1DeviceContext1> _d2d_context = GetWrappedResource<ID2D1DeviceContext1>(session);

		ID2D1DeviceContext2* _d2d_context2;

		DX::ThrowIfFailed(_d2d_context.Get()->QueryInterface<ID2D1DeviceContext2>(&_d2d_context2));

		ComPtr<ID2D1ImageSource> _d2d_image_source;

		DX::ThrowIfFailed(_d2d_context2->CreateImageSourceFromDxgi(&_dxgi_surface, 1, DXGI_COLOR_SPACE_TYPE::DXGI_COLOR_SPACE_YCBCR_FULL_G22_NONE_P709_X601, D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS::D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS_NONE, &_d2d_image_source));

		win2d_bitmap = GetOrCreate<CanvasVirtualBitmap>(Device, _d2d_image_source.Get());
	}

	session->Antialiasing = CanvasAntialiasing::Aliased;

	session->Clear(Colors::Black);

	session->DrawImage(win2d_bitmap);
}