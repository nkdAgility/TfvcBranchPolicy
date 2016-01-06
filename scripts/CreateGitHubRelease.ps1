# Enable -Verbose option
[CmdletBinding()]
param([string]$gitHubApiKey, [string]$artifact)

# The version number for this release
$versionNumber = $Env:GITVERSION_SemVer
# The Commit SHA for corresponding to this release
$commitId = $Env:GITVERSION_Sha
# The notes to accompany this release, uses the commit message in this case
$releaseNotes = $Env:APPVEYOR_REPO_COMMIT_MESSAGE
# The folder artifacts are built to
$artifactOutputDirectory = $Env:BUILD_SOURCESDIRECTORY

# Set to true to mark this as a draft release (not visible to users)
$draft = $TRUE
# Set to true to mark this as a pre-release version
$preRelease = $TRUE


$releaseData = @{
   tag_name = [string]::Format("v{0}", $versionNumber);
   target_commitish = $commitId;
   name = [string]::Format("v{0}", $versionNumber);
   body = $releaseNotes;
   draft = $draft;
   prerelease = $preRelease;
 }

 $releaseParams = @{
   Uri = "https://api.github.com/repos/nkdAgility/TfvcBranchPolicy/releases";
   Method = 'POST';
   Headers = @{
     Authorization = 'Basic ' + [Convert]::ToBase64String(
     [Text.Encoding]::ASCII.GetBytes($gitHubApiKey + ":x-oauth-basic"));
   }
   ContentType = 'application/json';
   Body = (ConvertTo-Json $releaseData -Compress)
 }

 $result = Invoke-RestMethod @releaseParams 
 $uploadUri = $result | Select -ExpandProperty upload_url
 $uploadUri = $uploadUri -replace '\{\?name\}', "?name=$artifact"
 $uploadFile = Join-Path -path $artifactOutputDirectory -childpath $artifact

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

 $result = Invoke-RestMethod @uploadParams
