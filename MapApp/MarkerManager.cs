using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapApp
{
    public class MarkerManager
    {
        public List<Coordinates> CoordinatesList;
        public GMapOverlay Overlay;
        public GMapMarker MarkerInFocus;
        public bool MarkerIsMoving;
        private int MovingMarkerNumber;
        public MarkerManager(GMapControl gMap, List<Coordinates> coordinates)
        {
            foreach (Coordinates item in coordinates)
            {
                this.Overlay = new GMapOverlay("markers");
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(item.Latitude, item.Longitude), GMarkerGoogleType.blue_pushpin);
                Overlay.Markers.Add(marker);
                gMap.Overlays.Add(this.Overlay);
            }
            this.CoordinatesList = coordinates;
        }
        public GMapMarker CreateMarker(double latitude, double longitude)
        {
            GMapMarker marker = new GMarkerGoogle(new PointLatLng(latitude, longitude), GMarkerGoogleType.blue_pushpin);
            Overlay.Markers.Add(marker);
            this.CoordinatesList[this.MovingMarkerNumber] = new Coordinates {Latitude = latitude, Longitude = longitude };
            return marker;
        }
        public void FindMarkerNumber(double latitude, double longitude)
        {
            this.MovingMarkerNumber = this.CoordinatesList.FindIndex(x => (x.Latitude == latitude & x.Longitude == longitude));
        }
    }
}
