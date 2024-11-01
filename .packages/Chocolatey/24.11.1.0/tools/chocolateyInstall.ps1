$ErrorActionPreference = 'Stop'

Get-Process 'Sucrose*' | % {
	Write-Output ('Closing: {0}' -f $_.ProcessName)
	Stop-Process -InputObject $_ -Force
}

$packageArgs = @{
	packageName    = 'SucroseWallpaperEngine'
	checksumType   = 'sha256'
	fileType       = 'exe'
	silentArgs     = '/s'
	validExitCodes = @(0)
}

if ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') {
	$packageArgs['checksum'] = 'E541410FA40CA01656B1780A2CE300F16AFBE10B2B7CE4A329AE7DE92259D4A6'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_24.11.1.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = '70257F7834E2FB3D71761A86784A0F8D4419AADA9E387276385DBF8B86186BE1'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x64_24.11.1.0.exe'
		} else {
			$packageArgs['checksum'] = '8BBF307A83B9C551C6FFEFF38BFF6C13C2293FE87B6773278199DDF9B02FD24C'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.1.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '8BBF307A83B9C551C6FFEFF38BFF6C13C2293FE87B6773278199DDF9B02FD24C'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.1.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs