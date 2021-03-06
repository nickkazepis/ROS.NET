﻿// File: XmlRpcSource.cs
// Project: XmlRpc_Wrapper
// 
// ROS.NET
// Eric McCann <emccann@cs.uml.edu>
// UMass Lowell Robotics Laboratory
// 
// Reimplementation of the ROS (ros.org) ros_cpp client in C#.
// 
// Created: 11/06/2013
// Updated: 07/23/2014

#region USINGZ

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

#endregion

namespace XmlRpc_Wrapper
{
    [DebuggerStepThrough]
    public abstract class XmlRpcSource : IDisposable
    {
        protected IntPtr __instance;

        public IntPtr instance
        {
            [DebuggerStepThrough] get { return __instance; }
            [DebuggerStepThrough]
            set
            {
                if (__instance != IntPtr.Zero)
                    RmRef(ref __instance);
                if (value != IntPtr.Zero)
                    AddRef(value);
                __instance = value;
            }
        }

        public int FD
        {
            get { return getfd(instance); }
            set { setfd(instance, value); }
        }

        public bool KeepOpen
        {
            get { return getkeepopen(instance) != 0; }
            set { setkeepopen(instance, value); }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Close();
        }

        #endregion

        #region P/Invoke

        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_Close", CallingConvention = CallingConvention.Cdecl)]
        private static extern void close(IntPtr target);

        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_GetFd", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getfd(IntPtr target);

        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_SetFd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setfd(IntPtr target, int fd);

        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_GetKeepOpen", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte getkeepopen(IntPtr target);

        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_SetKeepOpen", CallingConvention = CallingConvention.Cdecl)]
        private static extern void setkeepopen(IntPtr target, [MarshalAs(UnmanagedType.I1)] bool keepopen);

        [DllImport("XmlRpcWin32.dll", EntryPoint = "XmlRpcSource_HandleEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt16 handleevent(IntPtr target, UInt16 eventType);

        #endregion

        public virtual void RmRef(ref IntPtr i)
        {
        }

        public virtual void AddRef(IntPtr i)
        {
        }

        public void SegFault()
        {
            if (__instance == IntPtr.Zero)
            {
                throw new Exception("BOOM");
            }
        }

        internal virtual void Close()
        {
            close(instance);
        }

        internal virtual UInt16 HandleEvent(UInt16 eventType)
        {
            return handleevent(instance, eventType);
        }
    }
}