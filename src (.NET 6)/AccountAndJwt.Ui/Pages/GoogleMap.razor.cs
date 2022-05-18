using AccountAndJwt.Ui.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace AccountAndJwt.Ui.Pages
{
    [Route("googleMap")]
    public partial class GoogleMap
    {
        private Int32 _zoom = 3;
        private Boolean _showMadridMarker;
        private EventConsole _console;



        // FUNCTIONS //////////////////////////////////////////////////////////////////////////////
        void OnMapClick(GoogleMapClickEventArgs args)
        {
            _console.Log($"Map clicked at Lat: {args.Position.Lat}, Lng: {args.Position.Lng}");
        }
        void OnMarkerClick(RadzenGoogleMapMarker marker)
        {
            _console.Log($"Map {marker.Title} marker clicked. Marker position -> Lat: {marker.Position.Lat}, Lng: {marker.Position.Lng}");
        }
    }
}