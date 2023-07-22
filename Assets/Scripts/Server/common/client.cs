using Abelkhan;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityWebSocket;
using UnityEngine;

namespace Client
{
    class GateProxy
    {
        private Abelkhan.Ichannel _ch;
        private Abelkhan.client_call_gate_caller _client_call_gate_caller;

        public GateProxy(Abelkhan.Ichannel ch)
        {
            _ch = ch;
            _client_call_gate_caller = new Abelkhan.client_call_gate_caller(ch, Abelkhan.ModuleMgrHandle._modulemng);
        }

        public Action<ulong> onGateTime;
        public Action<Abelkhan.Ichannel> onGateDisconnect;
        public void heartbeats()
        {
            _client_call_gate_caller.heartbeats().callBack((ulong _svr_timetmp)=> {
                onGateTime?.Invoke(_svr_timetmp);
            }, ()=> {}).timeout(5000, ()=> {
                onGateDisconnect?.Invoke(_ch);
            });
        }

        public void get_hub_info(string hub_type, Action<Abelkhan.hub_info> cb)
        {
            _client_call_gate_caller.get_hub_info(hub_type).callBack((hub_info) => {
                cb(hub_info);
            }, () => { 
                
            }).timeout(5000, ()=> {
                onGateDisconnect?.Invoke(_ch);
            });
        }

        public void call_hub(string hub, string func, ArrayList argv)
        {
            var _serialization = MsgPack.Serialization.MessagePackSerializer.Get<ArrayList>();
            using (MemoryStream st = new MemoryStream())
            {
                var _event = new ArrayList();
                _event.Add(func);
                _event.Add(argv);
                _serialization.Pack(st, _event);
                st.Position = 0;

                _client_call_gate_caller.forward_client_call_hub(hub, st.ToArray());
            }
        }
    }

    class HubProxy
    {
        public string _hub_name;
        public string _hub_type;

        private Abelkhan.Ichannel _ch;
        private Abelkhan.client_call_hub_caller _client_call_hub_caller;

        public HubProxy(string hub_name, string hub_type, Abelkhan.Ichannel ch)
        {
            _hub_name = hub_name;
            _hub_type = hub_type;

            _ch = ch;
            _client_call_hub_caller = new Abelkhan.client_call_hub_caller(ch, Abelkhan.ModuleMgrHandle._modulemng);
        }

        public void connect_hub(string cuuid)
        {
            _client_call_hub_caller.connect_hub(cuuid);
        }

        public Action<string, ulong> onHubTime;
        public Action<Abelkhan.Ichannel> onHubDisconnect;
        public void heartbeats()
        {
            _client_call_hub_caller.heartbeats().callBack((ulong _hub_timetmp) =>
            {
                onHubTime?.Invoke(_hub_name, _hub_timetmp);
            }, () => { }).timeout(5000, () =>
            {
                onHubDisconnect?.Invoke(_ch);
            });
        }

        public void call_hub(string func, ArrayList argv)
        {
            var _serialization = MsgPack.Serialization.MessagePackSerializer.Get<ArrayList>();
            using (MemoryStream st = new MemoryStream())
            {
                var _event = new ArrayList();
                _event.Add(func);
                _event.Add(argv);
                _serialization.Pack(st, _event);
                st.Position = 0;

                _client_call_hub_caller.call_hub(st.ToArray());
            }
        }
    }

	public class Client
	{
        public event Action onGateDisConnect;
        public event Action<string> onHubDisConnect;

        public event Action<ulong> onGateTime;
        public event Action<string, ulong> onHubTime;

        public String uuid;
        public Service.Timerservice timer;
        public Common.ModuleManager modulemanager;

        public string current_hub;

        private Int64 _heartbeats;

        private GateProxy _gateproxy;
        private Dictionary<string, HubProxy> _hubproxy_set;
        private Dictionary<Abelkhan.Ichannel, HubProxy> _ch_hubproxy_set;

        private List<Abelkhan.Ichannel> add_chs;
        private List<Abelkhan.Ichannel> remove_chs;

        private Abelkhan.gate_call_client_module _gate_call_client_module;
        private Abelkhan.hub_call_client_module _hub_call_client_module;

        public Client()
		{
			timer = new Service.Timerservice();
			modulemanager = new Common.ModuleManager();

            _heartbeats = timer.refresh();

            _hubproxy_set = new Dictionary<string, HubProxy>();
            _ch_hubproxy_set = new Dictionary<Abelkhan.Ichannel, HubProxy>();

            add_chs = new List<Abelkhan.Ichannel>();
            remove_chs = new List<Abelkhan.Ichannel>();

            timer.addticktime(5000, heartbeats);

            _gate_call_client_module = new Abelkhan.gate_call_client_module(Abelkhan.ModuleMgrHandle._modulemng);
            _gate_call_client_module.on_ntf_cuuid += ntf_cuuid;
            _gate_call_client_module.on_call_client += gate_call_client;

            _hub_call_client_module = new Abelkhan.hub_call_client_module(Abelkhan.ModuleMgrHandle._modulemng);
            _hub_call_client_module.on_call_client += hub_call_client;
        }

        private void ntf_cuuid(string _uuid)
        {
            Debug.Log("ntf_cuuid begin!");
            uuid = _uuid;

            Debug.Log("ntf_cuuid call onGateConnect begin!");
            onGateConnect?.Invoke();
            Debug.Log("ntf_cuuid call onGateConnect end!");
        }

