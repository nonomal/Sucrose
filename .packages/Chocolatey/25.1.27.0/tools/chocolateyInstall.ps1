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
	$packageArgs['checksum'] = '5AE1C548CAB95D9251AE07E0E6195FA2DB2D0900DC7A18273F38302E65B3C773'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v25.1.27.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_25.1.27.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = '8ADBEEF7D3648DBDA5486C9423BE7DD993F744C4F4F0B29C2947FFD14540DF92'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v25.1.27.0/Sucrose_Bundle_.NET_Framework_4.8_x64_25.1.27.0.exe'
		} else {
			$packageArgs['checksum'] = '5BF72D293B24B6186D385EABFE1343B2914426166A8E6E22CFA443423200FA8C'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v25.1.27.0/Sucrose_Bundle_.NET_Framework_4.8_x86_25.1.27.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '5BF72D293B24B6186D385EABFE1343B2914426166A8E6E22CFA443423200FA8C'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v25.1.27.0/Sucrose_Bundle_.NET_Framework_4.8_x86_25.1.27.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs