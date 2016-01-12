# Enable -Verbose option
[CmdletBinding()]
param(  [string]$gitHubApiKey, 
        [string]$contents, 
        [string]$versionNumber, 
        [string]$commitId,
        [string]$copyRoot)


# The notes to accompany this release, uses the commit message in this case
$releaseNotes = ""

# Set to true to mark this as a draft release (not visible to users)
$draft = $TRUE
# Set to true to mark this as a pre-release version
$preRelease = $FALSE

Write-Verbose "gitHubApiKey: $gitHubApiKey" -verbose
Write-Verbose "contents: $contents" -verbose
Write-Verbose "versionNumber: $versionNumber" -verbose
Write-Verbose "commitId: $commitId" -verbose
Write-Verbose "copyRoot: $copyRoot" -verbose
Write-Verbose "BUILD_REPOSITORY_NAME: $Env:BUILD_REPOSITORY_NAME" -verbose
Write-Verbose "BUILD_REPOSITORY_URI: $Env:BUILD_REPOSITORY_URI" -verbose

$preRelease = ($Env:PreReleaseTag -ne "")


$releaseData = @{
   tag_name = [string]::Format("v{0}", $versionNumber);
   target_commitish = $commitId;
   name = [string]::Format("v{0}", $versionNumber);
   body = $releaseNotes;
   draft = $draft;
   prerelease = $preRelease;
 }

 $releaseParams = @{
   Uri = "https://api.github.com/repos/$Env:BUILD_REPOSITORY_NAME/releases";
   Method = 'POST';
   Headers = @{
     Authorization = 'Basic ' + [Convert]::ToBase64String(
     [Text.Encoding]::ASCII.GetBytes($gitHubApiKey + ":x-oauth-basic"));
   }
   ContentType = 'application/json';
   Body = (ConvertTo-Json $releaseData -Compress)
 }

 $result = Invoke-RestMethod @releaseParams 

 Write-Verbose $result  -verbose

 # Apply the version to the assembly property files
$files = gci $copyRoot -recurse | 
	?{ $_.PSIsContainer } | 
	foreach { gci -Path $_.FullName -Recurse -include $contents } | Get-Unique
if($files)
{
	Write-Verbose "Will upload $($files.count) files." -verbose
	
	foreach ($file in $files) {
			
			
		if(-not $Disable)
		{

             $uploadUri = $result | Select -ExpandProperty upload_url
             Write-Verbose "Upload URI: $uploadUri"  -verbose
             $uploadUri = $uploadUri -replace '\{\?name,label\}', "?name=$($file.Name)"
             $uploadFile = $file

             $uploadParams = @{
               Uri = $uploadUri;
               Method = 'POST';
               Headers = @{
                 Authorization = 'Basic ' + [Convert]::ToBase64String(
                 [Text.Encoding]::ASCII.GetBytes($gitHubApiKey + ":x-oauth-basic"));
               }
               ContentType = 'application/zip';
               InFile = $uploadFile
             }

             $uresult = Invoke-RestMethod @uploadParams
             Write-Verbose $uresult  -verbose
		}
	}
}
else
{
	Write-Warning "Found no files." -verbose
}




