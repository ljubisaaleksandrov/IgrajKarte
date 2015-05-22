function clearMarkers(markers) {
    if (markers) {
        if (markers.length > 0) {
            for (var i = 0; i < markers.length; i++) {
                if (markers[i]) {
                    markers[i].setMap(null);
                }
            }
        }
    }
}

function addMarker(rowId, lat, lng) {
    if (map != null) {
        var marker;

        if (rowId == 0) {
            clearMarkers();
            map.setCenter(new google.maps.LatLng(lat, lng));
        }

        marker = new google.maps.Marker({
            position: new google.maps.LatLng(lat, lng),
            animation: google.maps.Animation.DROP
        });

        return marker;
    }
    
    return null;
}