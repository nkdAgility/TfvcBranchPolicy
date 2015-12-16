# Enable -Verbose option
[CmdletBinding()]
param([string]$PublishVersion)

Write-Verbose "PublishVersion: $PublishVersion" -verbose
Write-Verbose "PublishVersion: $GitVersion_AssemblySemVer" -verbose

if ($PublishVersion -eq "")
{
	$PublishVersion = $GitVersion_AssemblySemVer
}
	
# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$VersionRegex = "\d+\.\d+\.\d+\.\d+"
	
# Make sure path to source code directory is available
if (-not $Env:BUILD_SOURCESDIRECTORY)
{
	Write-Error ("BUILD_SOURCESDIRECTORY environment variable is missing.")
	exit 1
}
elseif (-not (Test-Path $Env:BUILD_SOURCESDIRECTORY))
{
	Write-Error "BUILD_SOURCESDIRECTORY does not exist: $Env:BUILD_SOURCESDIRECTORY"
	exit 1
}
Write-Verbose "BUILD_SOURCESDIRECTORY: $Env:BUILD_SOURCESDIRECTORY"

# Apply the version to the assembly property files
$files = gci $Env:BUILD_SOURCESDIRECTORY -recurse | 
	?{ $_.PSIsContainer } | 
	foreach { gci -Path $_.FullName -Recurse -include *.vsixmanifest }
if($files)
{
	Write-Verbose "Will apply $PublishVersion to $($files.count) files." -verbose
	
	foreach ($file in $files) {
			
			
		if(-not $Disable)
		{
			$filecontent = Get-Content($file)
			attrib $file -r
			$filecontent -replace $VersionRegex, $PublishVersion | Out-File $file -Encoding utf8
			Write-Verbose "$file.FullName - version applied" -verbose
		}
	}
}
else
{
	Write-Warning "Found no files." -verbose
}


