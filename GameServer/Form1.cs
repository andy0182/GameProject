using DeenGames.Utils.AStarPathFinder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServerObject.StartListening(9000);
            ServerObject.AddObject<World>("RemotingObject", WellKnownObjectMode.Singleton);
        }
    }
}
