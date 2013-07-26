﻿// File: Param.cs
// Project: ROS_C-Sharp
// 
// ROS#
// Eric McCann <emccann@cs.uml.edu>
// UMass Lowell Robotics Laboratory
// 
// Reimplementation of the ROS (ros.org) ros_cpp client in C#.
// 
// Created: 03/04/2013
// Updated: 07/26/2013

#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using XmlRpc_Wrapper;

#endregion

namespace Ros_CSharp
{
    public static class Param
    {
        public static Dictionary<string, XmlRpcValue> parms = new Dictionary<string, XmlRpcValue>();
        public static object parms_mutex = new object();
        public static List<string> subscribed_params = new List<string>();

        /// <summary>
        ///     Sets the paramater on the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <param name="val">Value of the paramter</param>
        public static void set(string key, XmlRpcValue val)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            parm.Set(2, val);
            lock (parms_mutex)
            {
                if (master.execute("setParam", parm, ref response, ref payload, true))
                {
                    if (subscribed_params.Contains(mapped_key))
                        parms.Add(mapped_key, val);
                }
            }
        }

        /// <summary>
        ///     Sets the paramater on the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <param name="val">Value of the paramter</param>
        public static void set(string key, string val)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            parm.Set(2, val);
            lock (parms_mutex)
            {
                if (master.execute("setParam", parm, ref response, ref payload, true))
                {
                    if (subscribed_params.Contains(mapped_key))
                        parms.Add(mapped_key, parm);
                }
            }
        }

        /// <summary>
        ///     Sets the paramater on the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <param name="val">Value of the paramter</param>
        public static void set(string key, double val)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            parm.Set(2, val);
            lock (parms_mutex)
            {
                if (master.execute("setParam", parm, ref response, ref payload, true))
                {
                    if (subscribed_params.Contains(mapped_key))
                        parms.Add(mapped_key, parm);
                }
            }
        }

        /// <summary>
        ///     Sets the paramater on the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <param name="val">Value of the paramter</param>
        public static void set(string key, int val)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            parm.Set(2, val);
            lock (parms_mutex)
            {
                if (master.execute("setParam", parm, ref response, ref payload, true))
                {
                    if (subscribed_params.Contains(mapped_key))
                        parms.Add(mapped_key, parm);
                }
            }
        }

        /// <summary>
        ///     Sets the paramater on the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <param name="val">Value of the paramter</param>
        public static void set(string key, bool val)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            parm.Set(2, val);
            lock (parms_mutex)
            {
                if (master.execute("setParam", parm, ref response, ref payload, true))
                {
                    if (subscribed_params.Contains(mapped_key))
                        parms.Add(mapped_key, parm);
                }
            }
        }

        /// <summary>
        ///     Gets the parameter from the parameter server
        /// </summary>
        /// <param name="key">Name of the parameter</param>
        /// <returns></returns>
        public static XmlRpcValue getParam(String key)
        {
            string mapped_key = names.resolve(key);
            XmlRpcValue parm = new XmlRpcValue(), response = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            lock (parms_mutex)
            {
                if (! master.execute("getParam", parm, ref response, ref payload, false))
                {
                    string s = response[1].GetString();

                    throw new Exception(s);
                }
            }

            return payload;
        }

        /// <summary>
        ///     Checks if the paramter exists.
        /// </summary>
        /// <param name="key">Name of the paramerer</param>
        /// <returns></returns>
        public static bool had(string key)
        {
            XmlRpcValue parm = new XmlRpcValue(), result = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, names.resolve(key));
            if (!master.execute("hasParam", parm, ref result, ref payload, false))
                return false;
            return payload.Get<bool>();
        }

        /// <summary>
        ///     Deletes a parameter from the parameter server.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool del(string key)
        {
            string mapped_key = names.resolve(key);
            lock (parms_mutex)
            {
                if (subscribed_params.Contains(key))
                {
                    subscribed_params.Remove(key);
                    if (parms.ContainsKey(key))
                        parms.Remove(key);
                }
            }

            XmlRpcValue parm = new XmlRpcValue(), result = new XmlRpcValue(), payload = new XmlRpcValue();
            parm.Set(0, this_node.Name);
            parm.Set(1, mapped_key);
            if (!master.execute("deleteParam", parm, ref result, ref payload, false))
                return false;
            return true;
        }

        internal static void init(IDictionary remapping_args)
        {
            foreach (object o in remapping_args.Keys)
            {
                string name = (string) o;
                string param = (string) remapping_args[o];
                if (name.Length < 2) continue;
                if (name[0] == '_' && name[1] != '_')
                {
                    string local_name = "~" + name.Substring(1);
                    int i = 0;
                    bool success = int.TryParse(param, out i);
                    if (success)
                    {
                        set(names.resolve(local_name), i);
                        continue;
                    }
                    double d = 0;
                    success = double.TryParse(param, out d);
                    if (success)
                    {
                        set(names.resolve(local_name), d);
                        continue;
                    }
                    bool b = false;
                    success = bool.TryParse(param.ToLower(), out b);
                    if (success)
                    {
                        set(names.resolve(local_name), b);
                        continue;
                    }
                    set(names.resolve(local_name), param);
                }
            }
            XmlRpcManager.Instance.bind("paramUpdate", paramUpdateCallback);
        }

        /// <summary>
        ///     Manually update the value of a parameter
        /// </summary>
        /// <param name="key">Name of parameter</param>
        /// <param name="v">Value to update param to</param>
        public static void update(string key, XmlRpcValue v)
        {
            string clean_key = names.clean(key);
            lock (parms_mutex)
            {
                if (!parms.ContainsKey(clean_key))
                    parms.Add(clean_key, v);
                else
                    parms[clean_key] = v;
            }
        }

        /// <summary>
        ///     Fired when a parameter gets updated
        /// </summary>
        /// <param name="parm">Name of parameter</param>
        /// <param name="result">New value of parameter</param>
        public static void paramUpdateCallback(IntPtr parm, IntPtr result)
        {
            XmlRpcValue val = XmlRpcValue.LookUp(parm);
            val.Set(0, 1);
            val.Set(1, "");
            val.Set(2, 0);
            update(XmlRpcValue.LookUp(parm)[1].Get<string>(), XmlRpcValue.LookUp(parm)[2]);
        }

        public static bool getImpl(string key, ref XmlRpcValue v, bool use_cache)
        {
            string mapped_key = names.resolve(key);

            if (use_cache)
            {
                lock (parms_mutex)
                {
                    if (subscribed_params.Contains(mapped_key))
                    {
                        if (parms.ContainsKey(mapped_key))
                        {
                            if (parms[mapped_key].Valid)
                            {
                                v = parms[mapped_key];
                                return true;
                            }
                            return false;
                        }
                    }
                    else
                    {
                        subscribed_params.Add(mapped_key);
                        XmlRpcValue parm = new XmlRpcValue(), result = new XmlRpcValue(), payload = new XmlRpcValue();
                        parm.Set(0, this_node.Name);
                        parm.Set(1, XmlRpcManager.Instance.uri);
                        parm.Set(2, mapped_key);
                        if (!master.execute("subscribeParam", parm, ref result, ref payload, false))
                        {
                            subscribed_params.Remove(mapped_key);
                            use_cache = false;
                        }
                    }
                }
            }

            XmlRpcValue parm2 = new XmlRpcValue(), result2 = new XmlRpcValue();
            parm2.Set(0, this_node.Name);
            parm2.Set(1, mapped_key);

            bool ret = master.execute("getParam", parm2, ref result2, ref v, false);

            if (use_cache)
            {
                lock (parms_mutex)
                {
                    parms.Add(mapped_key, v);
                }
            }

            return ret;
        }
    }
}