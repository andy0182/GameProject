using RemotingServerClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameClient
{
    public class ClientObject1 : Player
    {
        public override Entity Create(int ID)
        {
            throw new NotImplementedException();
        }

        public override void SetPosition(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public override void SetRotate(float x, float y, float z, float w)
        {
            throw new NotImplementedException();
        }

        public override void SetTarget(float x, float y, float z)
        {
            throw new NotImplementedException();
        }
    }
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServerObject.InitConnect();
            ServerClient tmp =ServerObject.GetObject<ServerClient>("tcp://127.0.0.1:9000/RemotingObject");
            var maiinterface=tmp.Loging("Andy by 2016", "123456");
            maiinterface.Register(new ClientObject1());
        }
    }
}
