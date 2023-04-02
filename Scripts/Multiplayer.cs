using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Multiplayer : Node
{
    ENetMultiplayerPeer peer = new ENetMultiplayerPeer();
    public string CharName = "";
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


        GetTree().ChangeSceneToFile("res://Interface/scenario.tscn");
        //var player = (PackedScene)ResourceLoader.Load("res://player.tscn");
        //GetNode("/root/Scenario").AddChild(player.Instantiate<Player>());

        peer.PeerConnected += Peer_PeerConnected;
    }

    [Rpc(MultiplayerApi.RpcMode.Authority)]
    private void Peer_PeerConnected(long id)
    {
        GD.Print($"{id} conectado");
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Testzin()
    {
        GD.Print("Test");
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void AddPlayer()
    {
        GetTree().ChangeSceneToFile("res://Interface/scenario.tscn");
        //var player = GetNode("/root/Player");
        //GetNode("/root/Scenario").AddChild(player);
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
        Rpc(nameof(SendMethods));
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void SendMethods()
    {
        var x = Rpc(nameof(Testzin));
        var y = Rpc(nameof(AddPlayer));
    }

    public void GetName()
    {
        var nameLineEdit = (Godot.LineEdit)GetNode("/root/Menu/Name");
        var mpinfo = (Multiplayer)GetNode("/root/Multiplayer");
        mpinfo.CharName = nameLineEdit.Text;
    }
}
