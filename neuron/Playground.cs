using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using BlueBlocksLib.FileAccess;

namespace neuron {
	public partial class Playground<T> : Panel {


		public Playground() {
			InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs pe) {
			pe.Graphics.FillRectangle(Brushes.Black, pe.ClipRectangle);
			pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			foreach (var o in objects) {
				List<ILinkable> toremove = new List<ILinkable>();

				foreach (var link in o.Edges) {
					if (objects.Contains(link)) {
						Pen p = new Pen(Brushes.White);
						p.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
						pe.Graphics.DrawLine(p, o.Center, link.GetArrowArrivalFromSourceCenter(o.Center));
					} else {
						toremove.Add(link);
					}
				}

				// kill dead links
				foreach (ILinkable dead in toremove) {
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

		public interface ILinkable {
			HashSet<ILinkable> Edges { get; }
			Point GetArrowArrivalFromSourceCenter(Point center);
			Point Center { get; }
			Point TopLeft { get; }
			void AskToNotify(Action<ILinkable> action);
			void CancelAskToNotify();
			Control MainControl { get; }
			string Name { get; }
		}

		public class Box : ILinkable {
			Dragger dragger;
			Panel panel;
			Button btn;
			ContextMenu cm = new ContextMenu();
			string name;

			public T data;

			internal Box(string name, Panel panel, Button btn, Control dragelem) {
				dragger = new Dragger(dragelem, panel);
				this.panel = panel;
				this.panel.LocationChanged += new EventHandler((o, e) => { panel.Parent.Refresh(); });


				btn.Click += new EventHandler((o, e) => { cm.Show(btn, new Point()); });

				dragelem.Click += new EventHandler(dragelem_Click);

				panel.MouseHover += new EventHandler(panel_MouseHover);
				panel.MouseLeave += new EventHandler(panel_MouseLeave);
				dragelem.MouseHover += new EventHandler(panel_MouseHover);
				dragelem.MouseLeave += new EventHandler(panel_MouseLeave);
				origColor = panel.BackColor;

				this.name = name;
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

			public Point GetArrowArrivalFromSourceCenter(Point center) {
				int x = center.X, y = center.Y;

				if (center.X < panel.Location.X) {
					x = panel.Location.X;
				}
				if (center.Y < panel.Location.Y) {
					y = panel.Location.Y;
				}
				if (center.X > panel.Location.X + panel.Width) {
					x = panel.Location.X + panel.Width;
				}
				if (center.Y > panel.Location.Y + panel.Height) {
					y = panel.Location.Y + panel.Height;
				}

				return new Point(x, y);
			}

			public void LinkTo(ILinkable link) {
				if (link != this) {
					links.Add(link);
				}
			}

			public HashSet<ILinkable> Edges {
				get {
					return links;
				}
			}

			internal HashSet<ILinkable> links = new HashSet<ILinkable>();

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

			public string Name {
				get {
					return name;
				}
			}
		}

		HashSet<ILinkable> objects = new HashSet<ILinkable>();

		bool selecting = false;
		public void SelectNode(Action<ILinkable> onSelectDone) {
			if (!selecting && objects.Count != 0) {
				selecting = true;
				Label l = new Label();
				l.TextAlign = ContentAlignment.MiddleLeft;
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
			Refresh();
		}

		public Box AddBox(string name, Color color, T data) {
			if (selecting) {
				return null;
			}

			Label lbl = new Label();
			lbl.Text = name;
			lbl.TextAlign = ContentAlignment.MiddleCenter;
			lbl.AutoSize = true;
			Padding pad = new System.Windows.Forms.Padding(5);
			lbl.Padding = pad;

			Button btn = new Button();
			btn.Text = ".";
			btn.Size = new Size(20, 20);


			FlowLayoutPanel p = new FlowLayoutPanel();
			p.FlowDirection = FlowDirection.LeftToRight;
			p.AutoSize = true;
			p.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			p.BackColor = color;
			p.Location = new Point(50, 50);

			p.Controls.Add(lbl);
			p.Controls.Add(btn);

			Box b = new Box(name, p, btn, lbl);
			b.data = data;
			btn.Tag = b;	// fill the button with this box
			Controls.Add(p);
			objects.Add(b);

			return b;
		}

		public void Save(string filename) {

			Dictionary<ILinkable, int> linkable = new Dictionary<ILinkable, int>();
			foreach (var o in objects) {
				linkable[o] = linkable.Count;
			}

			GraphFileFormat gff = new GraphFileFormat();
			gff.numVertices = linkable.Count;
			gff.vertices = linkable.Keys.Select(x => new VertexFormat() {
				edges = x.Edges.Select(y => linkable[y]).ToArray(),
				x = x.TopLeft.X,
				y = x.TopLeft.Y,
				numEdges = x.Edges.Count,
				name = x.Name,
				color = x.MainControl.BackColor.ToArgb()
			}).ToArray();


			using (FormattedWriter fw = new FormattedWriter(filename)) {
				fw.Write(gff);
			}
		}

		public void Load(string filename) {
			using (FormattedReader fr = new FormattedReader(filename)) {
				var gff = fr.Read<GraphFileFormat>();
				Box[] boxes = gff.vertices.Select(x => { 
					var box = AddBox(x.name, Color.FromArgb(x.color), x.data);
					box.MainControl.Location = new Point(x.x, x.y);
					return box;
				}).ToArray();

				for (int i = 0; i < boxes.Length; i++) {
					foreach (int edge in gff.vertices[i].edges) {
						boxes[i].LinkTo(boxes[edge]);
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

			public int color;
			public string name;
			public T data;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct GraphFileFormat {
			public int numVertices;

			[ArraySize("numVertices")]
			public VertexFormat[] vertices;
		}
	}
}
