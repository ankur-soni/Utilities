param($user, $pwd)

$nugetFile = "$ENV:AGENT_HOMEDIRECTORY\agent\worker\tools\nuget.exe"
Write-Output "Looking for nuget.exe in $nugetFile"

if (-not (Test-Path $nugetFile))
{
  Write-Error "nuget.exe could not be located."
  return
}

Write-Output "nuget.exe located"

$cmd = "$nugetFile Sources Add -Name "SilicusPackageStore" -Source http://10.4.1.38:5002/nuget -UserName $user -Password $pwd -StorePasswordInClearText"
Write-Output $cmd
iex $cmd