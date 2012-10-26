using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BlueBlocksLib.FileAccess;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Drawing;
using BlueBlocksLib.Collections;
using BlueBlocksLib.SetUtils;
using BlueBlocksLib.BaseClasses;

namespace BlueBlocksLib.Controls
{

    public class GraphMap<T, TLink> : Control
    {
        public GraphMap()
        {
			InitializeComponent();
		}

        const int Margin = 2;
        const int ArrowSize = 10;


        static int Sign(double num)
        {
            if (num > 0) { return 1; };
            if (num < 0) { return -1; };
            return 0;
        }

        static Point CenterOfRect(Rectangle aRect)
        {
            return new Point(aRect.X + aRect.Width / 2, aRect.Y + aRect.Height / 2);
        }

        static Point PointOnRectFromCenter(Rectangle aRect, double dy, double dx)
        {
            var aCenter = CenterOfRect(aRect);
            var ratio = (double)aRect.Height / (double)aRect.Width;

            var w2 = aRect.Width / 2;
            var h2 = aRect.Height / 2;

            var m1 = dy / dx;
            var m2 = dx / dy;

            if (Math.Abs(ratio) > Math.Abs(m1) || dy == 0)
            {
                // going out the side
                var y = aCenter.Y + Sign(dx) * (w2 + Margin) * m1;
                var x = aCenter.X + Sign(dx) * (w2 + Margin);
                return new Point((int)x, (int)y);
            }
            else if (dy != 0)
            {
                // going out the top/bottom
                var y = aCenter.Y + Sign(dy) * (h2 + Margin);
                var x = aCenter.X + Sign(dy) * (h2 + Margin) * m2;
                return new Point((int)x, (int)y);
            }
            return aCenter;
        }

        static Pair<Point, Point> ClosestLineBetweenRects(Rectangle aRect, Rectangle bRect)
        {
            var aCenter = CenterOfRect(aRect);
            var bCenter = CenterOfRect(bRect);

            var dy = bCenter.Y - aCenter.Y;
            var dx = bCenter.X - aCenter.X;

            var p1 = PointOnRectFromCenter(aRect, dy, dx);
            var p2 = PointOnRectFromCenter(bRect, -dy, -dx);

            // make the arrow head coords
            var angle = Math.Atan2(dx, dy);

            var ap1x = p2.X - ArrowSize * Math.Sin(angle + Math.PI / 6);
            var ap1y = p2.Y - ArrowSize * Math.Cos(angle + Math.PI / 6);

            var ap2x = p2.X - ArrowSize * Math.Sin(angle - Math.PI / 6);
            var ap2y = p2.Y - ArrowSize * Math.Cos(angle - Math.PI / 6);

            return new Pair<Point, Point>() { a = p1, b = p2 };
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.FillRectangle(Brushes.Black, pe.ClipRectangle);
			pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (var o in objects) {
				List<ILinkable> toremove = new List<ILinkable>();

                foreach (var link in o.Edges)
                {
                    if (objects.Contains(link.Key))
                    {
						Pen p = new Pen(Brushes.White, 3);
						p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                        var points = ClosestLineBetweenRects(new Rectangle(o.MainControl.Location, o.MainControl.Size), new Rectangle(link.Key.MainControl.Location, link.Key.MainControl.Size));
                        pe.Graphics.DrawLine(p, points.a, points.b);
                        link.Value.UpdateLocation();
					} else {
                        toremove.Add(link.Key);
					}
				}

				// kill dead links
				foreach (ILinkable dead in toremove) {
                    this.Controls.Remove(o.Edges[dead].LinkControl);
                    o.Edges.Remove(dead);
				}
			}
		}

		protected override void OnPaintBackground(PaintEventArgs pevent) {
		}

		public class Dragger {

			Control toBeDragged;

			internal Dragger(Control c, Control toBeDragged) {
				c.MouseDown += new MouseEventHandler(c_MouseDown);
				c.MouseUp += new MouseEventHandler(c_MouseUp);
				c.MouseMove += new MouseEventHandler(c_MouseMove);
				this.toBeDragged = toBeDragged;
			}
			int prevx, prevy;
			bool dragging = false;

