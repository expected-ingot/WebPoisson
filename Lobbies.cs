using Steamworks;

namespace WebPoisson;

class Lobbies
{
    public static List<CSteamID> lobbies;
    public static bool LobbyReady = false;
    public static LobbyCreated_t lobby;


    public static void LobbyListGet(LobbyMatchList_t result)
    {
        Console.WriteLine($"There are {result.m_nLobbiesMatching} lobbies.");
        for (int i = 0; i < result.m_nLobbiesMatching; i++)
        {
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            lobbies.Add(lobbyID);
            SteamMatchmaking.RequestLobbyData(lobbyID);
        }
    }
    public static dynamic GetLobbyKey(CSteamID lobby, string key)
    {
        /*
            Some lobby keys:
            - age_limit: If true, shows 18+ marking in in-game server picker.
            - cap: Max players
            - code: 6 character uppercase alphanumerical lobby code
            - lobby_name: Lobby name. If empty, game shows "{name}'s Lobby"
            - name: Lobby hoster's Steam name
            - public: If true, lobby is shown in the in-game server picker
            - ref: webfishing_gamelobby
            - server_browser_value: No idea what this is
            - type: Either code_only or public. Self explanatory.
            - version: Game version of the hoster.
            - (MODDED) lurefilter: If set to dedicated, lobby shows up as a
            dedicated server when using Lure.
        */
        return SteamMatchmaking.GetLobbyData(lobby, key);
    }
    public static void LobbyDataGet(LobbyDataUpdate_t result)
    {
        for (int i = 0; i < lobbies.Count; i++)
        {
            if (lobbies[i].m_SteamID == result.m_ulSteamIDLobby)
            {
                for (int x = 0; x < SteamMatchmaking.GetLobbyDataCount((CSteamID)lobbies[i].m_SteamID); x++)
                {
                    string key;
                    string value;
                    SteamMatchmaking.GetLobbyDataByIndex((CSteamID)lobbies[i].m_SteamID, x, out key, 255, out value, 255);
                    Console.WriteLine($"Lobby #{i}: {key} = {value}");
                }
                // Console.WriteLine($"Lobby #{i}: {GetLobbyKey(lobbies[i], "lobby_name")} {GetLobbyKey(lobbies[i], "code")} {GetLobbyKey(lobbies[i], "public")}");
            }
        }
    }
    public static void LobbyCreateGet(LobbyCreated_t result)
    {
        Console.WriteLine("Steamworks says we got a lobby.");
        LobbyReady = true;
        lobby = result;
    }
    public static void SetLobbyKey(LobbyCreated_t lobby, string key, string value)
    {
        // The value should just convert into whatever
        SteamMatchmaking.SetLobbyData((CSteamID)lobby.m_ulSteamIDLobby, key, value);
    }

    public static void CreateLobby(ELobbyType type)
    {
        SteamMatchmaking.CreateLobby(type, 25);
        Console.WriteLine("Crealkting lksgoipnytonpky...");
        while (!LobbyReady)
        {
            SteamAPI.RunCallbacks();
            Thread.Sleep(100);
        }
        Console.WriteLine("The lobby is probably created. If it isn't, future me will deal with error handling");
        SetLobbyKey(lobby, "name", "tojmek");
        SetLobbyKey(lobby, "lobby_name", "WebPoisson dedicated server. WIP but feel free to join");
        SetLobbyKey(lobby, "cap", "12");
        SetLobbyKey(lobby, "public", "true");
        SetLobbyKey(lobby, "ref", "webfishing_gamelobby");
        SetLobbyKey(lobby, "type", "public");
        SetLobbyKey(lobby, "version", "1.1");
        SetLobbyKey(lobby, "code", "CHUJUU");
        Console.WriteLine("Set keys! Server should now be visible.");
        // No idea how this makes the server just work but it does
    }
}