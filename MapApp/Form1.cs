using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapApp
{
    public partial class Form1 : Form
    {
        private PointLatLng StartingLocation = new PointLatLng(61.259445, 73.393182);
        private string ConnectionString = Properties.Settings.Default.ObjectCoordinatesConnectionString.ToString();
        private MarkerManager MarkerManager;
        private DataProvider Provider = new DataProvider();
        public Form1()
        {
            InitializeComponent();
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl1.ShowCenter = false;
            gMapControl1.Position = this.StartingLocation;
            try
            {
                List<Coordinates> coordinates = this.Provider.ReadCoordinatesFromDatabase(this.ConnectionString);
                this.MarkerManager = new MarkerManager(gMapControl1, coordinates);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Provider.SaveCoordinatesToDatabase(this.ConnectionString, this.MarkerManager.CoordinatesList);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        private void gMapControl1_OnMarkerEnter(GMapMarker marker)
        {
            this.MarkerManager.MarkerInFocus = marker;
            this.MarkerManager.FindMarkerNumber(marker.Position.Lat, marker.Position.Lng);
            this.gMapControl1.MouseDown += GMapControl1_MouseDown;
        }

        private void GMapControl1_MouseDown(object sender, MouseEventArgs e)
        {
            this.MarkerManager.MarkerInFocus.IsVisible = false;
            if (e.Button == MouseButtons.Left)
            {
                this.MarkerManager.MarkerInFocus.Dispose();
                this.MarkerManager.MarkerIsMoving = true;
            }
        }

        private void gMapControl1_OnMarkerLeave(GMapMarker marker)
        {
            this.MarkerManager.MarkerInFocus = null;
            this.gMapControl1.MouseDown -= GMapControl1_MouseDown;
        }

        private void gMapControl1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.MarkerManager.MarkerIsMoving)
                return;
            this.MarkerManager.MarkerInFocus = this.MarkerManager.CreateMarker(gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat, gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng);
            this.MarkerManager.MarkerIsMoving = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do You Want To Save Your Data", "MapApp", MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    this.Provider.SaveCoordinatesToDatabase(this.ConnectionString, this.MarkerManager.CoordinatesList);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (dialogResult == DialogResult.Cancel) e.Cancel = true;
        }
    }
}
