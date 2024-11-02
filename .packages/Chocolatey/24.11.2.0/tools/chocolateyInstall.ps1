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
	$packageArgs['checksum'] = '3A297912255C5616C7A3C1A6AF02E776E52A251070D00176962DDB10B16B8320'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_24.11.1.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = '4F0C0B67D4A054314D4C5173008BB4A7CD81CCD5B7B3AD1385301DA963209D2C'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x64_24.11.1.0.exe'
		} else {
			$packageArgs['checksum'] = '249798600E315B79BFA76CD22347C42CED80DA95574F8EC24A8E0F943826A3E1'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.1.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '249798600E315B79BFA76CD22347C42CED80DA95574F8EC24A8E0F943826A3E1'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.1.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.1.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs