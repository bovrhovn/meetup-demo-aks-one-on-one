<#

 .SYNOPSIS

Compile containers inside the folder containers with az cli

.DESCRIPTION

Compile containers inside the folder containers with az cli and sends tasks to the registry to compile the code
 
.EXAMPLE

PS > Compile-Containers.ps1 

Compile containers inside the folder containers with az cli in resource group monitoring-rg and registry acr-monitoring

.EXAMPLE

PS > Compile-Containers.ps1 -ResourceGroupName "demo-rg" -RegistryName "acr-demo" -FolderName "containers" -TagName "latest" -SourceFolder "src"

Compiles containers inside the folder containers with az cli in resource group monitoring-rg and registry acr-monitoring
with folder name containers, tag name latest and source folder src
    
. LINK

http://github.com/vrhovnik
 
#>
param(
    [Parameter(Mandatory = $false)]
    $ResourceGroupName = "rg-meetup",
    [Parameter(Mandatory = $false)]
    $RegistryName = "meetupsul",
    [Parameter(Mandatory = $false)]
    $FolderName = "Docker",
    [Parameter(Mandatory = $false)]
    $TagName = "latest",
    [Parameter(Mandatory = $false)]
    $SourceFolder = "src",
    [Parameter(Mandatory=$false)]
    [switch]$InstallCli
)
$logPath = "$HOME/Downloads/container-build.log"
Start-Transcript -Path $logPath -Force
if ($InstallCli)
{
    Start-Process Install-AzCli.ps1 -NoNewWindow -Wait
}

Write-Output "Reading registry $RegistryName in Azure"
$registry = Get-AzContainerRegistry -ResourceGroupName $ResourceGroupName -Name $RegistryName
Write-Output "Registry $($registry.Name) has been read"

Write-Output "Reading the folder $FolderName"
Get-ChildItem -Path $FolderName | ForEach-Object {
    $imageName = $_.Name.Split('-')[1]
    $dockerFile = $_.FullName
    Write-Output "Building image $imageName with tag $TagName based on $dockerFile"
    $imageNameWithTag = "$($imageName):$TagName"
    Write-Output "Taging image with $imageNameWithTag"
    Write-Information "Call data with AZ cli as we don't have support in Azure PowerShell for this yet."
    # you can install by providing the switch -InstallCli 
    az acr build --registry $registry.Name --image $imageNameWithTag -f $dockerFile $SourceFolder
}
Write-Output "Building images done"
Stop-Transcript
#read it in notepad
Start-Process "notepad" -ArgumentList $logPath 
