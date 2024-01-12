# meetup demo: Azure Kubernetes Service 1 on 1

Demos for meetup about [Azure Kubernetes Service](https://learn.microsoft.com/en-us/azure/aks/intro-kubernetes) and how
to operate the heavy machinery.

## Prerequisites

1. An active [Azure](https://www.azure.com) subscription - [MSDN](https://my.visualstudio.com) or trial
   or [Azure Pass](https://microsoftazurepass.com) is fine - you can also do all of the work
   in [Azure Shell](https://shell.azure.com) (all tools installed) and by
   using [Github Codespaces](https://docs.github.com/en/codespaces/developing-in-codespaces/creating-a-codespace)
2. [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/) installed to work with Azure
3. [GitHub](https://github.com/) account (sign-in or join [here](https://github.com/join)) - how to authenticate with
   GitHub
   available [here](https://docs.github.com/en/get-started/quickstart/set-up-git#authenticating-with-github-from-git)
4. [RECOMMENDATION] [PowerShell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed - we do recommend an editor like [Visual Studio Code](https://code.visualstudio.com) to be able to write
   scripts, YAML pipelines and connect to repos to submit changes.
5. [OPTIONAL] GitHub CLI installed to work with GitHub - [how to install](https://cli.github.com/manual/installation)
6. [OPTIONAL] [Github GUI App](https://desktop.github.com/) for managing changes and work
   on [forked](https://docs.github.com/en/get-started/quickstart/fork-a-repo) repo
7. [OPTIONAL] [Windows Terminal](https://learn.microsoft.com/en-us/windows/terminal/install)

If you will be working on your local machines, you will need to have:

1. [Powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.2)
   installed
2. git installed - instructions step by step [here](https://docs.github.com/en/get-started/quickstart/set-up-git)
3. [.NET](https://dot.net) installed to run the application if you want to run it
4. an editor (besides notepad) to see and work with code, yaml, scripts and more (for
   example [Visual Studio Code](https://code.visualstudio.com))

## Scripts

Scripts are available in [scripts folder](./Scripts). The scripts are written
in [PowerShell](https://docs.microsoft.com/en-us/powershell/scripting/overview?view=powershell-7.2).

1. [Add-DirToSystemEnv.ps1](./Scripts/Add-DirToSystemEnv.ps1) - adds a directory to the system environment variable
   PATH
2. [Install-AZCLI.ps1](./Scripts/Install-AZCLI.ps1) - installs Azure CLI
3. [Install-Bicep.ps1](./Scripts/Install-Bicep.ps1) - installs Bicep language

## Demo code structure

# Additional information and links

1. [Kubernetes Api Client Libraries](https://github.com/kubernetes-client) and [3rd party community-maintained client libraries](https://kubernetes.io/docs/reference/using-api/client-libraries/#community-maintained-client-libraries)
2. [Kubernetes Api Overview](https://kubernetes.io/docs/reference/using-api/) and [controlling access to cluster](https://kubernetes.io/docs/concepts/security/controlling-access/)
3. [Kubeconfig view](https://kubernetes.io/docs/concepts/configuration/organize-cluster-access-kubeconfig/)
4. [Setup kubectl](https://kubernetes.io/docs/tasks/tools/install-kubectl/)
5. [Power tools for kubectl](https://github.com/ahmetb/kubectx)
6. [Portainer](https://www.portainer.io/installation/)
7. [Azure Kubernetes Service](https://docs.microsoft.com/en-us/azure/aks/)

# CREDITS

In this demo, we used the following 3rd party libraries and solutions:
1. [Spectre Console](https://github.com/spectresystems/spectre.console/)
2. [C# managed library for Kubernetes](https://github.com/kubernetes-client/csharp)

# Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