			void c_MouseMove(object sender, MouseEventArgs e) {
				if (dragging) {
					int dx = e.X - prevx;
					int dy = e.Y - prevy;
					var s = toBeDragged;
					s.Location = new Point(s.Location.X + dx, s.Location.Y + dy);

                    if (s.Location.X < 0)
                    {
                        s.Location = new Point(0, s.Location.Y);
                    }

                    if (s.Location.Y < 0)
                    {
                        s.Location = new Point(s.Location.X, 0);
                    }

                    if (s.Location.X + s.Size.Width > s.Parent.Width)
                    {
                        s.Location = new Point(s.Parent.Width - s.Size.Width, s.Location.Y);
                    }

                    if (s.Location.Y + s.Size.Height > s.Parent.Height)
                    {
                        s.Location = new Point(s.Location.X, s.Parent.Height - s.Size.Height);
                    }
				}
			}

			void c_MouseUp(object sender, MouseEventArgs e) {
				if (dragging) {
					dragging = false;
				}
			}

			void c_MouseDown(object sender, MouseEventArgs e) {
				dragging = true;
				prevx = e.X;
				prevy = e.Y;
			}
		}

        public class Link<TLink>
        {
            ContextMenu cm = new ContextMenu();

            public readonly Panel LinkControl;

            public readonly ILinkable From;
            public readonly ILinkable To;
            public TLink Data
            {
                get
                {
                    return linkdata;
                }
            }
        

            TLink linkdata;
            FlowLayoutPanel p;
            Label lbl;

            public Link(ILinkable From, ILinkable To, TLink linkdata)
            {
                lbl = new Label();
                lbl.Text = "Link";
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                lbl.AutoSize = true;
                Padding pad = new System.Windows.Forms.Padding(5);
                lbl.Padding = pad;

                Button btn = new Button();
                btn.Text = ".";
                btn.Size = new Size(20, 20);


                p = new FlowLayoutPanel();
                p.FlowDirection = FlowDirection.LeftToRight;
                p.AutoSize = true;
                p.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
                p.BackColor = Color.White;
                p.Location = new Point(50, 50);

                p.Controls.Add(lbl);
                p.Controls.Add(btn);

                LinkControl = p;
                this.From = From;
                this.To = To;
                this.linkdata = linkdata;

                btn.Click += new EventHandler((o, e) => { cm.Show(btn, new Point()); });

                UpdateLocation();
            }

            public void UpdateLocation()
            {
                var points = ClosestLineBetweenRects(new Rectangle(From.MainControl.Location, From.MainControl.Size), new Rectangle(To.MainControl.Location, To.MainControl.Size));
                LinkControl.Location = new Point((points.b.X + points.a.X - LinkControl.Width) / 2, (points.b.Y + points.a.Y - LinkControl.Height) / 2);
            }

            public string Name
            {
                get
                {
                    return lbl.Text;
                }
                set
                {
                    lbl.Text = value;
                }
            }

            public void AddAction(string actionname, Action<TLink> action)
            {
                var item = new MenuItem(actionname);
                item.Tag = action;
                item.Click += new EventHandler((o, e) =>
                {
                    var self = (MenuItem)o;
                    var act = (Action<TLink>)self.Tag;
                    act(linkdata);
                });
                cm.MenuItems.Add(item);
            }
        }

		public interface ILinkable {
			Dictionary<ILinkable, Link<TLink>> Edges { get; }
			Point Center { get; }
			Point TopLeft { get; }
			void AskToNotify(Action<ILinkable> action);
			void CancelAskToNotify();
			Control MainControl { get; }
			string Name { get; }
            T Data { get; }
            Control Secondary { get; }
            Link<TLink> LinkTo(ILinkable other, TLink data);
		}

		public class Box : ILinkable {
			Dragger dragger;
			Panel panel;
			Button btn;
			ContextMenu cm = new ContextMenu();
            Label label;

            public Control Secondary { get; set; }

