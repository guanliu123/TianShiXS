using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MsgPack.Serialization;

namespace Abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

/*this struct code is codegen by abelkhan codegen for c#*/
/*this caller code is codegen by abelkhan codegen for c#*/
    public class player_login_player_login_cb
    {
        private UInt64 cb_uuid;
        private player_login_rsp_cb module_rsp_cb;

        public player_login_player_login_cb(UInt64 _cb_uuid, player_login_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<UserData> on_player_login_cb;
        public event Action<Int32> on_player_login_err;
        public event Action on_player_login_timeout;

        public player_login_player_login_cb callBack(Action<UserData> cb, Action<Int32> err)
        {
            on_player_login_cb += cb;
            on_player_login_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.player_login_timeout(cb_uuid);
            });
            on_player_login_timeout += timeout_cb;
        }

        public void call_cb(UserData info)
        {
            if (on_player_login_cb != null)
            {
                on_player_login_cb(info);
            }
        }

        public void call_err(Int32 err)
        {
            if (on_player_login_err != null)
            {
                on_player_login_err(err);
            }
        }

        public void call_timeout()
        {
            if (on_player_login_timeout != null)
            {
                on_player_login_timeout();
            }
        }

    }

    public class player_login_create_role_cb
    {
        private UInt64 cb_uuid;
        private player_login_rsp_cb module_rsp_cb;

        public player_login_create_role_cb(UInt64 _cb_uuid, player_login_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<UserData> on_create_role_cb;
        public event Action<Int32> on_create_role_err;
        public event Action on_create_role_timeout;

        public player_login_create_role_cb callBack(Action<UserData> cb, Action<Int32> err)
        {
            on_create_role_cb += cb;
            on_create_role_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.create_role_timeout(cb_uuid);
            });
            on_create_role_timeout += timeout_cb;
        }

        public void call_cb(UserData info)
        {
            if (on_create_role_cb != null)
            {
                on_create_role_cb(info);
            }
        }

        public void call_err(Int32 err)
        {
            if (on_create_role_err != null)
            {
                on_create_role_err(err);
            }
        }

        public void call_timeout()
        {
            if (on_create_role_timeout != null)
            {
                on_create_role_timeout();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class player_login_rsp_cb : Common.IModule {
        public Dictionary<UInt64, player_login_player_login_cb> map_player_login;
        public Dictionary<UInt64, player_login_create_role_cb> map_create_role;
        public player_login_rsp_cb(Common.ModuleManager modules)
        {
            map_player_login = new Dictionary<UInt64, player_login_player_login_cb>();
            modules.add_mothed("player_login_rsp_cb_player_login_rsp", player_login_rsp);
            modules.add_mothed("player_login_rsp_cb_player_login_err", player_login_err);
            map_create_role = new Dictionary<UInt64, player_login_create_role_cb>();
            modules.add_mothed("player_login_rsp_cb_create_role_rsp", create_role_rsp);
            modules.add_mothed("player_login_rsp_cb_create_role_err", create_role_err);
        }

        public void player_login_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = UserData.protcol_to_UserData(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_player_login_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void player_login_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _err = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_player_login_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_err);
            }
        }

        public void player_login_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_player_login_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private player_login_player_login_cb try_get_and_del_player_login_cb(UInt64 uuid){
            lock(map_player_login)
            {
                if (map_player_login.TryGetValue(uuid, out player_login_player_login_cb rsp))
                {
                    map_player_login.Remove(uuid);
                }
                return rsp;
            }
        }

        public void create_role_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = UserData.protcol_to_UserData(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_create_role_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void create_role_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _err = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_create_role_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_err);
            }
        }

        public void create_role_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_create_role_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private player_login_create_role_cb try_get_and_del_create_role_cb(UInt64 uuid){
            lock(map_create_role)
            {
                if (map_create_role.TryGetValue(uuid, out player_login_create_role_cb rsp))
                {
                    map_create_role.Remove(uuid);
                }
                return rsp;
            }
        }

    }

    public class player_login_caller {
        public static player_login_rsp_cb rsp_cb_player_login_handle = null;
        private ThreadLocal<player_login_hubproxy> _hubproxy;
        public Client.Client _client_handle;
        public player_login_caller(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            if (rsp_cb_player_login_handle == null)
            {
                rsp_cb_player_login_handle = new player_login_rsp_cb(_client_handle.modulemanager);
            }

            _hubproxy = new ThreadLocal<player_login_hubproxy>();
        }

        public player_login_hubproxy get_hub(string hub_name)
        {
            if (_hubproxy.Value == null)
{
                _hubproxy.Value = new player_login_hubproxy(_client_handle, rsp_cb_player_login_handle);
            }
            _hubproxy.Value.hub_name_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b = hub_name;
            return _hubproxy.Value;
        }

    }

    public class player_login_hubproxy {
        public string hub_name_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b;
        private Int32 uuid_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b = (Int32)RandomUUID.random();

        public Client.Client _client_handle;
        public player_login_rsp_cb rsp_cb_player_login_handle;

        public player_login_hubproxy(Client.Client client_handle_, player_login_rsp_cb rsp_cb_player_login_handle_)
        {
            _client_handle = client_handle_;
            rsp_cb_player_login_handle = rsp_cb_player_login_handle_;
        }

        public player_login_player_login_cb player_login(string token, string nick_name){
            var uuid_ab86d08e_f3b3_5b3e_a2b9_8a2b5c189a51 = (UInt64)Interlocked.Increment(ref uuid_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b);

            var _argv_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b = new ArrayList();
            _argv_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b.Add(uuid_ab86d08e_f3b3_5b3e_a2b9_8a2b5c189a51);
            _argv_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b.Add(token);
            _argv_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b.Add(nick_name);
            _client_handle.call_hub(hub_name_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b, "player_login_player_login", _argv_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b);

            var cb_player_login_obj = new player_login_player_login_cb(uuid_ab86d08e_f3b3_5b3e_a2b9_8a2b5c189a51, rsp_cb_player_login_handle);
            lock(rsp_cb_player_login_handle.map_player_login)
            {                rsp_cb_player_login_handle.map_player_login.Add(uuid_ab86d08e_f3b3_5b3e_a2b9_8a2b5c189a51, cb_player_login_obj);
            }            return cb_player_login_obj;
        }

        public player_login_create_role_cb create_role(string name, string nick_name){
            var uuid_ef86ed88_4838_5896_8241_9edf3c4b6d21 = (UInt64)Interlocked.Increment(ref uuid_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b);

            var _argv_30293c4a_8f5b_307e_a08a_ff76e003f95d = new ArrayList();
            _argv_30293c4a_8f5b_307e_a08a_ff76e003f95d.Add(uuid_ef86ed88_4838_5896_8241_9edf3c4b6d21);
            _argv_30293c4a_8f5b_307e_a08a_ff76e003f95d.Add(name);
            _argv_30293c4a_8f5b_307e_a08a_ff76e003f95d.Add(nick_name);
            _client_handle.call_hub(hub_name_803b03c3_eef6_3b5c_a790_4cd13c6c4e4b, "player_login_create_role", _argv_30293c4a_8f5b_307e_a08a_ff76e003f95d);

            var cb_create_role_obj = new player_login_create_role_cb(uuid_ef86ed88_4838_5896_8241_9edf3c4b6d21, rsp_cb_player_login_handle);
            lock(rsp_cb_player_login_handle.map_create_role)
            {                rsp_cb_player_login_handle.map_create_role.Add(uuid_ef86ed88_4838_5896_8241_9edf3c4b6d21, cb_create_role_obj);
            }            return cb_create_role_obj;
        }

    }
    public class player_archive_cost_strength_cb
    {
        private UInt64 cb_uuid;
        private player_archive_rsp_cb module_rsp_cb;

        public player_archive_cost_strength_cb(UInt64 _cb_uuid, player_archive_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<UserData> on_cost_strength_cb;
        public event Action<Int32> on_cost_strength_err;
        public event Action on_cost_strength_timeout;

        public player_archive_cost_strength_cb callBack(Action<UserData> cb, Action<Int32> err)
        {
            on_cost_strength_cb += cb;
            on_cost_strength_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.cost_strength_timeout(cb_uuid);
            });
            on_cost_strength_timeout += timeout_cb;
        }

        public void call_cb(UserData info)
        {
            if (on_cost_strength_cb != null)
            {
                on_cost_strength_cb(info);
            }
        }

        public void call_err(Int32 err)
        {
            if (on_cost_strength_err != null)
            {
                on_cost_strength_err(err);
            }
        }

        public void call_timeout()
        {
            if (on_cost_strength_timeout != null)
            {
                on_cost_strength_timeout();
            }
        }

    }

    public class player_archive_level_up_cb
    {
        private UInt64 cb_uuid;
        private player_archive_rsp_cb module_rsp_cb;

        public player_archive_level_up_cb(UInt64 _cb_uuid, player_archive_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<UserData> on_level_up_cb;
        public event Action<Int32> on_level_up_err;
        public event Action on_level_up_timeout;

        public player_archive_level_up_cb callBack(Action<UserData> cb, Action<Int32> err)
        {
            on_level_up_cb += cb;
            on_level_up_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.level_up_timeout(cb_uuid);
            });
            on_level_up_timeout += timeout_cb;
        }

        public void call_cb(UserData info)
        {
            if (on_level_up_cb != null)
            {
                on_level_up_cb(info);
            }
        }

        public void call_err(Int32 err)
        {
            if (on_level_up_err != null)
            {
                on_level_up_err(err);
            }
        }

        public void call_timeout()
        {
            if (on_level_up_timeout != null)
            {
                on_level_up_timeout();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class player_archive_rsp_cb : Common.IModule {
        public Dictionary<UInt64, player_archive_cost_strength_cb> map_cost_strength;
        public Dictionary<UInt64, player_archive_level_up_cb> map_level_up;
        public player_archive_rsp_cb(Common.ModuleManager modules)
        {
            map_cost_strength = new Dictionary<UInt64, player_archive_cost_strength_cb>();
            modules.add_mothed("player_archive_rsp_cb_cost_strength_rsp", cost_strength_rsp);
            modules.add_mothed("player_archive_rsp_cb_cost_strength_err", cost_strength_err);
            map_level_up = new Dictionary<UInt64, player_archive_level_up_cb>();
            modules.add_mothed("player_archive_rsp_cb_level_up_rsp", level_up_rsp);
            modules.add_mothed("player_archive_rsp_cb_level_up_err", level_up_err);
        }

        public void cost_strength_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = UserData.protcol_to_UserData(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_cost_strength_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void cost_strength_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _err = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_cost_strength_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_err);
            }
        }

        public void cost_strength_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_cost_strength_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private player_archive_cost_strength_cb try_get_and_del_cost_strength_cb(UInt64 uuid){
            lock(map_cost_strength)
            {
                if (map_cost_strength.TryGetValue(uuid, out player_archive_cost_strength_cb rsp))
                {
                    map_cost_strength.Remove(uuid);
                }
                return rsp;
            }
        }

        public void level_up_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = UserData.protcol_to_UserData(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_level_up_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void level_up_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _err = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_level_up_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_err);
            }
        }

        public void level_up_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_level_up_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private player_archive_level_up_cb try_get_and_del_level_up_cb(UInt64 uuid){
            lock(map_level_up)
            {
                if (map_level_up.TryGetValue(uuid, out player_archive_level_up_cb rsp))
                {
                    map_level_up.Remove(uuid);
                }
                return rsp;
            }
        }

    }

    public class player_archive_caller {
        public static player_archive_rsp_cb rsp_cb_player_archive_handle = null;
        private ThreadLocal<player_archive_hubproxy> _hubproxy;
        public Client.Client _client_handle;
        public player_archive_caller(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            if (rsp_cb_player_archive_handle == null)
            {
                rsp_cb_player_archive_handle = new player_archive_rsp_cb(_client_handle.modulemanager);
            }

            _hubproxy = new ThreadLocal<player_archive_hubproxy>();
        }

        public player_archive_hubproxy get_hub(string hub_name)
        {
            if (_hubproxy.Value == null)
{
                _hubproxy.Value = new player_archive_hubproxy(_client_handle, rsp_cb_player_archive_handle);
            }
            _hubproxy.Value.hub_name_229b670b_b203_3780_89af_1bc6486bd86f = hub_name;
            return _hubproxy.Value;
        }

    }

    public class player_archive_hubproxy {
        public string hub_name_229b670b_b203_3780_89af_1bc6486bd86f;
        private Int32 uuid_229b670b_b203_3780_89af_1bc6486bd86f = (Int32)RandomUUID.random();

        public Client.Client _client_handle;
        public player_archive_rsp_cb rsp_cb_player_archive_handle;

        public player_archive_hubproxy(Client.Client client_handle_, player_archive_rsp_cb rsp_cb_player_archive_handle_)
        {
            _client_handle = client_handle_;
            rsp_cb_player_archive_handle = rsp_cb_player_archive_handle_;
        }

        public player_archive_cost_strength_cb cost_strength(){
            var uuid_20fc04b0_1384_5a5e_a1ce_d79c07d25765 = (UInt64)Interlocked.Increment(ref uuid_229b670b_b203_3780_89af_1bc6486bd86f);

            var _argv_c1f5a2a8_b494_3f28_8814_9be35e02be6a = new ArrayList();
            _argv_c1f5a2a8_b494_3f28_8814_9be35e02be6a.Add(uuid_20fc04b0_1384_5a5e_a1ce_d79c07d25765);
            _client_handle.call_hub(hub_name_229b670b_b203_3780_89af_1bc6486bd86f, "player_archive_cost_strength", _argv_c1f5a2a8_b494_3f28_8814_9be35e02be6a);

            var cb_cost_strength_obj = new player_archive_cost_strength_cb(uuid_20fc04b0_1384_5a5e_a1ce_d79c07d25765, rsp_cb_player_archive_handle);
            lock(rsp_cb_player_archive_handle.map_cost_strength)
            {                rsp_cb_player_archive_handle.map_cost_strength.Add(uuid_20fc04b0_1384_5a5e_a1ce_d79c07d25765, cb_cost_strength_obj);
            }            return cb_cost_strength_obj;
        }

        public player_archive_level_up_cb level_up(){
            var uuid_3d264b5b_1f8b_5dd4_9c9f_37ecbf3a2ab6 = (UInt64)Interlocked.Increment(ref uuid_229b670b_b203_3780_89af_1bc6486bd86f);

            var _argv_b1c410d2_01f8_37cb_9fa7_b185d02f4ace = new ArrayList();
            _argv_b1c410d2_01f8_37cb_9fa7_b185d02f4ace.Add(uuid_3d264b5b_1f8b_5dd4_9c9f_37ecbf3a2ab6);
            _client_handle.call_hub(hub_name_229b670b_b203_3780_89af_1bc6486bd86f, "player_archive_level_up", _argv_b1c410d2_01f8_37cb_9fa7_b185d02f4ace);

            var cb_level_up_obj = new player_archive_level_up_cb(uuid_3d264b5b_1f8b_5dd4_9c9f_37ecbf3a2ab6, rsp_cb_player_archive_handle);
            lock(rsp_cb_player_archive_handle.map_level_up)
            {                rsp_cb_player_archive_handle.map_level_up.Add(uuid_3d264b5b_1f8b_5dd4_9c9f_37ecbf3a2ab6, cb_level_up_obj);
            }            return cb_level_up_obj;
        }

    }

}
