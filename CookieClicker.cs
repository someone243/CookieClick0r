using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Media;
using System.Threading;
using PlayerIOClient;


namespace CookieClicker
{
    public class CookieClicker : Form
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox email_TextBox;
        private System.Windows.Forms.Button connect_Button;
        private System.Windows.Forms.TextBox password_TextBox;
        private System.Windows.Forms.TextBox worldID_TextBox;

        public int CookiesDisplay
        {
            get { return (int)Math.Floor((decimal)cookies); }
        }
        System.Threading.Timer cpsTick;

        private void tickCps(object state)
        {

            con.Send("b", 0, 25, 36, 385, $"Cookies: {CookiesDisplay}", 1);
            cookies += cps / 10;
        }
        System.Threading.Timer signTick;

        private void tickSign(object state)
        {
            if (toPlaceQueue.Count > 0)
            {
                Block b = toPlaceQueue.Dequeue();
                con.Send("b", 0, b.x, b.y, b.id, b.extra);
            }
        }

        private double _cps;
        public double cps
        {
            get
            {
                RecalculateCps(); return _cps;
            }
            set { _cps = value; }
        }

        Queue<Block> toPlaceQueue = new Queue<Block>();
        private void RecalculateCps()
        {
            cps = BCursor.cps
                + BGrandma.cps
                + BFarm.cps
                + BMine.cps
                + BFactory.cps
                + BBank.cps
                + BTemple.cps
                + BWizardTower.cps
                + BShipment.cps
                + BAlchemyLab.cps
                + BPortal.cps
                + BTimeMachine.cps
                + BAntimatterCondenser.cps
                + BPrism.cps;
        }
        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public double cookies = 0;

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.email_TextBox = new System.Windows.Forms.TextBox();
            this.connect_Button = new System.Windows.Forms.Button();
            this.worldID_TextBox = new System.Windows.Forms.TextBox();
            this.password_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // email_TextBox
            // 
            this.email_TextBox.Location = new System.Drawing.Point(12, 12);
            this.email_TextBox.Name = "email_TextBox";
            this.email_TextBox.Size = new System.Drawing.Size(179, 20);
            this.email_TextBox.TabIndex = 0;
            // 
            // connect_Button
            // 
            this.connect_Button.Location = new System.Drawing.Point(197, 12);
            this.connect_Button.Name = "connect_Button";
            this.connect_Button.Size = new System.Drawing.Size(75, 72);
            this.connect_Button.TabIndex = 1;
            this.connect_Button.Text = "Connect";
            this.connect_Button.UseVisualStyleBackColor = true;
            this.connect_Button.Click += new System.EventHandler(this.Connect_ButtonClick);
            // 
            // worldID_TextBox
            // 
            this.worldID_TextBox.Location = new System.Drawing.Point(12, 64);
            this.worldID_TextBox.Name = "worldID_TextBox";
            this.worldID_TextBox.Size = new System.Drawing.Size(179, 20);
            this.worldID_TextBox.TabIndex = 2;
            // 
            // password_TextBox
            // 
            this.password_TextBox.Location = new System.Drawing.Point(12, 38);
            this.password_TextBox.Name = "password_TextBox";
            this.password_TextBox.PasswordChar = '*';
            this.password_TextBox.Size = new System.Drawing.Size(179, 20);
            this.password_TextBox.TabIndex = 3;
            // 
            // CookieClicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 100);
            this.Controls.Add(this.password_TextBox);
            this.Controls.Add(this.worldID_TextBox);
            this.Controls.Add(this.connect_Button);
            this.Controls.Add(this.email_TextBox);
            this.Name = "CookieClicker";
            this.Text = "Cookie Clicker!";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        static void Main(string[] args)
        {
            System.Console.WriteLine("Test");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            Application.Run(new CookieClicker());
        }

        public CookieClicker()
        {
            cpsTick = new System.Threading.Timer(tickCps, null, Timeout.Infinite, 100);
            signTick = new System.Threading.Timer(tickSign, null, Timeout.Infinite, 100);
            InitializeComponent();
            Achiev.Init(this);
        }

