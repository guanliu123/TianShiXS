using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StarkSDKSpace.StarkAccount;
using Abelkhan;

namespace GameClient
{
    public class GameClient
    {
        public Client.Client _client;
        public Abelkhan.login_caller _login_Caller;
        public Abelkhan.player_login_caller _player_login_Caller;
        public Abelkhan.player_archive_caller _player_archive_Caller;

        public Abelkhan.player_client_module _player_Client_Module;

        public string _player_hub_name;

        public GameClient() 
        {
            _client = new Client.Client();
            _login_Caller = new login_caller(_client);
            _player_login_Caller = new player_login_caller(_client);
            _player_archive_Caller = new player_archive_caller(_client);

            _player_Client_Module = new player_client_module(_client);
            _player_Client_Module.on_archive_sync += _player_Client_Module_on_archive_sync;
        }

        public void _player_Client_Module_on_archive_sync(UserData obj)
        {
            //... do some thing
        }
    }
}