            public T Data { get; set; }

			internal Box(Panel panel, Button btn, Label label) {
				dragger = new Dragger(label, panel);
				this.panel = panel;
				this.panel.LocationChanged += new EventHandler((o, e) => { panel.Parent.Refresh(); });
                this.label = label;

				btn.Click += new EventHandler((o, e) => { cm.Show(btn, new Point()); });

				label.Click += new EventHandler(dragelem_Click);

				panel.MouseLeave += new EventHandler(panel_MouseLeave);
				label.MouseLeave += new EventHandler(panel_MouseLeave);

                panel.MouseEnter += new EventHandler(panel_MouseHover);
                label.MouseEnter += new EventHandler(panel_MouseHover);

                panel.LocationChanged += new EventHandler(panel_LocationChanged);

				origColor = panel.BackColor;

			}

            void panel_LocationChanged(object sender, EventArgs e)
            {
            }

			public void AddAction(string actionname, Action<Box> action) {
				var item = new MenuItem(actionname);
				item.Tag = action;
				item.Click += new EventHandler((o, e) => {
					var self = (MenuItem)o;
					var act = (Action<Box>)self.Tag;
					act(this);
				});
				cm.MenuItems.Add(item);
			}

			public void RemoveAction(string actionname) {
				List<MenuItem> mis = new List<MenuItem>();
				foreach (MenuItem item in cm.MenuItems) {
					if (item.Text == actionname) {
						mis.Add(item);
					}
				}
				foreach (var mi in mis) {
					cm.MenuItems.Remove(mi);
				}
			}

			public Control MainControl {
				get {
					return panel;
				}
			}

			readonly Color origColor;
			void panel_MouseLeave(object sender, EventArgs e) {
				panel.BackColor = origColor;
			}

			void panel_MouseHover(object sender, EventArgs e) {
				if (requestToNotify != null) {
					panel.BackColor = Color.FromArgb(origColor.ToArgb() | 0x007f7f7f);
					panel.Refresh();
                    panel.Parent.Refresh();
				}
			}

			Action<ILinkable> requestToNotify;

			void dragelem_Click(object sender, EventArgs e) {
				if (requestToNotify != null) {
					requestToNotify(this);
				}
			}

			public Point Center {
				get {
					return new Point(panel.Location.X + panel.Width / 2, panel.Location.Y + panel.Height / 2);
				}
			}

			public Link<TLink> LinkTo(ILinkable link, TLink linkdata) {
				if (link != this) {
                    var newlink = new Link<TLink>(this, link, linkdata);
                    links.Add(link, newlink);
                    this.panel.Parent.Controls.Add(newlink.LinkControl);
                    return newlink;
				}
                return null;
			}

			public Dictionary<ILinkable, Link<TLink>> Edges {
				get {
					return links;
				}
			}

            internal Dictionary<ILinkable, Link<TLink>> links = new Dictionary<ILinkable, Link<TLink>>();

			public void AskToNotify(Action<ILinkable> action) {
				requestToNotify = action;
			}

			public void CancelAskToNotify() {
				requestToNotify = null;
			}

			public Point TopLeft {
				get {
					return panel.Location;
				}
				set {
					panel.Location = value;
				}
			}

            internal Func<string, T> getDisplayName;

			public string Name {
				get {
                    return label.Text;
                }
                set
                {
                    label.Text = getDisplayName(Data);
                }
			}
		}

        Set<ILinkable> objects = new Set<ILinkable>();

		public IEnumerable<ILinkable> Vertices {
			get {
				return objects;
			}
		}


		bool selecting = false;
		public void SelectNode(Action<ILinkable> onSelectDone) {
			if (!selecting && objects.Count != 0) {
				selecting = true;
				Label l = new Label();
				l.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
				l.Text = "Select a node...";
				l.BackColor = Color.Green;
				l.ForeColor = Color.White;
				l.Dock = DockStyle.Top;
				Controls.Add(l);

				foreach (var o in objects) {
					o.AskToNotify(x => {
						onSelectDone(x);
						foreach (var obj in objects) {
                            obj.CancelAskToNotify();
						}
						Controls.Remove(l);
						selecting = false;
						Refresh();
					});
				}
			}
		}

