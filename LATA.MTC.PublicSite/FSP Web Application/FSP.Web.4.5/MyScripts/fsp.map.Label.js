//adapded from this example http://code.google.com/apis/maps/documentation/javascript/overlays.html#CustomOverlays
//text overlays
function TxtOverlay(opt_options) {

    // Initialization
    this.setValues(opt_options);

    // Label specific
    var span = this.span_ = document.createElement('span');
    span.style.cssText = 'position: relative; left: 0%; top: 0px; border-radius: 3px; ' +
                         'white-space: nowrap; border: 1px solid blue; ' +
                         'padding: 3px; background-color: ' + this.get('color');
    span.onmouseover = function () {
        alert('over');
    };
    span.onmouseout = function () {
        alert('out');
    };
    span.click = function () {
        alert('click');
    };

    this.span_ = span;

    var div = this.div_ = document.createElement('div');
    var button = document.createElement('INPUT');
    button.type = 'button';
    button.value = 'Click';
    div.appendChild(button);
    div.appendChild(span);
    div.style.cssText = 'position: absolute; background-color: ' + this.get('color');
    
}

TxtOverlay.prototype = new google.maps.OverlayView();

TxtOverlay.prototype.onAdd = function () {
   
    var pane = this.getPanes().overlayLayer;

    //var div_parent = document.createElement("DIV");
    //div_parent.appendChild(this.div_);
    pane.appendChild(this.div_);

 
   // Ensures the label is redrawn if the text or position is changed.
    var me = this;
   
    this.listeners_ = [
      google.maps.event.addListener(this, 'position_changed',
          function () { me.draw(); }),
      google.maps.event.addListener(this, 'text_changed',
          function () { me.draw(); }),
      
    ];

    //google.maps.event.addListener(this.div_, 'click', function () {
    //    google.maps.event.trigger(me, 'click');
    //})
    //google.maps.event.addListener(this.div_, 'mouseover', function () {
    //    google.maps.event.trigger(me, 'mouseover');
    //})
    //google.maps.event.addListener(this.div_, 'mouseout', function () {
    //    google.maps.event.trigger(me, 'mouseout');
    //})
    
    google.maps.event.addDomListener(this.div_, 'click', function () {
        google.maps.event.trigger(me, 'click');
    })
    google.maps.event.addDomListener(this.div_, 'mouseover', function () {
        google.maps.event.trigger(me, 'mouseover');
    })
    google.maps.event.addDomListener(this.div_, 'mouseout', function () {
        google.maps.event.trigger(me, 'mouseout');
    })

}
TxtOverlay.prototype.draw = function () {

    var projection = this.getProjection();
    var position = projection.fromLatLngToDivPixel(this.get('position'));

    var div = this.div_;
    div.style.left = position.x + 'px';
    div.style.top = position.y + 'px';
    div.style.display = 'block';

    this.span_.innerHTML = this.get('text').toString();
    
}

TxtOverlay.prototype.onRemove = function () {
    this.div_.parentNode.removeChild(this.div_);

    // Label is removed from the map, stop updating its position/text.
    for (var i = 0, I = this.listeners_.length; i < I; ++i) {
        google.maps.event.removeListener(this.listeners_[i]);
    }
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
