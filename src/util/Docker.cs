using Newtonsoft.Json.Linq;

namespace SearchAThing.Util;

/// <summary>
/// docker network information
/// </summary>
public class DockerNetworkNfo
{

    /// <summary>
    /// name of network
    /// </summary>            
    public string Name { get; private set; }

    /// <summary>
    /// subnet belonging to this docker network
    /// </summary>            
    public string Subnet { get; private set; }

    /// <summary>
    /// create docker network info object
    /// </summary>
    public DockerNetworkNfo(string networkName, string subnet)
    {
        Name = networkName;
        Subnet = subnet;
    }

    /// <summary>
    /// stringify docker network obj
    /// </summary>
    public override string ToString() => $"name:{Name} subnet:{Subnet}";

}

/// <summary>
/// docker container info
/// used by [ListContainer](/api/SearchAThing.UtilToolkit.html#SearchAThing_UtilToolkit_ListContainers_CancellationToken_System_Boolean_System_Boolean_)
/// </summary>
public class DockerContainerNfo
{

    /// <summary>
    /// container name
    /// </summary>            
    public string Name { get; private set; }

    /// <summary>
    /// container ip address
    /// </summary>            
    public string IPAddress { get; private set; }

    /// <summary>
    /// construct container info obj
    /// </summary>
    public DockerContainerNfo(string name, string ipaddress)
    {
        Name = name;
        IPAddress = ipaddress;
    }

    /// <summary>
    /// stringify container obj
    /// </summary>
    public override string ToString() => $"name:{Name} ip:{IPAddress}";
}

public static partial class Toolkit
{

    #region list networks
    /// <summary>
    /// retrieve list of docker networks
    /// </summary>        
    public static async Task<IReadOnlyList<DockerNetworkNfo>> ListNetworks(CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        var res = new List<DockerNetworkNfo>();

        var cmdres = await ExecRedirect("docker", new[] { "network", "ls", "--format", "'{{.Name}}'" }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0) throw new Exception($"docker execution error: [{cmdres.Error}]");

        foreach (var network in cmdres.Output.Lines().Select(w => w.StripBegin("'").StripEnd("'")))
        {
            cmdres = await ExecRedirect("docker", new[] { "network", "inspect", network }, ct, sudo, verbose);
            if (cmdres.ExitCode != 0) throw new Exception($"docker execution error: [{cmdres.Error}]");

            var jsonObj = JObject.Parse("{cnf: " + cmdres.Output + "}");
            var q = jsonObj!["cnf"]![0]!["IPAM"]!["Config"]!;
            if (q.Count() > 0)
            {
                var subnet = (string)q[0]!["Subnet"]!;
                res.Add(new DockerNetworkNfo(network, subnet));
            }
        }

        return res;
    }
    #endregion

    #region list containers
    /// <summary>
    /// retrieve list of containers
    /// </summary>        
    public static async Task<IReadOnlyList<DockerContainerNfo>> ListContainers(CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        var res = new List<DockerContainerNfo>();

        var cmdres = await ExecRedirect("docker", new[] { "ps", "-a", "--format", "'{{.Names}}'" }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0) throw new Exception($"docker execution error: [{cmdres.Error}]");

        foreach (var container in cmdres.Output.Lines().Select(w => w.StripBegin("'").StripEnd("'")))
        {
            cmdres = await ExecRedirect("docker", new[] { "inspect", container }, ct, sudo, verbose);
            if (cmdres.ExitCode != 0) throw new Exception($"docker execution error: [{cmdres.Error}]");

            var jsonObj = JObject.Parse("{cnf: " + cmdres.Output + "}")!;
            var q = jsonObj["cnf"]![0]!["NetworkSettings"]!["Networks"]!;
            if (q.Count() > 0)
            {
                var ipaddress = (string)q!.First()!.Children()["IPAddress"]!.First()!;
                res.Add(new DockerContainerNfo(container, ipaddress));
            }
        }

        return res;
    }
    #endregion

    #region exec command in container
    /// <summary>
    /// executes given bash command into container
    /// </summary>        
    public static async Task<int> ExecContainerCmd(string containerName, string command, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        var res = await ExecNoRedirect("docker", new[]
        {
                "exec", containerName, "bash", "-c", command
            }, ct, sudo, verbose);

        return res.ExitCode;
    }
    #endregion

    #region run container            
    /// <summary>
    /// create new container from given image
    /// </summary>        
    public static async Task RunContainer(string containerName, string containerNetwork, string containerIp, string containerImage,
        string cpus, string memory, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        if (!File.Exists("/etc/timezone")) throw new Exception($"can't find /etc/timezone");
        var tz = File.ReadAllText("/etc/timezone");

        Console.Write($"Creating [{containerName}] container...");
        var args = new List<string>();
        args.AddRange(new[]
        {
                "run",
                "-d", "-ti",
                $"-e TZ={tz}",
                $"--name={containerName}",
                $"-h \"{containerName}\"",
                $"--network={containerNetwork}"
            });
        if (containerIp != null) args.Add($"--ip={containerIp}");
        args.AddRange(new[]
        {
                "--restart=unless-stopped",
                $"--cpus={cpus}",
                $"--memory={memory}",
                containerImage
            });
        var cmdres = await ExecRedirect("docker", args, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

    #region stop container            
    /// <summary>
    /// stop container
    /// </summary>        
    public static async Task StopContainer(string containerName, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        Console.Write($"Stopping [{containerName}] container...");
        var cmdres = await ExecRedirect("docker", new[] { "stop", containerName }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

    #region remove container
    /// <summary>
    /// remove (stopped) container
    /// </summary>        
    public static async Task RemoveContainer(string containerName, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        Console.Write($"Removing [{containerName}] container...");
        var cmdres = await ExecRedirect("docker", new[] { "rm", containerName }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

    #region remove network
    /// <summary>
    /// remove docker network
    /// </summary>        
    public static async Task RemoveNetwork(string networkName, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        Console.Write($"Removing [{networkName}] network...");
        var cmdres = await ExecRedirect("docker", new[] { "network", "rm", networkName }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

    #region create network
    /// <summary>
    /// create docker network
    /// </summary>        
    public static async Task CreateNetwork(string networkName, string subnet, CancellationToken ct, bool sudo = false, bool verbose = false)
    {
        Console.Write($"Creating [{networkName}] network...");
        var cmdres = await ExecRedirect("docker", new[] { "network", "create", $"--subnet={subnet}", networkName }, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

    #region build image
    /// <summary>
    /// build docker image
    /// </summary>        
    public static async Task BuildImage(string dockerImageName, string dockerSourceDir, CancellationToken ct,
        bool sudo = false, bool verbose = false, string[]? extra_args = null)
    {
        Console.WriteLine($"Creating [{dockerImageName}] docker image...");

        var args = new List<string>()
            {
                "build",
                "-t", dockerImageName
            };
        if (extra_args != null) args.AddRange(extra_args);
        args.Add("-f");
        args.Add($"{dockerSourceDir}/Dockerfile");
        args.Add(dockerSourceDir);
        var cmdres = await ExecNoRedirect("docker", args, ct, sudo, verbose);
        if (cmdres.ExitCode != 0)
        {
            Console.WriteLine("ERROR");
            throw new Exception($"docker execution error: [{cmdres.Error}]");
        }
        else
            Console.WriteLine("OK");
    }
    #endregion

}