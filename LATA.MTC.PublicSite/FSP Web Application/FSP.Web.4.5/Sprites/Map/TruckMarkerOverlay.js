function TruckMarkerOverlay(id, position, image, map, rotation) {

    // Now initialize all properties.
    this.id_ = id;
    this.position_ = position;
    this.image_ = image;
    this.map_ = map;
    this.rotation_ = rotation;

    // We define a property to hold the image's
    // div. We'll actually create this div
    // upon receipt of the add() method so we'll
    // leave it null for now.
    this.div_ = null;

    // Explicitly call setMap() on this overlay
    this.setMap(map);
}

TruckMarkerOverlay.prototype = new google.maps.OverlayView();

TruckMarkerOverlay.prototype.onAdd = function () {

    // Note: an overlay's receipt of onAdd() indicates that
    // the map's panes are now available for attaching
    // the overlay to the map via the DOM.

    // Create the DIV and set some basic attributes.
    var div = document.createElement('div');
    div.style.border = "none";
    div.style.borderWidth = "0px";
    div.style.position = "absolute";

    // Create an IMG element and attach it to the DIV.
    var img = document.createElement("img");
    img.id = this.id_;
    img.src = this.image_;
    
    this.image_ = img;

    div.appendChild(img);

    // Set the overlay's div_ property to this DIV
    this.div_ = div;

    this.listeners_ = [
       google.maps.event.addListener(this, 'position_changed',
           function () { me.draw(); }),
       google.maps.event.addListener(this, 'text_changed',
           function () { me.draw(); })
    ];

    google.maps.event.addDomListener(div, 'click', function (e) {
        alert('click');
    });
    google.maps.event.addDomListener(div, 'rightclick', function (e) {
        alert('rightclick');
    });

    // We add an overlay to a map via one of the map's panes.
    // We'll add this overlay to the overlayImage pane.
    var panes = this.getPanes();
    panes.overlayLayer.appendChild(div);

    var me = this;

    

}

TruckMarkerOverlay.prototype.draw = function () {

    // Size and position the overlay. We use a southwest and northeast
    // position of the overlay to peg it to the correct position and size.
    // We need to retrieve the projection from this overlay to do this.
    var overlayProjection = this.getProjection();

    // Retrieve the southwest and northeast coordinates of this overlay
    // in latlngs and convert them to pixels coordinates.
    // We'll use these coordinates to resize the DIV.
    //var sw = overlayProjection.fromLatLngToDivPixel(this.bounds_.getSouthWest());
    //var ne = overlayProjection.fromLatLngToDivPixel(this.bounds_.getNorthEast());

    var position = overlayProjection.fromLatLngToDivPixel(this.position_);

    // Resize the image's DIV to fit the indicated dimensions.
    var div = this.div_;
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
    //div.style.width = (ne.x - sw.x) + 'px';
    //div.style.height = (sw.y - ne.y) + 'px';

    //var img = this.image_;
    //img.rotate(this.rotation_);

    $("#" + this.id_).rotate(this.rotation_);
}

TruckMarkerOverlay.prototype.onRemove = function () {
    this.div_.parentNode.removeChild(this.div_);
    this.div_ = null;
    // Label is removed from the map, stop updating its position/text.
    for (var i = 0, I = this.listeners_.length; i < I; ++i) {
        google.maps.event.removeListener(this.listeners_[i]);
    }

}

TruckMarkerOverlay.prototype.setPosition = function (position) {
    var overlayProjection = this.getProjection();
    var newPos = overlayProjection.fromLatLngToDivPixel(position);
    var div = this.div_;
    div.style.left = newPos.x + 'px';
    div.style.top = newPos.y + 'px';
};

TruckMarkerOverlay.prototype.setRotation = function (rotation) {
    $("#" + this.id_).rotate(rotation);
};

TruckMarkerOverlay.prototype.setStatus = function (imageUrl) {
    this.image_.src = imageUrl;
};