//adapded from this example http://code.google.com/apis/maps/documentation/javascript/overlays.html#CustomOverlays
//text overlays
function TxtOverlay(id, pos, txt, desc,  map, visibility, cssText) {

    // Now initialize all properties.
    this.id = id;
    this.pos = pos;
    this.txt_ = txt;
    this.desc = desc;   
    this.map_ = map;
    this.visibility = visibility;
    this.cssText = cssText;
    this.zIndex = 1;

    this.div_;

    // Explicitly call setMap() on this overlay
    this.setMap(map);   
}

TxtOverlay.prototype = new google.maps.OverlayView();



TxtOverlay.prototype.onAdd = function () {
   
    // Create the DIV and set some basic attributes.
    var div = document.createElement('DIV');
    //div.className = this.cls_;	
    div.innerHTML = this.txt_;

    // Set the overlay's div_ property to this DIV
    this.div_ = div;
    var overlayProjection = this.getProjection();
    var position = overlayProjection.fromLatLngToDivPixel(this.pos);
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
    div.style.zIndex = '1';
    // We add an overlay to a map via one of the map's panes.

    var panes = this.getPanes();
    panes.floatPane.appendChild(div);


    var me = this;
    google.maps.event.addDomListener(me.div_, 'click', function () {
        google.maps.event.trigger(me, 'click');
    })
    google.maps.event.addDomListener(me.div_, 'mouseover', function () {
        google.maps.event.trigger(me, 'mouseover');
    })
    google.maps.event.addDomListener(me.div_, 'mouseout', function () {
        google.maps.event.trigger(me, 'mouseout');
    })

}
TxtOverlay.prototype.draw = function () {

    var overlayProjection = this.getProjection();
    var position = overlayProjection.fromLatLngToDivPixel(this.pos);

    this.div_.style.cssText = this.cssText; 
    this.div_.style.left = position.x + 'px';
    this.div_.style.top = position.y + 'px';
    this.div_.style.visibility = this.visibility;
    this.div_.style.zIndex = '1';

}

TxtOverlay.prototype.onRemove = function () {
    this.div_.parentNode.removeChild(this.div_);
    this.div_ = null;
}

TxtOverlay.prototype.toggle = function (visibility) {
    this.visibility = visibility;
    this.div_.style.visibility = this.visibility;
}

TxtOverlay.prototype.toggleDOM = function () {
    if (this.getMap()) {
        this.setMap(null);
    }
    else {
        this.setMap(this.map_);
    }
}
