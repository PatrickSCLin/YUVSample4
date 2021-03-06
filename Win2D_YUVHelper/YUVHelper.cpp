﻿#include "pch.h"
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

IDirect3DSurface^ YUVHelper::CreateDirect3DSurface(int32 dataPtr, int width, int height)
{
	D3D11_TEXTURE2D_DESC _d3d_texture_desc;

	_d3d_texture_desc.Width = width;

	_d3d_texture_desc.Height = height;

	_d3d_texture_desc.MipLevels = 1;

	_d3d_texture_desc.ArraySize = 1;

	_d3d_texture_desc.Format = DXGI_FORMAT::DXGI_FORMAT_NV12;

	_d3d_texture_desc.Usage = D3D11_USAGE::D3D11_USAGE_DYNAMIC;

	_d3d_texture_desc.SampleDesc.Count = 1;

	_d3d_texture_desc.SampleDesc.Quality = 0;

	_d3d_texture_desc.BindFlags = D3D11_BIND_FLAG::D3D11_BIND_SHADER_RESOURCE;

	_d3d_texture_desc.CPUAccessFlags = D3D11_CPU_ACCESS_FLAG::D3D11_CPU_ACCESS_WRITE;

	_d3d_texture_desc.MiscFlags = 0;

	D3D11_SUBRESOURCE_DATA _d3d_texture_data;

	_d3d_texture_data.pSysMem = (void*)IntPtr(dataPtr);

	_d3d_texture_data.SysMemPitch = width;

	ComPtr<ID3D11Texture2D> _d3d_texture;

	DX::ThrowIfFailed(d3d_device->CreateTexture2D(&_d3d_texture_desc, &_d3d_texture_data, &_d3d_texture));

	ComPtr<IDXGISurface> _dxgi_surface;

	DX::ThrowIfFailed(_d3d_texture.As(&_dxgi_surface));

	return Windows::Graphics::DirectX::Direct3D11::CreateDirect3DSurface(_dxgi_surface.Get());
}

CanvasVirtualBitmap^ YUVHelper::CreateCanvasVirtualBimap(CanvasDrawingSession^ session, IDirect3DSurface^ surface)
{
	ComPtr<ID2D1DeviceContext1> _d2d_context = GetWrappedResource<ID2D1DeviceContext1>(session);

	ComPtr<ID2D1DeviceContext2> _d2d_context2;

	DX::ThrowIfFailed(_d2d_context.As(&_d2d_context2));

	ComPtr<IDXGISurface> _dxgi_surface;

	DX::ThrowIfFailed(GetDXGIInterface(surface, _dxgi_surface.GetAddressOf()));

	IDXGISurface* surfaces[1] = { _dxgi_surface.Get() };

	ComPtr<ID2D1ImageSource> _d2d_image_source;

	DX::ThrowIfFailed(_d2d_context2->CreateImageSourceFromDxgi(surfaces, 1, DXGI_COLOR_SPACE_TYPE::DXGI_COLOR_SPACE_YCBCR_FULL_G22_NONE_P709_X601, D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS::D2D1_IMAGE_SOURCE_FROM_DXGI_OPTIONS_LOW_QUALITY_PRIMARY_CONVERSION, &_d2d_image_source));

	CanvasVirtualBitmap^ win2d_bitmap = GetOrCreate<CanvasVirtualBitmap>(Device, _d2d_image_source.Get());

	return win2d_bitmap;
}

CanvasVirtualBitmap^ YUVHelper::CreateCanvasVirtualBimap(CanvasDrawingSession^ session, int32 dataPtr, int width, int height)
{
	IDirect3DSurface^ surface = CreateDirect3DSurface(dataPtr, width, height);

	return CreateCanvasVirtualBimap(session, surface);
}

void YUVHelper::DrawImage(CanvasDrawingSession^ session, IDirect3DSurface^ surface, Rect destinationRect, Rect sourceRect)
{
	CanvasVirtualBitmap^ bitmap = CreateCanvasVirtualBimap(session, surface);

	DrawImage(session, bitmap, destinationRect, sourceRect);
}

void YUVHelper::DrawImage(CanvasDrawingSession^ session, int32 dataPtr, Rect destinationRect, Rect sourceRect)
{
	CanvasVirtualBitmap^ bitmap = CreateCanvasVirtualBimap(session, dataPtr, sourceRect.Width, sourceRect.Height);

	DrawImage(session, bitmap, destinationRect, sourceRect);
}

void  YUVHelper::DrawImage(CanvasDrawingSession^ session, CanvasVirtualBitmap^ bitmap, Rect destinationRect, Rect sourceRect)
{
	session->DrawImage(bitmap, destinationRect, sourceRect);
}

void YUVHelper::UpdateSurface(IDirect3DSurface^ surface, int32 dataPtr)
{
	ComPtr<IDXGISurface> _dxgi_surface;

	DX::ThrowIfFailed(GetDXGIInterface(surface, _dxgi_surface.GetAddressOf()));

	ComPtr<ID3D11Texture2D> _d3d_texture;

	DX::ThrowIfFailed(_dxgi_surface.As(&_d3d_texture));

	D3D11_TEXTURE2D_DESC _d3d_texture_desc;

	_d3d_texture->GetDesc(&_d3d_texture_desc);

	D3D11_MAPPED_SUBRESOURCE _resource;

	d3d_context->Map(_d3d_texture.Get(), 0, D3D11_MAP::D3D11_MAP_WRITE_DISCARD, 0, &_resource);

	memcpy(_resource.pData, (void*)IntPtr(dataPtr), sizeof(byte) * (size_t)(_d3d_texture_desc.Width * _d3d_texture_desc.Height * 1.5));

	d3d_context->Unmap(_d3d_texture.Get(), 0);
}