        public Connection con;
        public Client client;
        public bool isConnected;
        public int worldx;
        public int worldy;
        public Dictionary<int, player> users = new Dictionary<int, player>();
        public int milk = 0;

        public class player
        {
            public string username { get; set; }
            public int userid { get; set; }
            public Type buying { get; set; }
        }

        public void say(string msg)
        {
            con.Send("say", "[Bot]: " + msg);
        }

        public void pm(string user, string msg)
        {
            con.Send("say", "/pm " + user + " [Bot]: " + msg);
        }

        public void com(string msg)
        {
            con.Send("say", "/" + msg);
        }

        public async void Connect_ButtonClick(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                try
                {
                    client = PlayerIO.QuickConnect.SimpleConnect("everybody-edits-su9rn58o40itdbnw69plyw",
                        email_TextBox.Text, password_TextBox.Text, null);
                    // NOTE: Dont forget to change the  buttons into the specified names!
                    con = client.Multiplayer.CreateJoinRoom(worldID_TextBox.Text, "public", true,
                        new Dictionary<string, string>(), new Dictionary<string, string>());
                    con.Send("init");
                    await Task.Delay(500);
                    con.Send("init2");
                    con.OnMessage += handlemsg;
                    isConnected = true;
                    connect_Button.Text = "Disconnect";
                    cpsTick.Change(0, 100);
                    signTick.Change(5000, 20);
                }
                catch (PlayerIOError oops)
                {
                    MessageBox.Show(Convert.ToString(oops), "PlayerIO Error!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                catch (Exception oops2)
                {
                    MessageBox.Show(Convert.ToString(oops2), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                say("Goodbye!");
                await Task.Delay(500);
                isConnected = false;
                connect_Button.Text = "Connect";
                con.Disconnect();
            }
        }

        public void handlemsg(object sender, PlayerIOClient.Message m)
        {
            switch (m.Type)
            {
                case "init":
                    {
                        con.Send("init2");
                    }
                    break;
                case "init2":
                    {
                        say("Sucessfully joined world.");
                        con.Send("god", true);
                        con.Send("aura", 1, 7);
                        con.Send("m", 17 * 16, 1 * 16, 0, 0, 0, 0, 0, 0, 0, false, false, 0);
                    }
                    break;
                case "add":
                    {
                        string player = m.GetString(1);
                        say(player + " joined the world!");
                        users.Add(m.GetInt(0), new player()
                        {
                            userid = m.GetInt(0),
                            username = m.GetString(1)
                        });
                    }
                    break;
                case "left":
                    {
                        if (users.ContainsKey(m.GetInt(0)))
                        {
                            users.Remove(m.GetInt(0));
                        }
                    }
                    break;
                case "say":
                    {
                        string player = users[m.GetInt(0)].username;
                        if (Regex.IsMatch(m.GetString(1).Substring(0, 1), @"\.|\!|\@|\$|\-"))
                        {
                            var cmdprefix = m.GetString(1).Substring(0, 1);
                            var cmds = m.GetString(1).ToLower().Split(' ');
                            //if(cmds[0].StartWith(cmdprefix + ""))
                            //Use cmds[1] for multiple lined commands.

                        }
                    }
                    break;
                case "m":
                    {
                        string player = users[m.GetInt(0)].username;
                        if (m.GetInt(1) == 208 & m.GetInt(2) == 16)
                        {
                            switch (users[m.GetInt(0)].buying.Name)
                            {
                                case "BCursor":
                                    if (cookies > BCursor.cost)
                                    {
                                        cookies -= BCursor.cost;
                                        BCursor.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}")); /*  toPlaceQueue.Enqueue(new Block(27,36,385, $"Cookies per second: {cps}"));*/
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Cursor (cost: {BCursor.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BGrandma":
                                    if (cookies > BGrandma.cost)
                                    {
                                        cookies -= BGrandma.cost;
                                        BGrandma.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Grandma (cost: {BGrandma.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BFarm":
                                    if (cookies > BFarm.cost)
                                    {
                                        cookies -= BFarm.cost;
                                        BFarm.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Farm (cost: {BFarm.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BMine":
                                    if (cookies > BMine.cost)
                                    {
                                        cookies -= BMine.cost;
                                        BMine.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Mine (cost: {BMine.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BFactory":
                                    if (cookies > BFactory.cost)
                                    {
                                        cookies -= BFactory.cost;
                                        BFactory.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Factory (cost: {BFactory.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BBank":
                                    if (cookies > BBank.cost)
                                    {
                                        cookies -= BBank.cost;
                                        BBank.Buy(1);
                                        toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Bank (cost: {BBank.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BTemple":
                                    if (cookies > BTemple.cost)
                                    {
                                        cookies -= BTemple.cost;
                                        BTemple.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Temple (cost: {BTemple.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BWizardTower":
                                    if (cookies > BWizardTower.cost)
                                    {
                                        cookies -= BWizardTower.cost;
                                        BWizardTower.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Wizard Tower (cost: {BWizardTower.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BShipment":
                                    if (cookies > BShipment.cost)
                                    {
                                        cookies -= BShipment.cost;
                                        BShipment.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Shipment (cost: {BShipment.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BAlchemyLab":
                                    if (cookies > BAlchemyLab.cost)
                                    {
                                        cookies -= BAlchemyLab.cost;
                                        BAlchemyLab.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Alchemy Lab (cost: {BAlchemyLab.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BPortal":
                                    if (cookies > BPortal.cost)
                                    {
                                        cookies -= BPortal.cost;
                                        BPortal.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Portal (cost: {BPortal.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BTimeMachine":
                                    if (cookies > BTimeMachine.cost)
                                    {
                                        cookies -= BTimeMachine.cost;

                                        BTimeMachine.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Time Machine (cost: {BTimeMachine.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BAntimatterCondenser":
                                    if (cookies > BAntimatterCondenser.cost)
                                    {
                                        cookies -= BAntimatterCondenser.cost;
                                        BAntimatterCondenser.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Antimatter Condenser (cost: {BAntimatterCondenser.cost.FormatNumber()})"));
                                    }
                                    break;
                                case "BPrism":
                                    if (cookies > BPrism.cost)
                                    {
                                        cookies -= BPrism.cost;
                                        BPrism.Buy(1); toPlaceQueue.Enqueue(new Block(27, 36, 385, $"Cookies per second: {cps}"));
                                        toPlaceQueue.Enqueue(new Block(22, 4, 385, $"Buy Prism (cost: {BPrism.cost.FormatNumber()})"));

                                    }
                                    break;
                                    //case "BCursor":
                                    //    BCursor.Buy(1);
                                    //    break;
                                    //case "BCursor":
                                    //    BCursor.Buy(1);
                                    //    break;
                                    //case "BCursor":
                                    //    BCursor.Buy(1);
                                    //    break;
                            }
                            com($"teleport {player} 25 25");

                        }

                        if (m.GetInt(1) == 240 & m.GetInt(2) == 16)
                        {
                            com($"teleport {player} 25 25");
                        }
                        if (m.GetBoolean(10)) cookies++;
                        if (m.GetInt(7) == 0 & m.GetInt(8) == 0 & m.GetBoolean(10))
                        {
                            if (m.GetInt(1) == 352)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BCursor);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 384)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BGrandma);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 416)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BFarm);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 448)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BMine);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 480)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BFactory);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 512)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BBank);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 544)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BTemple);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 576)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BWizardTower);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 608)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BShipment);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 640)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BAlchemyLab);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 672)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BPortal);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 704)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BTimeMachine);
                                    com($"teleport {player} 14 3");
                                }
                            }
                            if (m.GetInt(1) == 736)
                            {
                                if (m.GetInt(2) == 64)
                                {
                                    users[m.GetInt(0)].buying = typeof(BPrism);
                                    com($"teleport {player} 14 3");
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }

    internal class Block
    {
        public int x, y, id;
        public string extra;
        public Block(int x, int y, int id)
        {
            this.x = x;
            this.y = y; this.id = id;
            this.extra = null;
        }
        public Block(int x, int y, int id, string extra)
        {
            this.x = x;
            this.y = y; this.id = id;
            this.extra = extra;
        }
    }
}