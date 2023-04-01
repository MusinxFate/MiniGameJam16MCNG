using Godot;
using System;

public partial class Multiplayer : Node
{
    ENetMultiplayerPeer peer = new ENetMultiplayerPeer();

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

        peer.PeerConnected += Peer_PeerConnected;
    }

    private void Peer_PeerConnected(long id)
    {
        GD.Print($"{id} conectado");
    }

    public void JoinServer()
    {
        try
        {
            peer.CreateClient("localhost", 4321);
            MultiplayerApi mp = MultiplayerApi.CreateDefaultInterface();
            mp.MultiplayerPeer = peer;
            GetTree().SetMultiplayer(mp);
            var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
            txtLabel.Text = "Connected";
        }
        catch (Exception e)
        {
            var txtLabel = (Label)GetNode("/root/Menu/LblMessageStatus");
            txtLabel.Text = "Error while trying to connect.";
            GD.Print($"Error {e.Message}");
        }
    }
}
