module Program

open Pulumi.FSharp.NamingConventions.Azure
open Pulumi.FSharp.AzureNative.Resources
open Pulumi.FSharp
open Pulumi

Deployment.run (fun () ->
    let rg =
        resourceGroup {
            name $"rg-dev-{Deployment.Instance.StackName}-{Region.shortName}-001"
        }
    
    dict []
)