using Pulumi;
using Pulumi.AzureNative.OperationalInsights;
using Pulumi.AzureNative.OperationalInsights.Inputs;
using Pulumi.AzureNative.Resources;
using Deployment = Pulumi.Deployment;

class MyStack : Stack
{
    public MyStack()
    {
        var resourceGroup = new ResourceGroup(
            name: "mews-test-pulumi",
            args: new ResourceGroupArgs
            {
                Location = "westeurope"
            }
        );
        var workspace = new Workspace(
            name: "mews-test-workspace",
            args: new WorkspaceArgs
            {
                ResourceGroupName = resourceGroup.Name,
                Location = "westeurope",
                WorkspaceName = "mews-test-workspace",
                Sku = new WorkspaceSkuArgs
                {
                    Name = WorkspaceSkuNameEnum.PerGB2018
                },
                Features = new WorkspaceFeaturesArgs
                {
                    EnableLogAccessUsingOnlyResourcePermissions = true
                },
                PublicNetworkAccessForIngestion = PublicNetworkAccessType.Enabled,
                PublicNetworkAccessForQuery = PublicNetworkAccessType.Enabled,
                RetentionInDays = 31,
                WorkspaceCapping = new WorkspaceCappingArgs
                {
                    DailyQuotaGb = 2
                }
            },
            options: new CustomResourceOptions
            {
                Protect = true
            }
        );
    }
}

class Program
{
    static Task<int> Main(string[] args) => Deployment.RunAsync<MyStack>();
}