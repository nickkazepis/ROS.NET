﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Ros_CSharp;
using XmlRpc_Wrapper;
using Messages;
using System.Threading;

namespace SimpleSubscriber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Subscriber<Messages.SimpleSubscriber.SimpleMessage> sub;
        NodeHandle nh;

        public MainWindow()
        {
            InitializeComponent();

            ROS.Init(new string[0], "wpf_listener");
            nh = new NodeHandle();

			sub = nh.subscribe<Messages.SimpleSubscriber.SimpleMessage>("/simple_topic", 10, subCallback);
        }

		public void subCallback(Messages.SimpleSubscriber.SimpleMessage msg)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                l.Content = "Receieved:\n" + msg.msg.data + " <--> " + msg.number;
            }), new TimeSpan(0,0,1));
        }

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			ROS.shutdown();
			ROS.waitForShutdown();
		}
	}//public partial class MainWindow : Window
}//namespace SimpleSubscriber

