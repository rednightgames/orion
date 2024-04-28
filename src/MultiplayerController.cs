using System;
using Godot;

public partial class MultiplayerController : Node2D
{
    [Export]
    private int port;

    [Export]
    private string address = "";
    private int Max_Connections = 3;
    private string DEFAULT_IP = "127.0.0.1";

    public Button start_button;
    public Button join_button;
    public Button host_button;
    public Button back_button;

    public LineEdit ip_line,
        port_line,
        name_line;

    private ENetMultiplayerPeer peer;
    private Label log;

    public override void _Ready()
    {
        Random rnd = new Random();
        port = rnd.Next(10000, 65536);
        start_button = GetNode<Button>("StartButton");
        join_button = GetNode<Button>("JoinButton");
        host_button = GetNode<Button>("HostButton");
        back_button = GetNode<Button>("BackButton");
        ip_line = GetNode<LineEdit>("IPLine");
        port_line = GetNode<LineEdit>("PortLine");
        name_line = GetNode<LineEdit>("NameLine");
        start_button.Visible = false;
        log = GetNode<Label>("Label");
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
    }

    public void OnIpLineTextChanged(string new_ip)
    {
        address = new_ip;
    }

    public void OnPortLineTextChanged(int new_port)
    {
        port = new_port;
    }

    private void ConnectionFailed()
    {
        GD.Print("Connection failed");
    }

    private void ConnectedToServer()
    {
        GD.Print("Connection to server");
    }

    private void PeerDisconnected(long id)
    {
        GD.Print("Player disconnect");
        log.Text += "Player disconnect " + id.ToString() + "\n";
    }

    private void PeerConnected(long id)
    {
        GD.Print("Player connect");
        log.Text += "Player connect " + id.ToString() + "\n";
    }

    public void OnJoinButtonDown()
    {
        host_button.Disabled = true;
        join_button.Disabled = true;
        ip_line.Editable = false;
        port_line.Editable = false;
        name_line.Editable = false;
        if (address == "")
        {
            address = DEFAULT_IP;
        }
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateClient(address, port);
        if (error != Error.Ok)
        {
            GD.Print("error cannont host" + error.ToString());
            return;
        }
        peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Connection to " + address);
        log.Text += "Connection to " + address + "\n";
    }

    public void OnHostButtonDown()
    {
        start_button.Visible = true;
        join_button.Disabled = true;
        host_button.Disabled = true;
        ip_line.Editable = false;
        port_line.Editable = false;
        name_line.Editable = false;
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, Max_Connections - 1);
        if (error != Error.Ok)
        {
            GD.Print("error cannont host" + error.ToString());
            return;
        }
        peer.Host.Compress(ENetConnection.CompressionMode.Fastlz);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("waiting for player");
        log.Text += "Waiting for player Port: " + port + "\n";
    }

    public void OnBackButtonPressed()
    {
        if (peer != null)
        {
            peer.Close();
        }
        GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
    }
}
