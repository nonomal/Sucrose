$ErrorActionPreference = 'Stop'

Get-Process 'Sucrose*' | % {
	Write-Output ('Closing: {0}' -f $_.ProcessName)
	Stop-Process -InputObject $_ -Force
}

$packageArgs = @{
	packageName    = 'Sucrose Wallpaper Engine'
	checksumType   = 'sha256'
	fileType       = 'exe'
	silentArgs     = '/s'
	validExitCodes = @(0)
}

if ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') {
	$packageArgs['checksum'] = '782FF52E49F3B4DE9C9F16142B1E0EDE9DE73F725F78519DC8AAB2679BA83317'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.8.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_24.11.8.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = 'EDF91879921C448D131F5744F11FB3FD621203DE2DD4BC4BAFFDFC41F2C6D29D'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.8.0/Sucrose_Bundle_.NET_Framework_4.8_x64_24.11.8.0.exe'
		} else {
			$packageArgs['checksum'] = '40FDE0FB0C1F5C35CF30ED959B5A937C73C71C0588744D5AF2552D6C9D1AB438'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.8.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.8.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '40FDE0FB0C1F5C35CF30ED959B5A937C73C71C0588744D5AF2552D6C9D1AB438'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.8.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.8.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs