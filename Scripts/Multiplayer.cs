using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class Multiplayer : Node
{
    ENetMultiplayerPeer peer = null;
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
        peer = new ENetMultiplayerPeer();
        peer.CreateServer(4321, 999);
        MultiplayerApi mp = MultiplayerApi.CreateDefaultInterface();
        mp.MultiplayerPeer = peer;
        GetTree().SetMultiplayer(mp);
    }

    public void JoinServer()
    {
        peer = new ENetMultiplayerPeer();
        peer.CreateClient("localhost", 4321);
        MultiplayerApi mp = MultiplayerApi.CreateDefaultInterface();
        mp.MultiplayerPeer = peer;
        GetTree().SetMultiplayer(mp);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void RegisterPlayer(string playerName)
    {
        if (Multiplayer.IsServer())
        {
            CreatePlayer(playerName);
        }

        long id = Multiplayer.GetRemoteSenderId();
        playerList.Add(id, playerName);
    }

    public void CreatePlayer(string playerName)
    {
        var scen = GetTree().Root.GetNode<scenario>("Scenario");
        var playerScene = (PackedScene)ResourceLoader.Load("res://player.tscn");

        var id = Multiplayer.GetUniqueId();
        var pl = playerScene.Instantiate<Player>();
        pl.syncPos = new Vector2(80, 520);
        pl.Name = id.ToString();
        pl.SetPlayerNickname(playerName);

        scen.GetNode("Players").AddChild(pl);
    }

    private void Peer_PeerConnected(long id)
    {
        GD.Print($"{id} conectado");
        RpcId(id, nameof(RegisterPlayer), CharName);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Testzin()
    {
        GD.Print("Test");
    }

    public void LoadWorld()
    {
        var scen = ResourceLoader.Load<PackedScene>("res://Interface/scenario.tscn").Instantiate<scenario>();
        GetTree().Root.AddChild(scen);
        GetNode<Control>("/root/Menu").Hide();
    }



    private void Mp_ConnectedToServer()
    {
        var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
        txtLabel.Text = "Connected";
        var id = Multiplayer.GetRemoteSenderId();
        RpcId(id, nameof(LoadWorld));
    }

    public void BeginGame()
    {
        LoadWorld();

        if (Multiplayer.IsServer())
        {
            CreatePlayer(CharName);
        }

    }

    public void GetName()
    {
        var nameLineEdit = (Godot.LineEdit)GetNode("/root/Menu/Name");
        var mpinfo = (Multiplayer)GetNode("/root/Multiplayer");
        mpinfo.CharName = nameLineEdit.Text;
    }
}