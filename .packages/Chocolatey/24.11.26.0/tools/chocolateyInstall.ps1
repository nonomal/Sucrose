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
	$packageArgs['checksum'] = 'A22FEDAF5DE5F216E4EF406853CE37BB8651D3CF2D86EBFBF36C0C675D6DEA60'
	$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.26.0/Sucrose_Bundle_.NET_Framework_4.8_ARM64_24.11.26.0.exe'
} else {
	if ([Environment]::Is64BitOperatingSystem) {
		if ([System.Environment]::Is64BitProcess) {
			$packageArgs['checksum'] = '382A958DE0B67EA7925DABDFF6852109ED44C8006519D518685BC5648A8D3687'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.26.0/Sucrose_Bundle_.NET_Framework_4.8_x64_24.11.26.0.exe'
		} else {
			$packageArgs['checksum'] = '904CCB2639AB5E97A85C6196C62F15A6282FFEE39BAD58987A3122718E1DFE50'
			$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.26.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.26.0.exe'
		}
	} else {
		$packageArgs['checksum'] = '904CCB2639AB5E97A85C6196C62F15A6282FFEE39BAD58987A3122718E1DFE50'
		$packageArgs['url'] = 'https://github.com/Taiizor/Sucrose/releases/download/v24.11.26.0/Sucrose_Bundle_.NET_Framework_4.8_x86_24.11.26.0.exe'
	}
}

Install-ChocolateyPackage @packageArgs