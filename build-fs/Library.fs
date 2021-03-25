namespace Build 

open System
open Nuke.Common
open Nuke.Common.Tooling
open System.ComponentModel

[<TypeConverter(typeof<Configuration>)>]
type Configuration() =
    inherit Enumeration()

    static member Debug = new Configuration(Value = "Debug");
    static member Release = new Configuration(Value = "Release");

    static member public op_Implicit(configuration: Configuration) : string = configuration.Value

type Build () = 
    inherit NukeBuild()

    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    static member public Main () = Build.Execute<Build>([| |])

    [<Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")>]
    member x.Configuration = if Build.IsLocalBuild then Configuration.Debug else Configuration.Release

    member x.Clean: Target = Target(fun t ->t.Before(x.Restore).Executes())

    member x.Restore: Target = Target (fun t -> t.Executes())

    member x.Compile: Target = Target (fun t -> t.DependsOn(x.Restore).Executes())