		public void Delete(ILinkable linkable) {
			objects.Remove(linkable);
            Controls.Remove(linkable.MainControl);

            foreach (var edge in linkable.Edges.Values)
            {
                this.Controls.Remove(edge.LinkControl);
            }
            Refresh();
		}

		public event Action<ILinkable, ILinkable> linkRemoved;

        public Box AddBox(Color color, T data)
        {
            return AddBox(color, data, x => x.ToString());
        }

		public Box AddBox(Color color, T data, Func<string, T> getDisplayName, Control secondary = null) {
			if (selecting) {
				return null;
			}

			Label lbl = new Label();
            lbl.Text = getDisplayName(data);
			lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			lbl.AutoSize = true;
			Padding pad = new System.Windows.Forms.Padding(5);
			lbl.Padding = pad;

			Button btn = new Button();
			btn.Text = ".";
			btn.Size = new Size(20, 20);

            FlowLayoutPanel topp = new FlowLayoutPanel();
            topp.FlowDirection = FlowDirection.TopDown;
            topp.AutoSize = true;
            topp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            topp.BackColor = color;


			FlowLayoutPanel p = new FlowLayoutPanel();
			p.FlowDirection = FlowDirection.LeftToRight;
			p.AutoSize = true;
			p.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			p.Location = new Point(50, 50);

			p.Controls.Add(lbl);
			p.Controls.Add(btn);

            topp.Controls.Add(p);
            if (secondary != null)
            {
                topp.Controls.Add(secondary);
                lbl.SizeChanged += new EventHandler((e, o) => { 
                    
                });
            }

            Box b = new Box(topp, btn, lbl);
			b.Data = data;
            b.Secondary = secondary;
			btn.Tag = b;	// fill the button with this box
            Controls.Add(topp);
			objects.Add(b);
            b.getDisplayName = getDisplayName;

			return b;
		}

		public void Save(string filename) {

			Dictionary<ILinkable, int> linkable = new Dictionary<ILinkable, int>();
			foreach (var o in objects) {
				linkable[o] = linkable.Count;
			}

			GraphFileFormat gff = new GraphFileFormat();
			gff.numVertices = linkable.Count;
            gff.vertices = ArrayUtils.ConvertAll(ArrayUtils.ToArray(linkable.Keys), x => new VertexFormat()
            {
                edges = ArrayUtils.ConvertAll(ArrayUtils.ToArray(x.Edges), y => linkable[y.Key]),
                x = x.TopLeft.X,
                y = x.TopLeft.Y,
                numEdges = x.Edges.Count,
                color = x.MainControl.BackColor.ToArgb()
            });


			using (FormattedWriter fw = new FormattedWriter(filename)) {
				fw.Write(gff);
			}
		}

		public void Load(string filename) {
			using (FormattedReader fr = new FormattedReader(filename)) {
				var gff = fr.Read<GraphFileFormat>();
                Box[] boxes = ArrayUtils.ConvertAll(gff.vertices, x =>
                {
                    var box = AddBox( Color.FromArgb(x.color), x.data);
                    box.MainControl.Location = new Point(x.x, x.y);
                    return box;
                });

				for (int i = 0; i < boxes.Length; i++) {
					foreach (int edge in gff.vertices[i].edges) {
                        boxes[i].LinkTo(boxes[edge], gff.vertices[i].linkdatas[edge]);
					}
				}

			}
		}

		[StructLayout(LayoutKind.Sequential)]
		struct VertexFormat {
			public int x, y;
			public int numEdges;

			[ArraySize("numEdges")]
			public int[] edges;

            [ArraySize("numEdges")]
            public TLink[] linkdatas;

			public int color;
			public T data;
		}


		[StructLayout(LayoutKind.Sequential)]
		struct GraphFileFormat {
            public int numVertices;

			[ArraySize("numVertices")]
			public VertexFormat[] vertices;

		}

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
