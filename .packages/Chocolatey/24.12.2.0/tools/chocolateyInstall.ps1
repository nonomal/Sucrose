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
	$packageArgs['checksum'] = '591656718233A282552CA9D2CA70EDE9C18DA386F3DB304FFA87E31D19CBE706'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.12.2.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_24.12.2.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = 'B74BA0003F36A532C700927FE2F2BB8F580AE72C4AE9DFECF45CA04422C067FE'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.12.2.0/Sucrose_Bundle_.NET_Framework_4.8_x64_24.12.2.0.exe'
		} else {
			$packageArgs['checksum'] = '0DB5825701A95611482C861AE8E1F98F20F1F9BF95844BF13DCD8E1B059E1F45'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.12.2.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.12.2.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '0DB5825701A95611482C861AE8E1F98F20F1F9BF95844BF13DCD8E1B059E1F45'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.12.2.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.12.2.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs