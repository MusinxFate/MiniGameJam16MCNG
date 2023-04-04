using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class Multiplayer : Node
{
    ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
    public string CharName = "";
    Dictionary<long, string> playerList = new Dictionary<long, string>();

    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public void CreateServer()
    {
        peer.CreateServer(4321, 999);
        MultiplayerApi mp = MultiplayerApi.CreateDefaultInterface();
        mp.MultiplayerPeer = peer;
        GetTree().SetMultiplayer(mp);

        var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
        txtLabel.Text = "Hosting...";


        //var packedscenario = ResourceLoader.Load<PackedScene>("res://Interface/scenario.tscn");
        //var scenarioobj = packedscenario.Instantiate<scenario>();
        //GetTree().Root.AddChild(scenarioobj);
        //GetTree().Root.RemoveChild(GetNode("Menu"));

        LoadWorld();

        var scenario = GetTree().Root.GetNode("Scenario");
        PackedScene playerScene = ResourceLoader.Load<PackedScene>("res://player.tscn");

        var spawnpos = scenario.GetNode<Marker2D>("Spawn/Point").Position;
        var player = playerScene.Instantiate<Player>();
        player.syncPos = spawnpos;
        player.Position = spawnpos;
        scenario.AddChild(player);


        peer.PeerConnected += Peer_PeerConnected;
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void RegisterPlayer(string playerName)
    {
        long id = Multiplayer.GetRemoteSenderId();
        playerList.Add(id, playerName);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void Peer_PeerConnected(long id)
    {
        GD.Print($"{id} conectado");
        RegisterPlayer(CharName);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Testzin()
    {
        GD.Print("Test");
    }

    [Rpc(CallLocal = true)]
    public void LoadWorld()
    {
        var scen = ResourceLoader.Load<PackedScene>("res://Interface/scenario.tscn").Instantiate<scenario>();
        GetTree().Root.AddChild(scen);
        GetNode<Control>("/root/Menu").Hide();
    }

    public void JoinServer()
    {
        try
        {
            var test = peer.CreateClient("localhost", 4321);
            if (test == Error.Ok)
            {
                MultiplayerApi mp = MultiplayerApi.CreateDefaultInterface();
                mp.MultiplayerPeer = peer;
                mp.ConnectedToServer += Mp_ConnectedToServer;
                GetTree().SetMultiplayer(mp);

            }
            else
            {
                var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
                txtLabel.Text = "Error while trying to connect.";
            }
        }
        catch (Exception e)
        {
            var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
            txtLabel.Text = "Error while trying to connect.";
            GD.Print($"Error {e.Message}");
        }
    }

    private void Mp_ConnectedToServer()
    {
        var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
        txtLabel.Text = "Connected";
        var id = Multiplayer.GetRemoteSenderId();
        RpcId(id, nameof(LoadWorld));
        Rpc(nameof(BeginGame));
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void BeginGame()
    {
        var scen = GetTree().Root.GetNode<scenario>("Scenario");
        var playerScene = (PackedScene)ResourceLoader.Load("res://player.tscn");

        foreach (var p in playerList)
        {
            var id = Multiplayer.GetRemoteSenderId();
            var pl = playerScene.Instantiate<Player>();
            pl.syncPos = new Vector2(1, 1);
            pl.Name = p.Key.ToString();

            scen.GetNode("Players").AddChild(pl);
        }
    }

    public void GetName()
    {
        var nameLineEdit = (Godot.LineEdit)GetNode("/root/Menu/Name");
        var mpinfo = (Multiplayer)GetNode("/root/Multiplayer");
        mpinfo.CharName = nameLineEdit.Text;
    }
}