        private void gate_call_client(string hub_name, byte[] rpc_argv)
        {
            using (var st = new MemoryStream())
            {
                st.Write(rpc_argv, 0, rpc_argv.Length);
                st.Position = 0;

                var _serialization = MsgPack.Serialization.MessagePackSerializer.Get<ArrayList>();
                var _event = _serialization.Unpack(st);

                var func = ((MsgPack.MessagePackObject)_event[0]).AsString();
                var argvs = ((MsgPack.MessagePackObject)_event[1]).AsList();

                current_hub = hub_name;
                modulemanager.process_module_mothed(func, argvs);
                current_hub = "";
            }
        }

        private void hub_call_client(byte[] rpc_argv)
        {
            using (var st = new MemoryStream())
            {
                st.Write(rpc_argv, 0, rpc_argv.Length);
                st.Position = 0;

                var _serialization = MsgPack.Serialization.MessagePackSerializer.Get<ArrayList>();
                var _event = _serialization.Unpack(st);

                var func = ((MsgPack.MessagePackObject)_event[0]).AsString();
                var argvs = ((MsgPack.MessagePackObject)_event[1]).AsList();

                var _hubproxy = _ch_hubproxy_set[_hub_call_client_module.current_ch.Value];

                current_hub = _hubproxy._hub_name;
                modulemanager.process_module_mothed(func, argvs);
                current_hub = "";
            }
        }

        private void heartbeats(long tick)
        {
            if (_gateproxy != null)
            {
                _gateproxy.heartbeats();
            }

            foreach (var _hubproxy in _hubproxy_set)
            {
                _hubproxy.Value.heartbeats();
            }

            timer.addticktime(5000, heartbeats);
        }

        public void get_hub_info(string hub_type, Action<Abelkhan.hub_info> cb)
        {
            _gateproxy?.get_hub_info(hub_type, cb);
        }

        public void call_hub(string hub_name, string func, ArrayList argv)
        {
            if (_hubproxy_set.TryGetValue(hub_name, out HubProxy _hubproxy))
            {
                _hubproxy.call_hub(func, argv);
                return;
            }

            if (_gateproxy != null)
            {
                _gateproxy.call_hub(hub_name, func, argv);
            }
        }

        public event Action onGateConnect;
        public event Action onGateConnectFaild;
        public void connect_gate(string wss, long timeout)
        {
            Debug.Log("connect_gate begin");
            connect(wss, timeout, (is_conn, ch) => {
                if (is_conn && ch != null)
                {
                    _gateproxy = new GateProxy(ch);
                    _gateproxy.onGateDisconnect += (_ch) =>
                    {
                        lock (remove_chs)
                        {
                            remove_chs.Add(ch);
                        }
                        _gateproxy = null;

                        onGateDisConnect?.Invoke();
                    };
                    _gateproxy.onGateTime += (tick) =>
                    {
                        onGateTime?.Invoke(tick);
                    };
                }
                else
                {
                    onGateConnectFaild?.Invoke();
                }
            });
        }

        public event Action<string> onHubConnect;
        public event Action<string> onHubConnectFaild;
        public void connect_hub(string hub_name, string hub_type, string wss, long timeout)
        {
            connect(wss, timeout, (is_conn, ch) => { 
                if (is_conn && ch != null)
                {
                    var _hubproxy = new HubProxy(hub_name, hub_type, ch);
                    _hubproxy.onHubDisconnect += (_ch) =>
                    {
                        lock (remove_chs)
                        {
                            remove_chs.Add(_ch);
                        }

                        if (_ch_hubproxy_set.ContainsKey(_ch))
                        {
                            var _proxy = _ch_hubproxy_set[_ch];
                            _hubproxy_set.Remove(_proxy._hub_name);
                            _ch_hubproxy_set.Remove(_ch);
                        }

                        onHubDisConnect?.Invoke(hub_name);
                    };
                    _hubproxy.onHubTime += (_hub_name, tick) =>
                    {
                        onHubTime?.Invoke(_hub_name, tick);
                    };
                    _hubproxy.connect_hub(uuid);

                    _hubproxy_set.Add(hub_name, _hubproxy);
                    _ch_hubproxy_set.Add(ch, _hubproxy);

                    onHubConnect?.Invoke(hub_name);
                }
                else
                {
                    onHubConnectFaild?.Invoke(hub_name);
                }
            });
        }

        private void connect(string wss, long timeout, Action<bool, Abelkhan.Ichannel> cb)
        {
            Debug.Log($"connect {wss} begin!");
            try
            {
                var s = new WebSocket(wss);
                var ch = new Abelkhan.WSChannel(s);
                lock (add_chs)
                {
                    add_chs.Add(ch);
                }
                s.ConnectAsync();

                cb(true, ch);
            }
            catch (System.Exception ex)
            {
                Log.Log.err("ex:{0}", ex);
                cb(false, null);
            }
            Debug.Log($"connect {wss} end!");
        }

        public Int64 poll()
        {
            Int64 tick_begin = timer.poll();

            
            while (true)
            {
                if (!Abelkhan.EventQueue.msgQue.TryDequeue(out Tuple<Abelkhan.Ichannel, ArrayList> _event))
                {
                    break;
                }
                Abelkhan.ModuleMgrHandle._modulemng.process_event(_event.Item1, _event.Item2);
            }

            lock (remove_chs)
            {
                foreach (var ch in remove_chs)
                {
                    add_chs.Remove(ch);
                }
                remove_chs.Clear();
            }

            Abelkhan.TinyTimer.poll();
			
            Int64 tick_end = timer.refresh();

            return tick_end - tick_begin;
        }

    }
}

