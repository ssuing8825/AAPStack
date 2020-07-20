- task: AzurePowerShell@4
  inputs:
    azureSubscription: '$(azureSubscription)'
    ScriptType: 'FilePath'
    ScriptPath: ./AAPStackARM/Deploy-AzureResourceGroup.ps1
    ScriptArguments:  -ArtifactStagingDirectory . -StorageAccountName 'AAPStack_SA' -ResourceGroupLocation eastus -TemplateFile ./azuredeploy.json -TemplateParametersFile ./azuredeploy.parameters.json -ResourceGroupName AAPStack
    azurePowerShellVersion: 'LatestVersion'

    ./AAPStackARM/Deploy-AzureResourceGroup.ps1