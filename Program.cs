using System.Text;
using Steamworks;

namespace WebPoisson;

class Program
{
    static Callback<LobbyMatchList_t> Callback_lobbyList;
    static Callback<LobbyDataUpdate_t> Callback_lobbyData;
    static Callback<LobbyCreated_t> Callback_lobbyCreated;
    static void Main(string[] args)
    {
        Callback_lobbyList = Callback<LobbyMatchList_t>.Create(Lobbies.LobbyListGet);
        Callback_lobbyData = Callback<LobbyDataUpdate_t>.Create(Lobbies.LobbyDataGet);
        Callback_lobbyCreated = Callback<LobbyCreated_t>.Create(Lobbies.LobbyCreateGet);

        Lobbies.lobbies = new List<CSteamID>();

        SteamAPI.Init();
        //Console.WriteLine(SteamUser.GetSteamID());
        //SteamMatchmaking.RequestLobbyList();
        Lobbies.CreateLobby(ELobbyType.k_ELobbyTypePublic);
        while (true) {
            SteamAPI.RunCallbacks();
            Thread.Sleep(1000);
            if (Lobbies.LobbyReady) {
                byte[] message = Encoding.UTF8.GetBytes("Hi!");
                SteamMatchmaking.SendLobbyChatMsg((CSteamID)Lobbies.lobby.m_ulSteamIDLobby, message, 7);
                // This doesn't work.
            }
        }
    }
}
